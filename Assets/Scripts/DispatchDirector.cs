using System.Collections;
using System.Linq;
using UnityEngine;

public class DispatchDirector : MonoBehaviour
{
    [SerializeField] private DispatchHunter[] _battleHunter = new DispatchHunter[4];
    [SerializeField] private SpriteRenderer _portal;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private DispatchMonster _battleMonster;
    [SerializeField] private Transform _transition;
    [SerializeField] private Sprite _natureBackground;
    [SerializeField] private Sprite _hellBackground;

    public void SetHunter(int index, Hunter hunter)
    {
        if (index < 0 || index >= 4)
            Debug.LogError("Invalid index");

        _battleHunter[index].Hunter = hunter;
    }

    public IEnumerator BattleRoutine()
    {
        var battleHunter = _battleHunter.Where(hunter => hunter.Hunter != null).ToArray();

        for (int i = battleHunter.Length - 1; i >= 1; i--)
        {
            StartCoroutine(battleHunter[i].EnterPortalRoutine(_portal.transform));
            yield return new WaitForSeconds(0.5f);
        }

        yield return battleHunter[0].EnterPortalRoutine(_portal.transform);

        var transitionStart = _transition.position;
        yield return MotionUtil.MoveEaseOutRoutine(_transition, transform.position, 1f);
        _portal.enabled = false;
        _battleMonster.AvatarCustomize.ShowAvatar();

        for (int i = 0; i < battleHunter.Length; i++)
        {
            battleHunter[i].transform.position = battleHunter[i].StartPosition;
            battleHunter[i].AvatarCustomize.ShowAvatar();
        }

        _background.sprite = _hellBackground;

        yield return new WaitForSeconds(0.7f);
        yield return MotionUtil.MoveEaseInRoutine(_transition, transitionStart, 1f);

        var hunterDeath = new[] { true, false, true, false };

        for (int i = battleHunter.Length - 1; i >= 0; i--)
        {
            yield return battleHunter[i].AttackBeginRoutine(_battleMonster.StartPosition - new Vector2(0.3f, 0));
            if (!hunterDeath[i])
            {
                _battleMonster.Death();
            }
            yield return new WaitForSeconds(0.5f);

            yield return battleHunter[i].AttackEndRoutine();
            yield return new WaitForSeconds(0.5f);

            if (!hunterDeath[i])
            {
                yield return _battleMonster.RespawnRoutine();
            }
            else
            {
                yield return _battleMonster.AttackBeginRoutine(battleHunter[i].StartPosition + new Vector2(0.3f, 0));
                battleHunter[i].Death();
            }

            yield return new WaitForSeconds(0.5f);

            yield return _battleMonster.AttackEndRoutine();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            _battleHunter[i].Initialize();
        }
        _battleMonster.Initialize();
        _portal.enabled = true;
        _background.sprite = _natureBackground;
    }
}
