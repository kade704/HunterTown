using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    [HideInInspector]
    public Unit_List unit_List;
    private DisPatch_Account dis_Account = null;

    private int day = 0;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        dis_Account = new DisPatch_Account();
    }

    public static GameManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public DisPatch_Account GetDisPatch_Account()
    {
        return dis_Account;
    }

    IEnumerator GameTimmer_Day()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            day++;
            Debug.Log("게임타이머 작동 : 일수 + 1");
            if (day % 7 == 0)
            {
                GameTimmer_Week();
            }
        }
    }

    public void GameTimmer_Week()
    {
        Debug.Log("게임타이머 작동 : 주간 진행");
        BroadcastMessage("Timmer_Check", SendMessageOptions.DontRequireReceiver);
    }
}
