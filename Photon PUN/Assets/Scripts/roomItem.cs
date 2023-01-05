using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class roomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    RoomInfo info;

    public void setUp(RoomInfo _info)
    {
        info = _info;
        roomName.text = _info.Name;
    }

    public void onClick()
    {
        Launcher.instance.joinRoom(info);
    }
}
