using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*DisPatch_UI
포탈을 선택 후 포탈의 능력,특성 등 표시해야 할 것을 표시하고, 출발할 유닛 설정 등
파견과 관련된 UI를 관리하는 스크립트
--------------------------------
Setting_UI  포탈 UI 설정 : 포탈 선택 시 Mouse_Click 로 부터 실행되는 코드로 UI에 
능력치 등 표시할 포탈을 이 코드에 배정하고 Text들을 변경하는 코드

Unit_SelectButton_Setting  유닛 선택 버튼 셋팅 : 보유한 유닛 수 만큼 버튼을 활성화
하고 버튼의 코드가 적용될 포탈의 DisPatch스크립트를 배정하는 코드

DisPatch_Unit_UI  파견 유닛 UI 갱신 : UnitSelectButton로 부터 실행되는 코드로 버튼
을 통해 변경된 파견을 진행할 유닛들을 표시하는 UI를 최신화 시키는 코드
 */


public class DisPatch_UI : MonoBehaviour
{
    #region 변수
    //특성
    [Tooltip("")]
    public Image ability1;
    public Image ability2;
    public Image ability3;

    public List<Image> disPatch_Select_Unit;

    //유닛 선택 버튼
    public Button[] selectButton = new Button[15];
    public Button[] unselectButton = new Button[4];

    //포탈 능력치
    public Text portal_Name;
    public Text portal_Power;
    public Text portal_Power_Rank;

    public Portal targetPortal;
    private DisPatch targetDisPatch;

    public Text[] disPatch_Unit_Text = new Text[4];

    public Text disPatch_All_Of_Power;
    public Text disPatch_Clear_Chance;
    #endregion
    #region UI 계산 코드
    //UI에 표시될 포탈의 능력치 (실제 능력치 / 기본 능력치) 반환
    private int Get_Portal_Power_UI(Portal portal)
    {
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Portal_Ability_Check(portal, Portal.Portal_Ability.Mutation))
        {
            return portal.portalBasePower;
        }
        else
        {
            return portal.portalPower;
        }
    }
    //표시될 랭크 설정
    public string Rank_Power(int rankpoint)
    {
        if (rankpoint <= 75)
        {
            return "F";
        }
        else if (rankpoint <= 200)
        {
            return "D";
        }
        else if (rankpoint <= 400)
        {
            return "C";
        }
        else if (rankpoint <= 700)
        {
            return "B";
        }
        else if (rankpoint <= 1000)
        {
            return "A";
        }
        else
        {
            return "S";
        }
    }

    #endregion

    public void Setting_UI(Portal portal)
    {
        //포탈 지정
        targetPortal = portal;
        //포탈에 배정된 DisPatch 스크립트 지정
        targetDisPatch = targetPortal.GetComponent<DisPatch>();
        //포탈 이름 UI에 표시
        portal_Name.text = targetPortal.portalName;
        //포탈 능력치 UI에 표시
        if (targetPortal.can_See_PortalPower)
        {
            int portal_Power_Text = Get_Portal_Power_UI(portal);
            portal_Power.text = portal_Power_Text.ToString();
            portal_Power_Rank.text = Rank_Power(portal_Power_Text);
        }
        else
        {
            portal_Power.text = "?";
            portal_Power_Rank.text = "?";
        }
        foreach(Button unselect in unselectButton)
        {
            unselect.gameObject.GetComponent<UnitSelectButton>().DisPatch_Unit_Button(targetDisPatch);
        }
        Unit_SelectButton_Setting();
        Setting_DisPatch_Chance_UI();
    }


    //유닛 선택 버튼 셋팅
    public void Unit_SelectButton_Setting()
    {
        for(int i = 0; i < selectButton.Length; i++)
        {
            //모든 버튼 비활성화
            selectButton[i].gameObject.SetActive(false);
        }
        Debug.Log(GameManager.Instance.unit_List.unitList.Count);
        //보유한 유닛 수만큼 버튼 활성화 및 버튼에 지정된 포탈의 DisPatch 배정
        for(int i = 0; i<GameManager.Instance.unit_List.unitList.Count; i++)
        {
            selectButton[i].gameObject.SetActive(true);
            selectButton[i].GetComponent<UnitSelectButton>().DisPatch_Unit_Button(targetDisPatch);
        }
    }
    //파견 유닛 UI 갱신 **아직 미구현 Debug로 임시 실행증
    public void DisPatch_Unit_UI(List<Unit> disUnits)
    {
        foreach(Unit unit in disUnits)
        {
            Debug.Log("파견 유닛"+unit.unit_name);
        }
    }

    //UI에 유닛 사망률, 파견 성공 확률 표시
    public void Setting_DisPatch_Chance_UI()
    {
        //사망률을 알려주는 모든 Text 비활성화
        for(int i = 0; i<disPatch_Unit_Text.Length; i++)
        {
            disPatch_Unit_Text[i].gameObject.SetActive(false);
            unselectButton[i].gameObject.SetActive(false);
        }

        for(int i = 0; i<targetDisPatch.disPatch_Units.Count;i++)
        {
            //유닛 수 만큼 사망률 Text 활성화
            disPatch_Unit_Text[i].gameObject.SetActive(true);
            unselectButton[i].gameObject.SetActive(true);
            //포탈 위험도 가려져 있을 시 ?로 표시
            if (!targetPortal.can_See_PortalDanger)
            {
                disPatch_Unit_Text[i].text = "사망 확률 : ?%";
            }
            else
            {
                disPatch_Unit_Text[i].text = "사망 확률 : " + (targetDisPatch.disPatch_Die_C_UI[i] * 100).ToString("F2") + "%";
            }
        }
        //배정한 유닛의 총 전투력 표시
        disPatch_All_Of_Power.text = "총 전투력 : " + targetDisPatch.all_Of_Power;
        //포탈 능력치 가려져 있는 경우 성공 확률 ?로 표시
        if(targetPortal.can_See_PortalPower)
        {
            disPatch_Clear_Chance.text = "파견 성공 확률 : " + (targetDisPatch.clearChance_UI*100).ToString("F2")+"%";
        }
        else
        {
            disPatch_Clear_Chance.text = "파견 성공 확률 : ?";
        }
    }
}
