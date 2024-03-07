using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnLocation;

    void Awake()
    {
        GameManager._Instance.StageSpawnPoints.Add(spawnLocation.transform);
    }
}
