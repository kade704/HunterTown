using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : Building
{
    public enum Ability
    {                                                                      //완료 여부
        Cristal,            //크리스탈 포탈   : 자원 +20%   
        Peace,              //평화의 문       : 유닛 사망률 -2%
        Know,               //파악된 세계     : 던전 생성 시 모든 특성 파악      1
        Good_Temper,        //적절한 기온     : 파견 시간 -10%                   1
        View,               //절경            : 증가하는 능력치 +2               1
        Badland,            //황무지          : 자원 -50%                        1
        Danger_Creatures,   //전투 생물       : 위험도 +30%                      1  
        Maze,               //미로            : 복잡도 +30%                      1
        Mutation,           //돌연변이        : 포탈 능력치 +20%         
        Lush_Forest,        //우거진 숲       : 최종 유닛 사망률 +3%              
        Glacier,            //빙하지역        : 파견 시간 +30%                    1  
        Lava,               //용암지역        : 포탈 성공확률 -5%                 
        OverCrow,           //과밀도          : 포탈 웨이브 시간 -50%             1
        Poision,            //독성 대기       : 실패 시 100%확률로 전멸           1  
        Undead,             //언데드          : 능력치가 감소되지 않음           1
        _MAX_,
    }

    public const float CristalVar = 0.2f;      //크리스탈 포탈
    public const float PeaceVar = 0.02f;     //평화의문
    public const float GoodTemperVar = 0.1f;  //적절한 기온
    public const int ViewVar = 2;
    public const float BadlandVar = 0.5f;
    public const float LushForestVar = 0.03f;


    [SerializeField] private int _defaultPower;
    [SerializeField] private bool _powerVisibility;
    [SerializeField] private int _defaultDanger;
    [SerializeField] private bool _dangerVisibility;
    [SerializeField] private int _portalMaze;
    [SerializeField] private int _portalBaseMaze;
    [SerializeField] private bool _mazeVisibility;
    [SerializeField] private List<Ability> _abilities = new List<Ability>();
    [SerializeField] private bool[] _abilityVisibilities = new bool[3];
    [SerializeField] private Hunter[] disPatchedHunters = new Hunter[4];

    public int DefaultPower { get { return _defaultPower; } set { _defaultPower = value; } }
    public int DefaultDanger { get { return _defaultDanger; } set { _defaultDanger = value; } }
    public bool PowerVisibility { get { return _powerVisibility; } set { _powerVisibility = value; } }
    public bool DangerVisibility { get { return _dangerVisibility; } set { _dangerVisibility = value; } }
    public bool MazeVisibility { get { return _mazeVisibility; } set { _mazeVisibility = value; } }

    public List<Ability> Abilities { get { return _abilities; } set { _abilities = value; } }
    public bool[] AbilityVisibilities => _abilityVisibilities;


    public float Unit_Die_Base_Count(Hunter hunter, bool disPatch_Pass)
    {
        float var = 0;
        int survivalPower = hunter.GetSurvivalPower();
        float die_Per = 0f;
        if (hunter.Abilities.Contains(Hunter.Ability.Sixth_Sense))
        {
            var += 0.03f;
        }
        if (hunter.Abilities.Contains(Hunter.Ability.Weakness))
        {
            var -= 0.07f;
        }
        if (disPatch_Pass)
        {
            die_Per = 1f - ((float)survivalPower / _defaultDanger);
        }
        else
        {
            die_Per = 1f - ((float)survivalPower / (_defaultDanger * 2f));
        }
        die_Per += var;

        if (die_Per < 0f)
        {
            die_Per = 0f;
        }
        return die_Per;
    }

    public float Unit_Die_Account(Hunter hunter, bool disPatch_Pass)
    {
        float unit_Die_Per = Unit_Die_Base_Count(hunter, disPatch_Pass);
        if (_abilities.Contains(Ability.Lush_Forest))
        {
            unit_Die_Per += LushForestVar;
        }
        if (_abilities.Contains(Ability.Peace))
        {
            unit_Die_Per += 0.02f;
        }
        if (unit_Die_Per < 0)
        {
            unit_Die_Per = 0;
        }
        return unit_Die_Per;
    }


}
