using Assets.RoomBuilder.Scripts;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
#endif

public class ModelsPlacementController : MonoBehaviour
{
    public Camera FirstPersonCamera;

    private bool _isQuitting = false;

    void Start()
    {
    }

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Update()
    {
        UpdateApplicationLifecycle();

        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (InstantPreviewInput.touchCount < 1 || (touch = InstantPreviewInput.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Should not handle input if the player is pointing on UI.
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        bool foundHit = false;

        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
        foundHit = Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit);

        if (foundHit)
        {
            if (hit.Trackable is DetectedPlane && Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                hit.Pose.rotation * Vector3.up) < 0)
            {
                DebugMessanger.ShowAndroidToastMessage("Click on background position!");
            }
            else
            {
                DebugMessanger.ShowAndroidToastMessage("Norm click!");
            }
        }
    }

    private void UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (_isQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void DoQuit()
    {
        Application.Quit();
    }
}