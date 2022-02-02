using Photon.Pun;
using UnityEngine;
using Photon.Voice.Unity;

public class SetMicrophone : MonoBehaviourPun {
    //デバイスのマイクを検出し、PhotonVoice の Recorder コンポーネントに設定します
    private void Start() {
        string[] devices = Microphone.devices;
        if (devices.Length > 0) {
            GetComponent<Recorder>().UnityMicrophoneDevice = devices[0];
        }
    }
}
