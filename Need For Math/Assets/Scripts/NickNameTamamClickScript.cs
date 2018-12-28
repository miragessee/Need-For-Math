using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameTamamClickScript : MonoBehaviourPunCallbacks
{
    public InputField edtNickname;

    public Transform yarisZamaniVerticalButton;
    public Transform lobiListesiVerticalButton;

    public Transform nickNamePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.LoadLevel(1);
            nickNamePanel.gameObject.SetActive(false);
            yarisZamaniVerticalButton.gameObject.SetActive(true);
            lobiListesiVerticalButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NickNameTamamClick()
    {
        Debug.Log("NickNameTamam");

        if (edtNickname.text.Length > 0)
        {
            PhotonNetwork.LocalPlayer.NickName = edtNickname.text;

            Debug.Log(edtNickname.text);
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);

            PhotonNetwork.GameVersion = "Need For Math v1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //PhotonNetwork.LoadLevel(1);
        nickNamePanel.gameObject.SetActive(false);
        yarisZamaniVerticalButton.gameObject.SetActive(true);
        lobiListesiVerticalButton.gameObject.SetActive(true);
    }
}
