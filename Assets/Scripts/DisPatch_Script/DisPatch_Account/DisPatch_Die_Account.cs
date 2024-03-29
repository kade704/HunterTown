using System.Collections.Generic;

public class DisPatch_Die_Account
{
    #region 공통
    //유닛 개별 생존력 계산
    public int Unit_Survival_Account(Unit unit)
    {
        //HP의 100% + 공격력의 80%
        return unit.Get_Unit_Hp() + (int)((float)unit.Get_Unit_Damage() * 0.8f);
    }

    private float Unit_Die_Base_Count(Unit unit, Portal portal, bool disPatch_Pass)
    {
        float var = 0;
        int survival = Unit_Survival_Account(unit);
        float die_Per = 0f;
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Sixth_Sense))
        {
            var += 0.03f;
        }
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Weakness))
        {
            var -= 0.07f;
        }
        if (disPatch_Pass)
        {
            die_Per = 1f - ((float)survival / portal.portalDanger);
        }
        else
        {
            die_Per = 1f - ((float)survival / (portal.portalDanger * 2f));
        }
        die_Per += var;

        if (die_Per < 0f)
        {
            die_Per = 0f;
        }
        return die_Per;
    }
    #endregion
    #region 파견 실제 사망률 관련 계산
    //전체 유닛 사망률 계산
    public float[] DisPatch_Die_Count(Portal portal, List<Unit> disPatch_Unit, bool disPatch_Pass)
    {
        float[] die_temp = new float[disPatch_Unit.Count];
        for (int i = 0; i < disPatch_Unit.Count; i++)
        {
            die_temp[i] = Unit_Die_Account(disPatch_Unit[i], portal, disPatch_Pass);
        }
        return die_temp;
    }

    //유닛 개별 사망률 계산
    private float Unit_Die_Account(Unit unit, Portal portal, bool disPatch_Pass)
    {
        float unit_Die_Per = Unit_Die_Base_Count(unit, portal, disPatch_Pass);
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Portal_Ability_Check(portal, Portal.Portal_Ability.Lush_Forest))
        {
            unit_Die_Per += Portal.Lush_Forest_Var;
        }
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Portal_Ability_Check(portal, Portal.Portal_Ability.Peace))
        {
            unit_Die_Per += 0.02f;
        }
        if (unit_Die_Per < 0)
        {
            unit_Die_Per = 0;
        }
        return unit_Die_Per;
    }
    #endregion
    #region UI에 표시할 사망률 계산
    public float[] DisPatch_Die_UI_Account(Portal portal, List<Unit> disPatch_Unit)
    {
        float[] die_temp = new float[disPatch_Unit.Count];
        for (int i = 0; i < disPatch_Unit.Count; i++)
        {
            die_temp[i] = Unit_Die_UI_Account(disPatch_Unit[i], portal);
        }
        return die_temp;
    }
    public float Unit_Die_UI_Account(Unit unit, Portal portal)
    {
        float unit_Die_Per = Unit_Die_Base_Count(unit, portal, true);
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Check_Portal_Ability_UI(portal, Portal.Portal_Ability.Peace))
        {
            unit_Die_Per += 0.02f;
        }
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Check_Portal_Ability_UI(portal, Portal.Portal_Ability.Lush_Forest))
        {
            unit_Die_Per -= 0.03f;
        }
        if (unit_Die_Per < 0)
        {
            unit_Die_Per = 0;
        }
        return unit_Die_Per;
    }
    #endregion
}
