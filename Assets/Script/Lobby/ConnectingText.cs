using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConnectingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    void Start() {
        StartCoroutine("displayConnecting");
    }

    IEnumerator displayConnecting() {
        while (true) {
            text.text = "Connecting.  ";
            yield return new WaitForSeconds(0.5f);
            text.text = "Connecting.. ";
            yield return new WaitForSeconds(0.5f);
            text.text = "Connecting...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
