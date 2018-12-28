using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageArabaSec : MonoBehaviourPunCallbacks
{
    public RCC_CarSelectionExample carSelectionExample;

    public static int selectedIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GarageArabaSecClick()
    {
        Debug.Log("GarageArabaSecClick");

        Debug.Log(carSelectionExample.selectedIndex);

        selectedIndex = carSelectionExample.selectedIndex;

        if (PhotonNetwork.IsMasterClient)
        {
            SetCustomProperties(PhotonNetwork.LocalPlayer, selectedIndex, 0);
        }
        else
        {
            SetCustomProperties(PhotonNetwork.LocalPlayer, selectedIndex, 1);
        }

        transform.GetComponentInChildren<Text>().text = "Seçildi. Oyuncu bekleniyor.";

        //PhotonNetwork.LoadLevel(2);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            //int sayac = 0;

            //for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount; i++)
            //{
                //Debug.Log("player " + i + ":" + PhotonNetwork.CurrentRoom.Players[i].NickName);

                //if (PhotonNetwork.CurrentRoom.Players[i].CustomProperties["car"] != null)
                //{
                //    sayac++;
                //}
            //}

            //if (PhotonNetwork.IsMasterClient == true /*&& sayac == 2*/)
            //{
                //PhotonNetwork.AutomaticallySyncScene = true;
                //PhotonNetwork.LoadLevel(2);
                photonView.RPC("LoadRace", RpcTarget.All);
            //}
        }
    }

    [PunRPC]
    public void LoadRace()
    {
        PhotonNetwork.LoadLevel(2);
    }

    private void SetCustomProperties(Player player, int car, int position)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position }, { "car", car } };
        player.SetCustomProperties(customProperties);
    }
}
