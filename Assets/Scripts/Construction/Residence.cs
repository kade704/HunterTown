using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Residence : MonoBehaviour
{
    [SerializeField] private int _increasePopulation;

    public int IncreasePopulation => _increasePopulation;
}
