using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Durable : MonoBehaviour
{
    [SerializeField]
    private int _durability;

    private Construction _construction;

    private SpriteRenderer _progressRenderer;
    private int _startDurability;
    private Coroutine _recoverDurabilityRoutine;
    private UnityEvent _onDurabilityChanged = new UnityEvent();

    public Construction Construction => _construction;
    public UnityEvent OnDurabilityChanged => _onDurabilityChanged;

    public int Durability
    {
        get => _durability;
        set
        {
            _durability = value;
            _onDurabilityChanged.Invoke();
        }
    }

    private void Awake()
    {
        _construction = GetComponent<Construction>();
        _progressRenderer = transform.Find("Durability").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _startDurability = Durability;

        _onDurabilityChanged.AddListener(DurabilityChanged);
    }

    private void DurabilityChanged()
    {
        if (Durability <= 0)
        {
            _construction.ConstructionGridMap.DestroyConstruction(_construction);
            GameManager.Instance.GetSystem<AudioController>().PlaySFX("Destruction");
        }
        else
        {
            if (Durability < _startDurability)
            {
                _progressRenderer.enabled = true;
                _progressRenderer.material.SetFloat("_Value", (float)Durability / _startDurability);
                if (_recoverDurabilityRoutine != null)
                {
                    StopCoroutine(_recoverDurabilityRoutine);
                }
                _recoverDurabilityRoutine = StartCoroutine(RecoverDurabilityRoutine());
            }
            else
            {
                _progressRenderer.enabled = false;
            }
        }
    }

    private IEnumerator RecoverDurabilityRoutine()
    {
        yield return new WaitForSeconds(10f);
        while (Durability < _startDurability)
        {
            Durability += 1;

            yield return new WaitForSeconds(1f);
        }
    }
}
