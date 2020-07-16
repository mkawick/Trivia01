using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwingAndZoom : MonoBehaviour
{
    [SerializeField]
    Transform cameraZoomOnPlayerPosition;
    [SerializeField]
    PlayerAnimController playerAnimController;
    [SerializeField]
    float timeForZoom = 2, timeToHangOnAnim = 5, timeToRestore = 1;

    Vector3 cameraOriginalPosition;
    Quaternion cameraOriginalRotation;
    bool isZooming = false,  isZoomed = false;
    float timeWhenZoomCompletes = 0, timeToCompleteForPause = 0;

    void Start()
    {
        cameraOriginalPosition = this.transform.position;
        cameraOriginalRotation = this.transform.rotation;
        var player = playerAnimController.GetComponent<PlayerWithCollider>();
        player.cameraSwingAndZoom = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BeginSadnessState(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BeginSadnessState(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BeginSadnessState(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BeginSadnessState(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BeginSadnessState(4);
        }
        if (isZooming)
        {
            if (timeWhenZoomCompletes < Time.time)
            {
                isZooming = false;
                timeToCompleteForPause = Time.time + timeToHangOnAnim;
                isZoomed = true;
            }
        }
        if(isZoomed)
        {
            if (timeToCompleteForPause < Time.time)
            {
                isZoomed = false;
                iTween.MoveTo(this.transform.gameObject, cameraOriginalPosition, timeToRestore);
                iTween.RotateTo(this.transform.gameObject, cameraOriginalRotation.eulerAngles, timeToRestore);
                playerAnimController.SadnessState(false, 0);
            }
        }
    }

    internal void BeginSadnessState(int which)
    {
        BeginCameraZoom();

        playerAnimController.SadnessState(true, which);
    }


    internal void BeginCelebrationState()
    {
        BeginCameraZoom();
        playerAnimController.Celebrate();
    }

    void BeginCameraZoom()
    {
        timeWhenZoomCompletes = Time.time + timeForZoom;
        iTween.MoveTo(this.transform.gameObject, cameraZoomOnPlayerPosition.position, timeForZoom);
        iTween.RotateTo(this.transform.gameObject, cameraZoomOnPlayerPosition.rotation.eulerAngles, timeForZoom);
        isZooming = true;
    }
}
