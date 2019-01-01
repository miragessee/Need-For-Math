using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
using Photon.Realtime;

public class PUNRaceManagerScript : MonoBehaviourPunCallbacks
{
    public static PUNRaceManagerScript instance;

    // To sets player car as camera target
    //public SmoothFollow cameraControl;

    public RCC_Camera cameControlRCC;

    public Text lapGUI;
    public Text positionGUI;
    public Text speedGUI;
    public Text messagesGUI;
    public Image weaponImageGUI;
    public Text weaponTextGUI;

    public Transform playersPanel;
    public Transform endRacePanel;
    private List<CarPanelGUI> carPanelGUIs = new List<CarPanelGUI>();
    private List<CarPanelGUI> endRaceGUIs = new List<CarPanelGUI>();
    private int loadedPlayers = 0;

    // reference to local player car
    [HideInInspector]
    public CarRaceControl localCar;

    // list of all player´s cars (for position calculation)
    List<CarRaceControl> carControllers = new List<CarRaceControl>();

    public float raceTime = 0;
    public double startTimestamp = 0;

    private RaceState state = RaceState.PRE_START;

    // All spawn points for track (uses one of them for player car)
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;



        Debug.Log("Created car");
        CreateCar();
        photonView.RPC("ConfirmLoad", RpcTarget.All);

        // get references to the player's GUIs in both the top-right panel and the
        // end-race-only final position/time panel
        int count = endRacePanel.childCount;
        for (int i = 0; i < count; i++)
        {
            endRaceGUIs.Add(endRacePanel.GetChild(i).GetComponent<CarPanelGUI>());
        }
        count = playersPanel.childCount;
        for (int i = 0; i < count; i++)
        {
            carPanelGUIs.Add(playersPanel.GetChild(i).GetComponent<CarPanelGUI>());
        }
    }

    private void SetCustomProperties(Player player, int car, int position)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "spawn", position }, { "car", car } };
        player.SetCustomProperties(customProperties);
    }

    // Update is called once per frame
    void Update()
    {
        raceTime += Time.deltaTime;

        SortCars();

        switch (state)
        {
            case RaceState.PRE_START:
                if (PhotonNetwork.IsMasterClient)
                {
                    CheckCountdown();
                }
                break;
            case RaceState.COUNTDOWN:
                messagesGUI.text = "" + (1 + (int)(startTimestamp - PhotonNetwork.Time));
                if (PhotonNetwork.Time >= startTimestamp)
                {
                    StartRace();
                }
                break;
            case RaceState.RACING:
                if (raceTime > 3)
                {
                    messagesGUI.text = "";
                }
                else
                {
                    messagesGUI.text = "GO!";
                }
                break;
            case RaceState.FINISHED:
                UpdatePlayerPanel(endRaceGUIs);
                break;
        }

        if (localCar.state == RaceState.FINISHED)
        {
            state = RaceState.FINISHED;
            // enable panel
            endRacePanel.gameObject.SetActive(true);
        }
        UpdatePlayerPanel(carPanelGUIs);
    }

    private void SortCars()
    {
        carControllers.Sort();
        int position = 1;
        foreach (CarRaceControl c in carControllers)
        {
            c.currentPosition = position;
            position++;
        }
    }

    // put the correct car references in position order to the appropriate GUIs
    // used on two panels (top-right, and final position/time)
    private void UpdatePlayerPanel(List<CarPanelGUI> guis)
    {
        foreach (CarRaceControl c in carControllers)
        {
            guis[c.currentPosition - 1].SetCar(c);
        }
    }

    // called when a player computer finishes loading this scene...
    [PunRPC]
    public void ConfirmLoad()
    {
        loadedPlayers++;
    }

    // master-client only: we start the countdown only when ALL players are connected
    private void CheckCountdown()
    {
        bool takingTooLong = raceTime >= 5;
        bool finishedLoading = loadedPlayers == PhotonNetwork.PlayerList.Length;
        if (takingTooLong || finishedLoading)
        {
            photonView.RPC("StartCountdown", RpcTarget.All, PhotonNetwork.Time + 4);
        }
    }

    // Instantiates player car on all peers, using the appropriate spawn point (based
    // on join order), and sets local camera target.
    private void CreateCar()
    {
        // gets spawn Transform based on player join order (spawn property)
        //int pos = (int)PhotonNetwork.LocalPlayer.CustomProperties["spawn"];
        //int carNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties["car"];
        int pos = 1;
        int carNumber = 0;
        Transform spawn = spawnPoints[pos];

        // instantiate car at Spawn Transform
        // car prefabs are numbered in the same order as the car sprites that the player chose from
        GameObject car = PhotonNetwork.Instantiate("Car" + carNumber, spawn.position, spawn.rotation, 0);
        //car = ((Transform)GameObject.Instantiate(carPrefab, spawn.position, spawn.rotation)).gameObject;

        localCar = car.GetComponent<CarRaceControl>();

        // car starting race position (for GUI) is same as spawn position + 1 (grid position)
        car.GetComponent<CarRaceControl>().currentPosition = pos + 1;

        // sets local car as the camera target
        //cameraControl.target = car.transform;
        cameControlRCC.playerCar = car.transform.GetComponent<RCC_CarControllerV3>();

        // enable GUI for local car
        car.GetComponent<CarGUI>().enabled = true;
        car.GetComponent<CarGUI>().lapGUI = lapGUI;
        car.GetComponent<CarGUI>().positionGUI = positionGUI;
        car.GetComponent<CarGUI>().speedGUI = speedGUI;
        car.GetComponent<CarGUI>().messagesGUI = messagesGUI;

        car.GetComponent<WeaponManager>().weaponImageGUI = weaponImageGUI;
        car.GetComponent<WeaponManager>().weaponTextGUI = weaponTextGUI;

        // ***** Template code for sending custom colors for cars *****
        // replace Color.black by a custom color chosen by the player
        // Check CarGUI as well!
        car.GetComponent<CarGUI>().SendColor(Color.black);
    }

    public void StartRace()
    {
        Debug.Log("Start");
        state = RaceState.RACING;
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in cars)
        {
            carControllers.Add(go.GetComponent<CarRaceControl>());
            //go.GetComponent<CarInput>().enabled = true;
            if(go.GetPhotonView().IsMine)
            {
                go.GetComponent<RCC_CarControllerV3>().enabled = true;
            }
            go.GetComponent<CarRaceControl>().currentWaypoint = GameObject.Find("Checkpoint1").GetComponent<Checkpoint>();
            go.GetComponent<CarRaceControl>().state = RaceState.RACING;
        }

        //localCar.GetComponent<CarInput>().controlable = true;
        localCar.GetComponent<RCC_CarControllerV3>().enabled = true;
        localCar.GetComponent<WeaponManager>().enabled = true;

        raceTime = 0;
    }

    [PunRPC]
    public void StartCountdown(double startTimestamp)
    {
        Debug.Log("Countdown");
        state = RaceState.COUNTDOWN;
        // sets local timestamp to the desired server timestamp (will be checked every frame)
        this.startTimestamp = startTimestamp;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " disconnected...");
        CarRaceControl toRemove = null;
        foreach (CarRaceControl rc in carControllers)
        {
            //Debug.Log (rc.photonView.owner);
            if (rc.photonView.Owner == null)
            {
                toRemove = rc;
            }
        }
        // remove car controller of disconnected player from the list
        carControllers.Remove(toRemove);
    }

    // Use this to go back to the menu, without leaving the lobby
    public void ResetToMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Menu");
    }
}
