using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class playerItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] private TMP_Text playerName;

    public void setUp(Player _player)
    {
        player = _player;
        playerName.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(otherPlayer == player)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
