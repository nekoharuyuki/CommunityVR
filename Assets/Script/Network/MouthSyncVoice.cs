using Photon.Pun;
using Photon.Voice.PUN;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// しゃべると口が動く機能
/// </summary>
public class MouthSyncVoice : MonoBehaviourPun
{
    [SerializeField] private Transform _mouth;

    private PhotonVoiceView _voice;
    private float _mouthSize;

    void Start()
    {
        if (photonView.IsMine)
        {
            _voice = GetComponent<PhotonVoiceView>();
            _voice.RecorderInUse.TransmitEnabled = true;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    //口のオブジェクトのY軸のスケールをLerpで滑らかに動かす
                    float targetMouthSize = Mathf.Lerp(0.1f, 1.0f, 100 * _voice.RecorderInUse.LevelMeter.CurrentAvgAmp);
                    _mouthSize = Mathf.Lerp(_mouthSize, targetMouthSize, 30.0f * Time.deltaTime);

                    //口の動きを同期通信させる
                    photonView.RPC(nameof(SyncMouth), RpcTarget.All, _mouthSize);
                })
                .AddTo(this);
        }
    }

    /// <summary>
    /// 口の動きの変化を送信
    /// </summary>
    /// <param name="mouthSize">口の大きさ</param>
    [PunRPC]
    private void SyncMouth(float mouthSize)
    {
        Vector3 localScale = _mouth.localScale;
        localScale.y = mouthSize;
        _mouth.localScale = localScale;
    }
}
