using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Residence : MonoBehaviour
{
    [SerializeField] private int _populationIncrease;

    public int PopulationIncrease => _populationIncrease;
}
