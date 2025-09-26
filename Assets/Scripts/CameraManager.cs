using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera followCamera;
    public CinemachineCamera panoramicCamera;

    public void SwitchToFollowCam()
    {
        // Set the priority of the follow camera higher to make it active.
        followCamera.Priority = 10;
        panoramicCamera.Priority = 5;
    }

    public void SwitchToPanoramicCam()
    {
        // Set the priority of the panoramic camera higher to make it active.
        panoramicCamera.Priority = 10;
        followCamera.Priority = 5;
    }
}