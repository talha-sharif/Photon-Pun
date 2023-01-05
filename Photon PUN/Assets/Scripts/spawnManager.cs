using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public static spawnManager instance;
    spawnPoint[] spawnPoints;
    private void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<spawnPoint>();
    }

    public Transform getSpawPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
