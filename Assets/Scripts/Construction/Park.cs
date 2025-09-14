using UnityEngine;

public class Park : MonoBehaviour
{
    [SerializeField] private int _increasePopulationGrowth;

    public int IncreasePopulationGrowth => _increasePopulationGrowth;
}
