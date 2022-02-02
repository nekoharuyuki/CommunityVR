using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Tooltip("Joins a specific room by name and creates it on demand.")]
    [SerializeField] private string roomName;

    bool triesToConnectToMaster = false;
    bool triesToConnectToRoom = false;

    //ルームオプションのプロパティー
    private readonly RoomOptions _roomOptions = new RoomOptions() {
        MaxPlayers = (byte)ConstantData.PlayerUpperLimit, //人数制限
        IsOpen = true, //部屋に参加できるか
        IsVisible = true, //この部屋がロビーにリストされるか
    };

    private void Update() {
        if (!PhotonNetwork.IsConnected && !triesToConnectToMaster) {
            ConnectToMaster();
        }
        if (PhotonNetwork.IsConnected && !triesToConnectToMaster && !triesToConnectToRoom) {
            StartCoroutine(WaitFrameAndConnect());
        }
    }

    public void ConnectToMaster() {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "PlayerName"; 
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v1";

        triesToConnectToMaster = true;

        //PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        triesToConnectToMaster = false;
        triesToConnectToRoom = false;
        Debug.Log(cause);
    }

    //マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        triesToConnectToMaster = false;
        Debug.Log("Connected to master!");
    }

    IEnumerator WaitFrameAndConnect() {
        triesToConnectToRoom = true;
        yield return new WaitForEndOfFrame();
        Debug.Log("Connecting");
        ConnectToRoom();
    }

    public void ConnectToRoom() {
        if (!PhotonNetwork.IsConnected)
            return;

        triesToConnectToRoom = true;

        //ルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom(roomName, _roomOptions, TypedLobby.Default);
    }

    //部屋への接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);

        SceneManager.LoadScene(roomName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)ConstantData.PlayerUpperLimit });
    }
}
