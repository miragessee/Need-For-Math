using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarisZamani : MonoBehaviourPunCallbacks
{
    public static int loadingScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        YarisZamani.loadingScene = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YarisZamaniClick()
    {
        Debug.Log("YarisZamani");

        PhotonNetwork.AddCallbackTarget(this);

        PhotonNetwork.AutomaticallySyncScene = true;

        Connect();
    }

    public void Connect()
    {
        Debug.Log("Connect func");

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = "Need For Math v1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void YarisZamaniBaslat()
    {
        PhotonNetwork.AddCallbackTarget(this);

        PhotonNetwork.AutomaticallySyncScene = true;

        YarisZamaniConnect();
    }

    public void YarisZamaniConnect()
    {
        Debug.Log("Connect func");

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("countOfPlayers InRoom:" + PhotonNetwork.CurrentRoom.PlayerCount);

                for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    Debug.Log("player " + i + ":" + PhotonNetwork.CurrentRoom.Players[i]);
                }

                if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                {
                    if (PhotonNetwork.IsMasterClient == true)
                    {
                        if (YarisZamani.loadingScene == 0)
                        {
                            Debug.Log("LoadLevel");

                            PhotonNetwork.AutomaticallySyncScene = true;
                            PhotonNetwork.LoadLevel(2);
                            YarisZamani.loadingScene = 1;
                        }

                        //PhotonNetwork.LevelLoadingProgress
                    }
                    //else
                    //{
                    //    Player masterClient = PhotonNetwork.MasterClient;
                    //    if(masterClient == PhotonNetwork.LocalPlayer)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

                    //        if (Baslat1vs1Script.loadingScene == 0)
                    //        {
                    //            Debug.Log("LoadLevel SetMasterClient");

                    //            PhotonNetwork.AutomaticallySyncScene = true;
                    //            PhotonNetwork.LoadLevel(2);
                    //            Baslat1vs1Script.loadingScene = 1;
                    //        }
                    //    }
                    //}
                }
                else if (PhotonNetwork.CurrentRoom.PlayerCount >= 0)
                {
                    //infoPanel.SetActive(true);

                    PhotonNetwork.LeaveRoom();

                    YarisZamaniBaslat();
                }
            }
            else
            {
                Debug.Log("Joining Room...");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                bool bj = PhotonNetwork.JoinRandomRoom();
            }
            //if(bj)
            //{
            //    Debug.Log("JoinRandomRoom TRUE");
            //}
            //else
            //{
            //    Debug.Log("JoinRandomRoom FALSE");

            //    PhotonNetwork.LeaveRoom();

            //    PhotonNetwork.JoinRandomRoom();
            //}
        }
        else
        {
            Debug.Log("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = "Need For Math v1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        //Guid id = Guid.NewGuid();

        //PhotonNetwork.JoinOrCreateRoom(id.ToString(), roomOptions, TypedLobby.Default);

        PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.LocalPlayer.NickName, roomOptions, TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:Disconnected");
    }

    public override void OnJoinedRoom()
    {
        //if (SteamManager.Initialized)
        //{
        //    string name = SteamFriends.GetPersonaName();
        //    Debug.Log(name);

        //    nickNameTMPMaster.text = name;

        //    CSteamID steamID = SteamUser.GetSteamID();
        //    profilFotoMaster.GetComponent<RawImage>().texture = steamAvatarGetir(steamID);

        //    lobbyIDS = new List<CSteamID>();
        //    Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        //    //Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
        //    Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        //    //Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);

        //    SteamAPICall_t try_toHost = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 8);
        //    Debug.Log(try_toHost.m_SteamAPICall);
        //}

        //infoPanel.gameObject.SetActive(false);

        Debug.Log("OnJoinedRoom");

        Debug.Log("countOfPlayers:" + PhotonNetwork.CountOfPlayers);

        SetCustomProperties(PhotonNetwork.LocalPlayer, 0, PhotonNetwork.PlayerList.Length - 1);

        //if (PhotonNetwork.CountOfPlayers == 2)
        //{
        if (PhotonNetwork.IsMasterClient == true)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel(1);
            }
        //}
        //else
        //{
        //    //infoPanel.SetActive(true);
        //}
    }

    private void SetCustomProperties(Player player, int car, int position)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position }, { "car", car } };
        player.SetCustomProperties(customProperties);
    }
}
