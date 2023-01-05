using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class playerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            createController();
        }
    }

    void createController()
    {
        Transform spawnPoint = spawnManager.instance.getSpawPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("Photon Prefabs", "Player Controller"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
    }

    internal void die()
    {
        PhotonNetwork.Destroy(controller);
        createController();
    }
}
