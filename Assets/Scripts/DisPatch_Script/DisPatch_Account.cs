using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DisPatch_Account
{
    DisPatch_Ability_Check ability_Check = new DisPatch_Ability_Check();
    DisPatch_Clear_Account clear_Account = new DisPatch_Clear_Account();
    DisPatch_Die_Account die_Account = new DisPatch_Die_Account();
    DisPatch_Power_Account power_Account = new DisPatch_Power_Account();
    DisPatch_Time_Account time_Account = new DisPatch_Time_Account();

    public DisPatch_Ability_Check GetAbility_Check()
    {
        return ability_Check;
    }
    public DisPatch_Clear_Account GetClear_Account()
    {
        return clear_Account;
    }
    public DisPatch_Die_Account GetDie_Account()
    {
        return die_Account;
    }
    public DisPatch_Power_Account GetPower_Account()
    {
        return power_Account;
    }
    public DisPatch_Time_Account GetTime_Account()
    {
        return time_Account;
    }
}
