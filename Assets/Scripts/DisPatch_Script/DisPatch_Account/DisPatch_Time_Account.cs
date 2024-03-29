using System.Collections.Generic;

public class DisPatch_Time_Account
{
    //
    public void DisPatch_Time_Count(Portal portal, List<Unit> disPatch_Unit)
    {
        int unit_Guide = 0;
        int unit_Terrible_Direction = 0;
        float time_Var = 0f;

        foreach (Unit unit in disPatch_Unit)
        {
            if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Guide))
            {
                unit_Guide++;
            }
            if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Unit_Ability_Check(unit, Unit.Unit_Ability.Terrible_Direction))
            {
                unit_Terrible_Direction++;
            }
        }
    }

    private void DisPatch_In_Timmer_Var(Portal portal)
    {
        if (GameManager.Instance.GetDisPatch_Account().GetAbility_Check().Portal_Ability_Check(portal, Portal.Portal_Ability.Good_Temper))
        {

        }
    }


}
