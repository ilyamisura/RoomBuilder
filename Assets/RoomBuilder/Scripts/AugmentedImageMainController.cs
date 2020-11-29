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
            MarkerVisualizer marker = null;
            
            _visualizers.TryGetValue(image.DatabaseIndex, out marker);
            if (image.TrackingState == TrackingState.Tracking && marker == null)
            {
                DebugMessanger.ShowAndroidToastMessage($"Tracking image with index {image.DatabaseIndex}");
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                marker = (MarkerVisualizer) Instantiate(markerVisualizerPrefabs[0], anchor.transform);
                marker.Image = image;
                _visualizers.Add(image.DatabaseIndex, marker);
            }
            else if (image.TrackingState == TrackingState.Stopped && marker != null)
            {
                _visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(marker.gameObject);
            }
        }
    }
}