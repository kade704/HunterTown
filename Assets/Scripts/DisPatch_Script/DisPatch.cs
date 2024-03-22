using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisPatch : MonoBehaviour
{
    #region 변수
    //파견 유닛 최대치
    private const int max_Unit_DisPatch = 4;

    //파견나갈 유닛 리스트 최대4개
    public List<Unit> disPatch_Units = new List<Unit>();
    //파견 UI 지정
    private DisPatch_UI disPatch_UI;
    //파견 진행 여부
    private bool is_DisPatching;
    //파견을 진행할 포탈에 대한 Portal 스크립트
    private Portal portal;
    //파견을 진행할 모든 유닛의 전투력
    public int all_Of_Power;
    //파견 진행 시 실제 성공률
    public float clearChance;
    //파견 진행 시 UI에 표시될 성공률 : 포탈의 특성이 가려졌을 경우 등
    public float clearChance_UI;
    //파견 진행 시 유닛 별 실제 사망률
    public float[] disPatch_Die_C = new float[4];
    //파견 진행 시 유닛 별 UI에 표시될 성공률 : 포탈의 특성이 가려졌을 경우 등
    public float[] disPatch_Die_C_UI = new float[4];
    /*
    파견에 사용할 아이템 - 아이템 관련 스크립트 작성 후 작성 예정
    파견에는 한번에 하나의 아이템만 사용.
    */
    #endregion

    #region 포탈 초기화 및 배정, 클릭 처리
    //포탈 배정
    private void Start()
    {
        portal = this.gameObject.GetComponent<Portal>();
    }

    //포탈 오브젝트 풀링으로 구현 예정, 파견 진행 여부 false로 지정
    private void OnEnable()
    {
        is_DisPatching = false;  
    }

    //Mouse_Click에서 실행, 파견UI 배정, 파견 스크립트 초기화
    public void SetDis_UI(DisPatch_UI UI)
    {
        //파라미터로 받아온 파견UI 배정
        disPatch_UI = UI;
        if (!is_DisPatching) //파견 중이 아니라면
        {
            disPatch_Units.Clear(); //파견중인 유닛 초기화
            all_Of_Power = 0;   //총 공격력 0
            clearChance = 0;    //클리어 실 확률 0
            clearChance_UI = 0; //클리어 UI 표시 확률 0
            //파견 사망률(표시,실 사망률) 0
            for(int i = 0; i < disPatch_Die_C.Length; i++)
            {
                disPatch_Die_C[i] = 0;
                disPatch_Die_C_UI[i] = 0;
            }
            disPatch_UI.Setting_DisPatch_Chance_UI();
        }
    }
    #endregion

    #region 파견 배치 관련
    //유닛 파견에 배치
    public void DisPatch_Input_Unit(Unit unit)
    {
        //유닛 파견 최대치보다 적으면
        if (disPatch_Units.Count < max_Unit_DisPatch)
        {
            Debug.Log("최대 인원 이하");
            //파라미터로 받은 유닛이 파견에 포함된 유닛인지 확인
            if (!disPatch_Units.Contains(unit))
            {
                Debug.Log("파견에 없는 인원");
                disPatch_Units.Add(unit);
                Debug.Log("파견추가 " + unit.unit_name);
                DisPatch_Setting();
            }
        }
    }

    //파견 인원의 총 전투력을 계산하는 코드 실행
    public void DisPatch_Power_Check()
    {
        all_Of_Power = GameManager.Instance.GetDisPatch_Account().GetPower_Account().DisPatch_Power_Count(disPatch_Units);
    }

    //사망률 계산 코드
    public void DisPatch_Unit_Die()
    {
        disPatch_Die_C = null;
        disPatch_Die_C = GameManager.Instance.GetDisPatch_Account().GetDie_Account().DisPatch_Die_Count(portal,disPatch_Units,true);
        disPatch_Die_C_UI = null;
        disPatch_Die_C_UI = GameManager.Instance.GetDisPatch_Account().GetDie_Account().DisPatch_Die_UI_Account(portal, disPatch_Units);
    }

    //포탈 파견 성공률 계산 코드
    public void DisPatch_Clear_Chance()
    {
        clearChance = GameManager.Instance.GetDisPatch_Account().GetClear_Account().DisPatch_Clear_Count(portal, all_Of_Power);
        Debug.Log("던전 클리어 확률 : " + clearChance);
        clearChance_UI = GameManager.Instance.GetDisPatch_Account().GetClear_Account().DisPatch_Clear_UI_Account(portal, all_Of_Power);
    }

    //파견에서 유닛 제거(제거할 위치)
    public void DisPatrch_Unput_Unit(int num)
    {
        disPatch_Units.RemoveAt(num);
        DisPatch_Setting();
    }

    //파견 UI최신화
    public void DisPatch_Setting()
    {
        DisPatch_Power_Check();
        DisPatch_Clear_Chance();
        DisPatch_Unit_Die();
        disPatch_UI.Setting_DisPatch_Chance_UI();
    }
    #endregion

    #region 파견
    public void DisPatch_Start()
    {
        
    }
    

    #endregion
}