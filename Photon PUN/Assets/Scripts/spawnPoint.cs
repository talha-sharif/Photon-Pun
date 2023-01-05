using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    [SerializeField] private Transform graphics;
    private void Awake()
    {
        graphics.gameObject.SetActive(false);
    }
}
