using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPatch_Clear_Account
{
    #region 파견 실제 성공률 계산
    //파견 성공률 계산
    public float DisPatch_Clear_Count(Portal portal, int all_Of_Damage)
    {
        float disPatch_Clear_temp = 0f;

        disPatch_Clear_temp = ((float)all_Of_Damage / (float)portal.portalPower) - (((float)all_Of_Damage / (float)portal.portalPower) / 6f);
        disPatch_Clear_temp += DisPatch_Clear_Variance(portal);
        if(disPatch_Clear_temp > 1f)
        {
            disPatch_Clear_temp = 1f;
        }

        return disPatch_Clear_temp;
    }

    //파견 성공률 증감 수치 계산
    private float DisPatch_Clear_Variance(Portal portal)
    {
        float var = 0f;
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Portal_Ability_Check(portal, Portal.Portal_Ability.Lava))
        {
            var -= 0.05f;
        }
        //정책 관련 작성
        return var;
    }
    #endregion
    #region UI에 표시될 성공률 계산
    //파견 성공률(UI에 표시될) 계산
    public float DisPatch_Clear_UI_Account(Portal portal, int all_Of_Power)
    {
        float disPatch_Clear_UI_temp = 0f;
        //돌연변이 특성 존재 및 파악 여부 측정
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Check_Portal_Ability_UI(portal, Portal.Portal_Ability.Mutation))
        {
            // (총 공격력 / 포탈 실 능력치) - (총 공격력 / 포탈 실 능력치 / 6)
            disPatch_Clear_UI_temp = ((float)all_Of_Power / (float)portal.portalPower) - ((float)all_Of_Power / (float)portal.portalPower / 6f);
        }
        else
        {
            // (총 공격력 / 포탈 기본 능력치(특성 적용 X) - (총 공격력 / 포탈 기본 능력치(특성 적용 X) / 6)
            disPatch_Clear_UI_temp = ((float)all_Of_Power / (float)portal.portalBasePower) - ((float)all_Of_Power / (float)portal.portalBasePower / 6f);
        }

        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Check_Portal_Ability_UI(portal, Portal.Portal_Ability.Lava))
        {
            disPatch_Clear_UI_temp -= 0.05f;
        }
        if(disPatch_Clear_UI_temp > 1f)
        {
            disPatch_Clear_UI_temp = 1f;
        }
        return disPatch_Clear_UI_temp;

    }
    #endregion
}
