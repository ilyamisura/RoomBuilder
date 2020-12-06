using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

public class MarkerVisualizer : MonoBehaviour
{
    public AugmentedImage Image;

    public GameObject CubeModel;

    public void Update()
    {
        //if (Image == null || Image.TrackingState != TrackingState.Tracking)
        //{
        //    CubeModel.SetActive(false);
        //    return;
        //}

        //CubeModel.SetActive(true);
    }
}
