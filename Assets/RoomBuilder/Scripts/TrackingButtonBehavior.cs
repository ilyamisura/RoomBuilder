using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackingButtonBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTrackingButtonClick()
    {
        SceneManager.LoadScene("MarkersTracking");
    }
}
