using System.Collections;
using System.Collections.Generic;
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
    }


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

    public Ability[] Abilities => _abilities.ToArray();
    public bool[] AbilityVisibilities => _abilityVisibilities;

}
