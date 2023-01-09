using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class scoreBoardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText, deathText, killsText;

    Player player;
    public void initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;

        updateStats();
    }

    void updateStats()
    {
        if(player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
        }
        if(player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathText.text = deaths.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                updateStats();
            }
        }
    }
}
