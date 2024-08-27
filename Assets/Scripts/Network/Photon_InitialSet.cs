using System.Collections;
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
            //PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            Log.text = "Connect to Master Server...";
        }
    }

    public override void OnConnectedToMaster()
    {
        Log.text = "Online : Master Server Connected!";
        UserID.text = GetUserID();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Log.text = "Lobby Joined!";
        JoinRandomButton.interactable = true;
    }
    public override void OnCreatedRoom()
    {
        Log.text = "Create Room Success!";
    }
    public override void OnJoinedRoom()
    {
        Log.text = "Join Room Success!";
        StartCoroutine(LoadScene());
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text = "Error : Join Random Room Failed!";
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log.text = "Error : Create Room Failed!";
    }


    private string GetUserID()
    {
        string userID = PlayerPrefs.GetString("UserID");
        if (string.IsNullOrEmpty(userID))
            userID = "UserID_" + Random.Range(0, 999).ToString("000");
        return userID;
    }
    public void OnClickJoinRandomRoom()
    {
        JoinRandomButton.interactable = false;
        PhotonNetwork.NickName = UserID.text;
        PlayerPrefs.SetString("UserID", UserID.text);
        PhotonNetwork.JoinRandomRoom();
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
