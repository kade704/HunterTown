using System.Collections;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] private BattleHunter[] _battleHunter = new BattleHunter[4];
    [SerializeField] private SpriteRenderer _portal;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private BattleMonster _monster;
    [SerializeField] private Transform _transition;
    [SerializeField] private Sprite _natureBackground;
    [SerializeField] private Sprite _hellBackground;

    public void SetBattleHunter(int index, Hunter hunter)
    {
        if (hunter != null)
        {
            _battleHunter[index].AvatarCustomize.ShowAvatar();
            _battleHunter[index].AvatarCustomize.CopyAvatar(hunter.GetComponent<AvatarCustomize>());
        }
        else
        {
            _battleHunter[index].AvatarCustomize.HideAvatar();
        }
    }

    public IEnumerator BattleRoutine()
    {
        for (int i = _battleHunter.Length - 1; i >= 1; i--)
        {
            if (_battleHunter[i].AvatarCustomize.IsVisible)
            {
                StartCoroutine(_battleHunter[i].EnterPortalRoutine(_portal.transform));
                yield return new WaitForSeconds(0.5f);
            }
        }

        yield return _battleHunter[0].EnterPortalRoutine(_portal.transform);

        var transitionStart = _transition.position;
        yield return MotionUtil.MoveEaseOutRoutine(_transition, transform.position, 1f);
        _portal.enabled = false;
        _monster.AvatarCustomize.ShowAvatar();

        for (int i = 0; i < _battleHunter.Length; i++)
        {
            _battleHunter[i].transform.position = _battleHunter[i].StartPosition;
            _battleHunter[i].AvatarCustomize.ShowAvatar();
        }

        _background.sprite = _hellBackground;

        yield return new WaitForSeconds(0.7f);
        yield return MotionUtil.MoveEaseInRoutine(_transition, transitionStart, 1f);

        var hunterDeath = new[] { true, false, true, false };

        for (int i = _battleHunter.Length - 1; i >= 0; i--)
        {
            if (_battleHunter[i].AvatarCustomize.IsVisible)
            {
                yield return _battleHunter[i].AttackBeginRoutine(_monster.StartPosition - new Vector2(0.3f, 0));
                if (!hunterDeath[i])
                {
                    _monster.Death();
                }
                yield return new WaitForSeconds(0.5f);

                yield return _battleHunter[i].AttackEndRoutine();
                yield return new WaitForSeconds(0.5f);

                if (!hunterDeath[i])
                {
                    yield return _monster.RespawnRoutine();
                }
                else
                {
                    yield return _monster.AttackBeginRoutine(_battleHunter[i].StartPosition + new Vector2(0.3f, 0));
                    _battleHunter[i].Death();
                }

                yield return new WaitForSeconds(0.5f);

                yield return _monster.AttackEndRoutine();
                yield return new WaitForSeconds(0.5f);

            }
        }
    }
}
