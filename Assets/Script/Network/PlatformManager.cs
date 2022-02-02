using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// アバター接続に関する
/// </summary>
public class PlatformManager : MonoBehaviourPunCallbacks {

    [SerializeField] private GameObject _avatar;

    //Modes
    public enum Mode { VR, Screen };
    [Tooltip("Choose the mode before building")]
    public Mode mode;

    void Awake() {
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene(0);
        }
        if (mode == Mode.Screen) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    private void Start() {
        // VRアバターを生成
        GameObject avatar = PhotonNetwork.Instantiate(
        _avatar.name,
        Vector3.zero,
        Quaternion.identity);
        avatar.name = _avatar.name;
    }
}
