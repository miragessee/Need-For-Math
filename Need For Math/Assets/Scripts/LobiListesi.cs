using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobiListesi : MonoBehaviourPunCallbacks
{
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

    public GameObject RoomListEntryPrefab;
    public GameObject RoomListContent;

    public void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LobiListesiClick()
    {
        //foreach (RoomInfo room in PhotonNetwork.GetCustomRoomList())
        //{
        //    if (!room.open)
        //        continue;
        //    GameObject buttonPrefab = Resources.Load<GameObject>("GUI/RoomGUI");
        //    GameObject roomButton = Instantiate<GameObject>(buttonPrefab);
        //    roomButton.GetComponent<RoomJoiner>().RoomName = room.name;
        //    string info = room.name.Trim() + " (" + room.playerCount + "/" + room.maxPlayers + ")";
        //    roomButton.GetComponentInChildren<Text>().text = info;
        //    rooms.Add(roomButton.GetComponent<RoomJoiner>());
        //    roomButton.transform.SetParent(racesPanel, false);
        //    roomButton.transform.position.Set(0, i * 120, 0);
        //}
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        Debug.Log("UpdateRoomListView");

        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            //entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            Debug.Log(info.Name);

            entry.GetComponentsInChildren<Text>()[0].text = info.Name;
            entry.GetComponentsInChildren<Text>()[1].text = "Oyuncu Sayısı: " + info.PlayerCount.ToString();

            roomListEntries.Add(info.Name, entry);
        }
    }
}
