using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class scoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject scoreboardItemPrefab;
    [SerializeField] private CanvasGroup canvasGroup;

    Dictionary<Player, scoreBoardItem> scoreBoardItems = new Dictionary<Player, scoreBoardItem>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            addScoreboardItem(player);
        }
    }

    private void addScoreboardItem(Player player)
    {
        scoreBoardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<scoreBoardItem>();
        item.initialize(player);
        scoreBoardItems[player] = item;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        addScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        removeScoreboardItem(otherPlayer);
    }

    private void removeScoreboardItem(Player player)
    {
        Destroy(scoreBoardItems[player].gameObject);
        scoreBoardItems.Remove(player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            canvasGroup.alpha = 1;
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
        }
    }
}
