using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_List : MonoBehaviour
{
    #region 더미 - 테스트용
    [Serializable]
    public struct Dumy_Test_State
    {
        public string unit_name;
        public int hp;
        public int damage;
        public int handicraft;
        public Unit.Unit_Ability ability1;
        public Unit.Unit_Ability ability2;
        public Unit.Unit_Ability ability3;
    }
    public Dumy_Test_State[] dumy_Unit = new Dumy_Test_State[5];
    


    public void Dumy_Unit_Set()
    {
        for (int i = 0; i < 5; i++)
        {
            Dumy_Test_State dumy_Test = dumy_Unit[i];
            Unit unit = new Unit(dumy_Test.unit_name,dumy_Test.handicraft,dumy_Test.damage,dumy_Test.handicraft,dumy_Test.ability1,dumy_Test.ability2,dumy_Test.ability3);
            unitList.Add(unit);
        }
        foreach (Unit unit in unitList)
        {
            Debug.Log(unit.unit_name);
        }
    }


    #endregion


    private void Start()
    {
        if (GameManager.Instance.unit_List == null)
        {
            GameManager.Instance.unit_List = this;
            Debug.Log("GameManager 에 unit_List배정 : " + this.name);
        }
        Dumy_Unit_Set();
    }
    public List<Unit> unitList = new List<Unit>();

    //더미유닛 능력치 설정

}
