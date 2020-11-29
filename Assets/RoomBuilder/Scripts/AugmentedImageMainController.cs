using System.Collections;
using System.Collections.Generic;
using Assets.RoomBuilder.Scripts;
using GoogleARCore;
using UnityEngine;

public class AugmentedImageMainController : MonoBehaviour
{
    public MarkerVisualizer[] markerVisualizerPrefabs;

    private Dictionary<int, MarkerVisualizer> _visualizers
        = new Dictionary<int, MarkerVisualizer>();

    private List<AugmentedImage> _tempAugmentedImages = new List<AugmentedImage>();

    public void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        DebugMessanger.ShowAndroidToastMessage("Hello world!");
    }

    public void Update()
    {
        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(_tempAugmentedImages, TrackableQueryFilter.Updated);
        // Create visualizers and anchors for updated augmented images that are tracking and do
        // not previously have a visualizer. Remove visualizers for stopped images.
        foreach (var image in _tempAugmentedImages)
        {
            _visualizers.TryGetValue(image.DatabaseIndex, out var marker);

            if (image.TrackingState == TrackingState.Tracking && marker == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
            {
                DebugMessanger.ShowAndroidToastMessage($"Tracking image with index {image.DatabaseIndex}");
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                marker = (MarkerVisualizer) Instantiate(markerVisualizerPrefabs[image.DatabaseIndex], anchor.transform);
                marker.Image = image;
                _visualizers.Add(image.DatabaseIndex, marker);
            }
            else if (image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && marker != null)
            {
                DebugMessanger.ShowAndroidToastMessage($"Destroy object {image.DatabaseIndex}");
                _visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(marker.gameObject);
            }
            // else if(image.TrackingState == TrackingState.Tracking && marker != null)
            // {
            //     if (Time.frameCount % 300 == 0)
            //     {
            //         DebugMessanger.ShowAndroidToastMessage($"Destroy object {image.DatabaseIndex}");
            //     }
            // }
        }
    }
}