using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public enum Ability
    {
        None,
        Leader,                 //리더쉽 있는 성격 - 파견 총 전투력 5%증가 -3
        Sixth_Sense,            //제6감 -  유닛 사망률 3% 감소 -4
        God_Of_Hammer,          //신들린 망치질 - 희귀재료 획득 1%  증가 -3 
        Indomitable_Power,      //불굴의 힘 - 해당 유닛 체력 20 % 증가 -2
        Attack_Stance,          //공격 태세 - 해당 유닛 공격력 20 % 증가 -2
        Skill_Hand,             //숙련된 손길 - 해당 유닛 손재주 20 % 증가 -2
        Guide,                  //길잡이  -  포탈 파견 시간 10% 감소 -1
        Handy_Hand,             //능숙한 손놀림 - 제련 시간 5 % 단축 -1
        Good_Learning_Power,    //뛰어난 학습력 - 능력치 증가 20% 확률로 150% 증가 -4
        Scout,                  //정찰 전문가  - 포탈 정찰 시 특성 파악 확률 몇퍼% 증가 -3
        Hero,                   //주인공 - 이 캐릭터 사망 시 30% 생존 후 능력치의 50% 증가  -6
        Good_Speaking_Skill,    //뛰어난 말솜씨 - 상점 가격 30% 할인 + 정책 성공 확률 10% 증가 + 훈련 시 모든 유닛 모든 능력치 1 증가  - 5
        Hephaestus,             //헤파이스토스 - 제련소에 배정 시 모든 유닛 능력치 10% 
        Genius,                 //천재 - 능력치 증가 시 +2 증가 -5
        Terrible_Direction,     //길치 - 포탈 파견 시간 15% 증가  + 2
        Weakness,               //병약 - 자신의 사망률 7% 증가 +4
        HandOfMinus,            //마이너스의 손 - 해당 유닛 손재주 30% 감소  + 3 
        Blue_Falcon,            //고문관 - 파견 총 전투력 7% 감소 +5
        Muscle_Loss,            //근손실 - 해당 유닛 체력 공격력 25% 감소 +4
        ForgetFulness,          //건망증 - 파견 보상 10% 감소 + 2
        ADHD,                   //ADHD -  능력치 증가 할 때 30% 증가 하지 않음 +5 
        Kleptomania,            //도벽 - 재련 시 20% 로 물건이 나오지 않음  + 3
    }
    [SerializeField] private string _displayName;
    [SerializeField] private int _defaultHp;
    [SerializeField] private int _defaultDamage;
    private bool _isDispatched = false;

    private List<Ability> _abilities = new List<Ability>();

    private SpriteRenderer _spriteRenderer;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public int DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public int DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public Ability[] Abilities => _abilities.ToArray();
    public Sprite Sprite => _spriteRenderer.sprite;

    public bool Dispatch
    {
        set
        {
            _isDispatched = value;
            _spriteRenderer.enabled = value;
        }
    }


    private void Awake()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        var _roads = ConstructionManager.Instance.RoadMap.Constructions.Cast<Road>().ToArray();
        Road start = null;
        if (_roads.Length > 0)
        {
            start = _roads[Random.Range(0, _roads.Length)];
            transform.position = start.transform.position;
        }
        StartCoroutine(IEnuMove(start));
    }

    void Update()
    {
        _spriteRenderer.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);
    }

    IEnumerator IEnuMove(Road start)
    {
        var _roads = ConstructionManager.Instance.RoadMap.Constructions.Cast<Road>().ToArray();
        Road target = null;
        if (_roads.Length > 0)
        {
            target = _roads[Random.Range(0, _roads.Length)];

            var path = PathFinder.SearchPath(start, target, _roads);

            int index = 0;

            while (index < path.Length)
            {
                while (Vector3.Distance(transform.position, path[index].transform.position) > 0.01)
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[index].transform.position, Time.deltaTime);
                    yield return null;
                }
                index++;
            }
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(IEnuMove(target));
    }

    public int GetHp()
    {
        float hpScale = 1;
        if (_abilities.Contains(Ability.Indomitable_Power))
        {
            hpScale += 0.2f;
        }
        if (_abilities.Contains(Ability.Muscle_Loss))
        {
            hpScale -= 0.25f;
        }
        return (int)(_defaultHp * hpScale);
    }

    public int GetDamage()
    {
        float damageScale = 1;
        if (_abilities.Contains(Ability.Attack_Stance))
        {
            damageScale += 0.2f;
        }
        if (_abilities.Contains(Ability.Muscle_Loss))
        {
            damageScale -= 0.25f;
        }
        return (int)(_defaultDamage * damageScale);
    }

    public int GetSurvivalPower()
    {
        return GetHp() + (int)(GetDamage() * 0.8f);
    }
}
