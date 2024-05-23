using System.Collections;
using System.Linq;
using UnityEngine;

public class DispatchDirector : MonoBehaviour
{
    [SerializeField] private Sprite _natureBackground;
    [SerializeField] private Sprite _hellBackground;

    private DispatchHunter[] _dispatchHunters;
    private DispatchMonster _monster;
    private SpriteRenderer _portal;
    private SpriteRenderer _background;

    public DispatchHunter[] DispatchHunters => _dispatchHunters;

    private void Awake()
    {
        _dispatchHunters = GetComponentsInChildren<DispatchHunter>();
        _monster = GetComponentInChildren<DispatchMonster>();
        _portal = transform.Find("Portal").GetComponent<SpriteRenderer>();
        _background = transform.Find("Background").GetComponent<SpriteRenderer>();
    }

    public void SetHunter(int index, Hunter hunter, Portal portal)
    {
        if (index < 0 || index >= 4)
            Debug.LogError("Invalid index");

        _dispatchHunters[index].Hunter = hunter;

        if (hunter != null)
        {
            var death = Random.Range(0, 1) > portal.CalcHunterDeathProbability(hunter);
            _dispatchHunters[index].Death = death;

            var reward = portal.HunterDispatchReward;
            var damage = Random.Range(0, reward);
            var hp = reward - damage;
            _dispatchHunters[index].IncleaseDamage = damage;
            _dispatchHunters[index].IncleaseHP = hp;
        }
    }

    public IEnumerator EnterPortal()
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = dispatchHunters.Length - 1; i >= 1; i--)
        {
            StartCoroutine(dispatchHunters[i].EnterPortalRoutine(_portal.transform));
            yield return new WaitForSeconds(0.5f);
        }

        yield return dispatchHunters[0].EnterPortalRoutine(_portal.transform);
    }

    public void PrepareBattle()
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        _portal.enabled = false;
        _monster.AvatarCustomize.ShowAvatar();

        for (int i = 0; i < dispatchHunters.Length; i++)
        {
            dispatchHunters[i].transform.position = dispatchHunters[i].StartPosition;
            dispatchHunters[i].AvatarCustomize.ShowAvatar();
        }

        _background.sprite = _hellBackground;
    }

    public IEnumerator BattleRoutine(Portal portal)
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = dispatchHunters.Length - 1; i >= 0; i--)
        {
            yield return dispatchHunters[i].AttackBeginRoutine(_monster.StartPosition - new Vector2(0.3f, 0));

            if (!dispatchHunters[i].Death)
            {
                _monster.Die();
                yield return new WaitForSeconds(0.5f);
            }

            yield return dispatchHunters[i].AttackEndRoutine();
            yield return new WaitForSeconds(0.5f);

            if (!dispatchHunters[i].Death)
            {
                yield return _monster.RespawnRoutine();
            }
            else
            {
                yield return _monster.AttackBeginRoutine(dispatchHunters[i].StartPosition + new Vector2(0.3f, 0));
                dispatchHunters[i].Die();
                GameManager.Instance.GetSystem<LoggerSystem>().LogError($"{dispatchHunters[i].Hunter.DisplayName}이(가) 사망했습니다.");
                GameManager.Instance.GetSystem<HunterSpawner>().RemoveHunter(dispatchHunters[i].Hunter);
            }

            yield return new WaitForSeconds(0.5f);

            yield return _monster.AttackEndRoutine();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void FinishBattle(Portal portal)
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        if (dispatchHunters.Any(hunter => !hunter.Death))
        {
            var earnedMoney = portal.Power * 10f;
            if (portal.ContainAbility("crystal_portal"))
            {
                earnedMoney *= 1.2f;
            }
            else if (portal.ContainAbility("badlands"))
            {
                earnedMoney *= 0.5f;
            }
            GameManager.Instance.GetSystem<Player>().Money += (int)earnedMoney;

            foreach (var dispatchHunter in dispatchHunters)
            {
                dispatchHunter.Hunter.DefaultDamage += dispatchHunter.IncleaseDamage;
                dispatchHunter.Hunter.DefaultHp += dispatchHunter.IncleaseHP;
            }

            GameManager.Instance.GetSystem<LoggerSystem>().LogInfo("파견에 성공했습니다. 포탈이 사라집니다.");
            GameManager.Instance.GetSystem<PortalGenerator>().RemovePortal(portal.GetComponent<Portal>());
        }
        else
        {
            GameManager.Instance.GetSystem<LoggerSystem>().LogError("파견에 실패했습니다.");
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            _dispatchHunters[i].Initialize();
        }
        _monster.Initialize();
        _portal.enabled = true;
        _background.sprite = _natureBackground;
    }
}
