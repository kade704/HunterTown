using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*UnitSelectButton
포탈클릭 시 활성화되는 파견 UI의 파견 유닛 선택 버튼에 배정되는 코드
----------------------------------------------
DisPatch_Unit_Button  DisPatch 배정 : 해당 코드를 가진 버튼에서 사용할 DisPatch를
배정하는 코드
SelectButton  유닛 배정 : 해당 버튼의 번호에 해당하는 유닛을 DisPatch의 파견 유닛
에 해당하는 리스트에 배정하는 코드
 */

public class UnitSelectButton : MonoBehaviour
{
    public int buttonNum;
    private DisPatch disPatch;

    //DisPatch 배정
    public void DisPatch_Unit_Button(DisPatch portal_Dis)
    {
        disPatch = portal_Dis;
    }
    //유닛 배정
    public void SelectButton()
    {
        Debug.Log("파견 인원 선택" + GameManager.Instance.unit_List.unitList[buttonNum].unit_name);
        disPatch.DisPatch_Input_Unit(GameManager.Instance.unit_List.unitList[buttonNum]);
    }
    public void UnSelectButton()
    {
        Debug.Log("파견 인원 제거"+disPatch.disPatch_Units[buttonNum].unit_name);
        disPatch.DisPatrch_Unput_Unit(buttonNum);
    }
}
