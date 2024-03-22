using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPatch_Power_Account
{
    #region 파견 전투력 관련 계산
    //파견 리스트 유닛 전투력 계산 - 아이템 적용 구현X 나중에 아이템 enum과 함께 구현
    public int DisPatch_Power_Count(List<Unit> disPatch_Units/*여기에 아이템 enum*/)
    {
        //리더 효과 중첩
        int ability_leader_count = 0;
        //고문관 효과 중첩
        int ability_bluefalcon_count = 0;
        int all_Of_Power = 0;
        //파견에 포함 되어있는 유닛의 특성 효과 합산 후 유닛 전투력 계산
        foreach (Unit unit in disPatch_Units)
        {
            if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Leader))
            {
                ability_leader_count++;
            }
            if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Blue_Falcon))
            {
                ability_bluefalcon_count++;
            }
            //유닛 전투력 계산 후 총 전투력에 합산
            all_Of_Power += Unit_Power_Account(unit);
        }
        //총 전투력에서 특성 적용
        all_Of_Power = All_Power_Var(all_Of_Power, ability_leader_count, ability_bluefalcon_count);
        Debug.Log("총 전투력" + all_Of_Power);
        return all_Of_Power;
    }

    /*
     여기에 아이템 적용 코드
     */

    //총 전투력 증감 적용
    private int All_Power_Var(int all_Of_Power, float leader, float bluefalcon)
    {
        //특성으로 증감시킬 수치 저장
        float all_Power_Var = 0;
        all_Power_Var = (leader * 0.05f) - (bluefalcon * 0.07f);
        //총 전투력에서 특성 증감치 적용 후 리턴
        return all_Of_Power - (int)(all_Of_Power * all_Power_Var);
    }


    //유닛 개별 전투력 계산
    public int Unit_Power_Account(Unit unit)
    {
        //유닛 기본 전투력 : 공격력의 120% + 체력의 50%
        int temp = (int)(unit.Get_Unit_Damage() * 1.2f) + (int)(unit.Get_Unit_Hp() * 0.5f);
        //기본 전투력에서 특성 증감치 계산 후 적용
        temp = (int)(temp + ((float)temp * Unit_Power_Variance(unit)));
        return temp;
    }



    //유닛 개별 전투력 증감 수치 계산
    private float Unit_Power_Variance(Unit unit)
    {
        float temp = 0;
        //공격 자세 특성 보유 시
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Attack_Stance))
        {
            temp += 0.2f;
        }
        //병약 특성 보유 시
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Weakness))
        {
            temp -= 0.25f;
        }
        return temp;
    }
    #endregion
}
