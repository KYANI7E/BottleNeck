using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Wave")]
public class WaveData : ScriptableObject
{
    public int[] location; //if -1 spawn at all portals
    public GameObject[] enemies;
    public int[] enemiesAmount;
    public float[] intervalBetweenEnemies;
    public float[] timeBetweenSets;
}
