using System.Collections;
using System.Linq;
using UnityEngine;

public class DispatchDirector : MonoBehaviour
{
    private Transform _camera;
    private DispatchHunter[] _dispatchHunters;
    private DispatchMonster[] _dispatchMonster;
    private Transform _dispatchEnterPortal;
    private Transform _dispatchExitPortal;

    public DispatchHunter[] DispatchHunters => _dispatchHunters;

    private void Awake()
    {
        _camera = transform.Find("Camera");
        _dispatchHunters = GetComponentsInChildren<DispatchHunter>();
        _dispatchMonster = GetComponentsInChildren<DispatchMonster>();
        _dispatchEnterPortal = transform.Find("EnterPortal");
        _dispatchExitPortal = transform.Find("ExitPortal");
    }

    public IEnumerator EnterPortal()
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = 0; i < dispatchHunters.Length; i++)
        {
            dispatchHunters[i].SetMovement(true);
            yield return MotionUtil.MoveToRoutine(dispatchHunters[i].transform, _dispatchEnterPortal.position, 2);
            dispatchHunters[i].SetMovement(false);
            dispatchHunters[i].AvatarCustomize.HideAvatar();
        }
    }

    public IEnumerator ExitPortal()
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = 0; i < dispatchHunters.Length; i++)
        {
            dispatchHunters[i].AvatarCustomize.ShowAvatar();
            dispatchHunters[i].SetMovement(true);
            yield return MotionUtil.MoveToRoutine(dispatchHunters[i].transform, dispatchHunters[i].StartPosition + new Vector2(0, -3), 2);
            dispatchHunters[i].SetMovement(false);
        }
    }

    public void PrepareBattle()
    {
        _camera.transform.localPosition = new Vector3(0, -3, -10);
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = 0; i < dispatchHunters.Length; i++)
        {
            dispatchHunters[i].transform.position = _dispatchExitPortal.position;
        }
    }

    private IEnumerator MarchRoutine()
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        var aliveHunters = dispatchHunters.Where(hunter => !hunter.IsDeath).ToArray();

        for (int i = 0; i < aliveHunters.Length; i++)
        {
            aliveHunters[i].SetMovement(true);
        }

        for (int i = 0; i < aliveHunters.Length; i++)
        {
            StartCoroutine(MotionUtil.MoveToRoutine(aliveHunters[i].transform, (Vector2)aliveHunters[i].transform.position + new Vector2(4, 0), 1));
        }
        yield return MotionUtil.MoveToRoutine(_camera.transform, (Vector2)_camera.transform.position + new Vector2(4, 0), 1);

        for (int i = 0; i < aliveHunters.Length; i++)
        {
            aliveHunters[i].SetMovement(false);
        }
    }

    public IEnumerator BattleRoutine()
    {
        yield return ExitPortal();

        yield return new WaitForSeconds(1f);

        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = 0; i < dispatchHunters.Length; i++)
        {
            yield return MarchRoutine();

            yield return new WaitForSeconds(1f);

            var startPosition = (Vector2)_dispatchHunters[i].transform.position;

            _dispatchHunters[i].SetFlip(true);
            _dispatchHunters[i].SetMovement(true);
            yield return MotionUtil.MoveToRoutine(_dispatchHunters[i].transform, (Vector2)_dispatchMonster[i].transform.position - new Vector2(0.3f, 0), 3);
            _dispatchHunters[i].SetMovement(false);

            var attackCount = Random.Range(0, 4);
            _dispatchHunters[i].Attack();
            _dispatchMonster[i].Attack();
            for (int j = 0; j < attackCount; j++)
            {
                yield return new WaitForSeconds(1);
                _dispatchHunters[i].Attack();
                _dispatchMonster[i].Attack();
            }

            if (_dispatchHunters[i].WillDeath)
            {
                _dispatchHunters[i].Die();
                _dispatchHunters[i].IsDeath = true;
            }
            else
            {
                _dispatchMonster[i].Die();

                yield return new WaitForSeconds(0.5f);

                _dispatchHunters[i].SetFlip(false);
                _dispatchHunters[i].SetMovement(true);
                yield return MotionUtil.MoveToRoutine(_dispatchHunters[i].transform, startPosition, 3);
                _dispatchHunters[i].SetMovement(false);
                _dispatchHunters[i].SetFlip(true);
            }

            yield return new WaitForSeconds(1);
        }
    }

    public void FinishBattle(Portal portal)
    {
        var dispatchHunters = _dispatchHunters.Where(hunter => hunter.Hunter != null).ToArray();

        if (dispatchHunters.Any(hunter => !hunter.WillDeath))
        {
            GameManager.Instance.GetSystem<MoneySystem>().Money += portal.Reward;

            GameManager.Instance.GetSystem<NotificationSystem>().NotifyInfo("파견에 성공했습니다. 포탈이 사라집니다.");
            GameManager.Instance.GetSystem<PortalGenerator>().RemovePortal(portal.GetComponent<Portal>());
        }
        else
        {
            GameManager.Instance.GetSystem<NotificationSystem>().NotifyError("파견에 실패했습니다.");
        }

        foreach (var dispatchHunter in dispatchHunters)
        {
            if (dispatchHunter.WillDeath)
            {
                GameManager.Instance.GetSystem<NotificationSystem>().NotifyError($"{dispatchHunter.Hunter.Interactable.DisplayName}이(가) 사망했습니다.");
                GameManager.Instance.GetSystem<HunterSpawner>().RemoveHunter(dispatchHunter.Hunter);
            }
            else
            {
                dispatchHunter.Hunter.DefaultDamage += dispatchHunter.IncleaseDamage;
                dispatchHunter.Hunter.DefaultHp += dispatchHunter.IncleaseHP;
            }
        }
    }

    public void Initialize(Portal portal)
    {
        _camera.localPosition = Vector3.zero;

        var hunters = portal.Visitable.VisitedHunters;
        for (int i = 0; i < 4; i++)
        {
            if (i < hunters.Length)
            {
                var death = Random.value < portal.CalcHunterDeathProbability(hunters)[i];
                _dispatchHunters[i].WillDeath = death;
                var reward = portal.HunterDispatchReward;
                var damage = Random.Range(0, reward);
                var hp = reward - damage;
                _dispatchHunters[i].Hunter = hunters[i];
                _dispatchHunters[i].IncleaseDamage = damage;
                _dispatchHunters[i].IncleaseHP = hp;
                _dispatchHunters[i].AvatarCustomize.ShowAvatar();

            }
            else
            {
                _dispatchHunters[i].WillDeath = false;
                _dispatchHunters[i].IncleaseDamage = 0;
                _dispatchHunters[i].IncleaseHP = 0;
                _dispatchHunters[i].Hunter = null;
                _dispatchHunters[i].AvatarCustomize.HideAvatar();
            }

            _dispatchHunters[i].Respawn();
            _dispatchHunters[i].SetFlip(true);
            _dispatchHunters[i].SetMovement(false);
            _dispatchHunters[i].IsDeath = false;
            _dispatchHunters[i].transform.position = _dispatchHunters[i].StartPosition;

            _dispatchMonster[i].Respawn();
            _dispatchMonster[i].RandomCustomize();
        }
    }
}
