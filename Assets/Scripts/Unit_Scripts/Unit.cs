using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public enum Unit_Ability
    {
        None,
        Leader,                 //리더쉽 있는 성격 - 파견 총 전투력 5%증가 -3
        Sixth_Sense,            //제6감 -  유닛 사망률 3% 감소 -4
        God_Of_Hammer,          //신들린 망치질 - 희귀재료 획득 1%  증가 -3 
        Indomitable_Power,      //불굴의 힘 - 해당 유닛 체력 20 % 증가 -2
        Attack_Stance,          //공격 태세 - 해당 유닛 공격력 20 % 증가 -2
        Skill_Hand,             //숙련된 손길 - 해당 유닛 손재주 20 % 증가 -2
        Guide,                  //길잡이  -  포탈 파견 시간 10% 감소 -1
        Handy_Hand,             //능숙한 손놀림 - 제련 시간 5 % 단축 -1
        Good_Learning_Power,    //뛰어난 학습력 - 능력치 증가 20% 확률로 150% 증가 -4
        Scout,                  //정찰 전문가  - 포탈 정찰 시 특성 파악 확률 몇퍼% 증가 -3
        Hero,                   //주인공 - 이 캐릭터 사망 시 30% 생존 후 능력치의 50% 증가  -6
        Good_Speaking_Skill,    //뛰어난 말솜씨 - 상점 가격 30% 할인 + 정책 성공 확률 10% 증가 + 훈련 시 모든 유닛 모든 능력치 1 증가  - 5
        Hephaestus,             //헤파이스토스 - 제련소에 배정 시 모든 유닛 능력치 10% 
        Genius,                 //천재 - 능력치 증가 시 +2 증가 -5
        Terrible_Direction,     //길치 - 포탈 파견 시간 15% 증가  + 2
        Weakness,               //병약 - 자신의 사망률 7% 증가 +4
        HandOfMinus,            //마이너스의 손 - 해당 유닛 손재주 30% 감소  + 3 
        Blue_Falcon,            //고문관 - 파견 총 전투력 7% 감소 +5
        Muscle_Loss,            //근손실 - 해당 유닛 체력 공격력 25% 감소 +4
        ForgetFulness,          //건망증 - 파견 보상 10% 감소 + 2
        ADHD,                   //ADHD -  능력치 증가 할 때 30% 증가 하지 않음 +5 
        Kleptomania,            //도벽 - 재련 시 20% 로 물건이 나오지 않음  + 3
    }
    public string unit_name;
    private int base_hp;
    private int base_damage;
    public int handicraft;
    public bool is_Active = false;
    public Sprite headImage;

    public List<Unit_Ability> ability = new List<Unit_Ability>();

    public Unit (string g_unit_name,int g_unit_hp, int g_unit_damage,int g_handicraft, Unit_Ability g_unit_ability1, Unit_Ability g_unit_ability2, Unit_Ability g_unit_ability3)
    {
        unit_name = g_unit_name;
        base_hp = g_unit_hp;
        base_damage = g_unit_damage;
        handicraft = g_handicraft;
        ability.Add(g_unit_ability1);
        ability.Add(g_unit_ability2);
        ability.Add(g_unit_ability3);
    }


    //HP 가져오기
    public int Get_Unit_Hp()
    {
        //특성으로 인한 증감치 계산
        float ability_var = 0;
        if(ability.Contains(Unit_Ability.Indomitable_Power))
        {
            ability_var += 0.2f;
        }
        if(ability.Contains(Unit_Ability.Muscle_Loss))
        {
            ability_var -= 0.25f;
        }
        return (int)(base_hp + (base_hp * ability_var));
    }
    //공격력 가져오기
    public int Get_Unit_Damage()
    {
        //특성으로 인한 증감치 계산
        float ability_var = 0;
        if (ability.Contains(Unit_Ability.Attack_Stance))
        {
            ability_var += 0.2f;
        }
        if (ability.Contains(Unit_Ability.Muscle_Loss))
        {
            ability_var -= 0.25f;
        }
        return (int)(base_damage + (base_damage * ability_var));
    }

    //유닛 능력치 증가
    public void Unit_State_Up()
    {

    }
}
