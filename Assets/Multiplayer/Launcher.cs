﻿using Assets;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The Ui Panel to let the user enter name, connect and play")] [SerializeField]
    private GameObject controlPanel;

    /// <summary>
    ///     Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    ///     we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    ///     Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool isConnecting;

    [Tooltip("The UI Label to inform the user that the connection is in progress")] [SerializeField]
    private GameObject progressLabel;

    private void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    /// <summary>
    ///     Start the connection process.
    ///     - If already connected, we attempt joining a random room
    ///     - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameManager.Instance.GameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"PUN {nameof(Launcher)}:OnConnectedToMaster() was called by PUN");

        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat($"PUN {nameof(Launcher)}:OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(
            "PUN {nameof(Launcher)}:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = GameManager.Instance.MaxPlayers});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"PUN {nameof(Launcher)}:OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the sandbox scene");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("SandboxScene");
        }
    }
}