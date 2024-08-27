using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon_InitialSet : MonoBehaviourPunCallbacks
{
    public GameObject UI_Canvas;
    public Text Log;
    public InputField UserID;
    public Button JoinRandomButton;

    public string gameVersion = "1.0";
    
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            JoinRandomButton.interactable = false;
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            Log.text = "Connect to Master Server...";
        }
    }

    private string GetUserID()
    {
        string userID = PlayerPrefs.GetString("UserID");
        if (string.IsNullOrEmpty(userID))
            userID = "UserID_" + Random.Range(0, 999).ToString("000");
        return userID;
    }

    public override void OnConnectedToMaster()
    {
        Log.text = "Online : Master Server Connected!";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Log.text = "Online : Lobby Joined!";
        UserID.text = GetUserID();
        JoinRandomButton.interactable = true;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text = "Online : Room Join Failed!";
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 10 });
    }
    public void OnClickJoinRandomRoom()
    {
        JoinRandomButton.interactable = false;
        PhotonNetwork.NickName = UserID.text;
        PlayerPrefs.SetString("UserID", UserID.text);
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Log.text = "Online : Room Joined!";
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("F1TrackDisplayScene");
        yield return ao;
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }



}
