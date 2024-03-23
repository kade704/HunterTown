using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPatch_Ability_Check
{
    #region 특성 확인 코드
    //유닛 특성 확인
    public bool Unit_Ability_Check(Unit unit, Unit.Unit_Ability ability)
    {
        //파라미터로 받은 유닛에 파라미터로 받은 특성이 있는 경우 true 반환
        if (unit.ability.Contains(ability))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Portal_Ability_Check(Portal portal, Portal.Portal_Ability ability)
    {
        //파라미터로 받은 유닛에 파라미터로 받은 특성이 있는 경우 true 반환
        if (portal.ability.Contains(ability))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region 특성 파악 상태 확인
    //포탈에 특성이 존재하며, 파악 가능한 상태일 경우 true
    public bool Check_Portal_Ability_UI(Portal portal, Portal.Portal_Ability ability)
    {
        if (portal.ability.Contains(ability))
        {
            if (portal.can_See_Ablitiy[portal.ability.IndexOf(ability)])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        { return false; }
    }
    #endregion
}
