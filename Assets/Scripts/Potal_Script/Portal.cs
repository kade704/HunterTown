using System.Collections.Generic;
using UnityEngine;

public class Portal : Building
{
    #region 포탈 특성
    //포탈 특성
    public enum Portal_Ability
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
    #endregion

    #region 포탈 특성 수치    
    public const float Cristal_Var = 0.2f;      //크리스탈 포탈
    public const float Peace_Var = 0.02f;     //평화의문
    //파악된 세계
    public const float Good_Temper_Var = 0.1f;  //적절한 기온
    public const int View_Var = 2;
    public const float Badland_Var = 0.5f;
    public const float Lush_Forest_Var = 0.03f;

    #endregion

    #region 포탈 능력치 관련 변수
    //포탈 명
    public string portalName;
    //포탈 난이도(특성 적용)
    public int portalPower;
    //포탈 난이도(특성 미적용)
    public int portalBasePower;
    //포탈 난이도 공개 여부
    public bool can_See_PortalPower;
    //포탈 위험도(특성 적용)
    public int portalDanger;
    //포탈 위험도(특성 미적용)
    public int portalBaseDanger;
    //포탈 위험도 공개 여부
    public bool can_See_PortalDanger;
    //포탈 복잡도(특성 적용)
    public int portalMaze;
    //포탈 복잡도(특성 미적용)
    public int portalBaseMaze;
    //포탈 복잡도 공개 여부
    public bool can_See_PortalMaze;
    //포탈 특성
    public List<Portal_Ability> ability = new List<Portal_Ability>();
    //포탈 특성 공개 여부
    public bool[] can_See_Ablitiy = new bool[3];
    #endregion


    //포탈 웨이브까지 남은 시간(주 단위)
    private int overTime;
    private bool is_Wave = false;

    //포탈 타이머 시작Start
    private void Start()
    {
        Debug.Log("포탈 생성");
    }


    public void Portal_Factory(string name, int Power, int Danger, int Maze)
    {
        this.gameObject.SetActive(true);
        portalName = name; portalPower = Power; portalDanger = Danger; portalMaze = Maze;

    }

    //포탈 타이머 코루틴 - 실험용 이후 교체할 것 포탈
    /*게임 시간에 영향을 받는 코드가 좀 더 있을 것 같으니 GameManager에서 브로드캐스팅으로
     구현할 것도 생각 해 볼 만 함*/

    //포탈 웨이브 시작
    public void Portal_Wave_Sp()
    {
        Debug.Log("포탈 웨이브 실행");
    }


    //게임 타이머 작동 
    public void Timmer_Check()
    {
        if (is_Wave)
        {
            return;
        }
        if (overTime >= 0)
        {
            Portal_Wave_Sp();
        }
        else
        {
            overTime--;
        }
    }
}