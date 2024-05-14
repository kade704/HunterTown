using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Residence : MonoBehaviour
{
    [SerializeField] private int _increasePopulation;

    private void Start()
    {
        GameManager.Instance.GetSystem<Player>().MaxPopulation += _increasePopulation;
    }

    private void OnDestroy()
    {
        //GameManager.Instance.GetSystem<Player>().MaxPopulation -= _increasePopulation;
    }
}
