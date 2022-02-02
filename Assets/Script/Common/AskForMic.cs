using UnityEngine;
using UnityEngine.Android;

public class AskForMic : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        string mic = Microphone.devices[0];
    }
}
