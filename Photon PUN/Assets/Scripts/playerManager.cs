using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class playerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    int kills;
    int deaths;

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

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }

    public static playerManager Find(Player player)
    {
        return FindObjectsOfType<playerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }

    public void getKill()
    {
        PV.RPC(nameof(rpc_getKill), PV.Owner);
    }

    [PunRPC]
    void rpc_getKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }
}
