﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwingAndZoom : MonoBehaviour
{
    [SerializeField]
    Transform cameraZoomOnPlayerPosition;
    [SerializeField]
    PlayerAnimController playerAnimController;

    Vector3 cameraOriginalPosition;
    Quaternion cameraOriginalRotation;
    bool isZooming = false,  isZoomed = false;
    float timeWhenZoomCompletes = 0, timeToCompleteForPause = 0;

    void Start()
    {
        cameraOriginalPosition = this.transform.position;
        cameraOriginalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            float timeToZoom = 2.0f;
            timeWhenZoomCompletes = Time.time + timeToZoom;
            iTween.MoveTo(this.transform.gameObject, cameraZoomOnPlayerPosition.position, timeToZoom);
            iTween.RotateTo(this.transform.gameObject, cameraZoomOnPlayerPosition.rotation.eulerAngles, timeToZoom);
            isZooming = true;
            playerAnimController.SadnessState(true);
        }
        if (isZooming)
        {
            if (timeWhenZoomCompletes < Time.time)
            {
                isZooming = false;
                float timeToPause = 2.0f;
                timeToCompleteForPause = Time.time * timeToPause;
                isZoomed = true;
            }
        }
        if(isZoomed)
        {
            if (timeToCompleteForPause < Time.time)
            {
                isZoomed = false;
                float returnTime = 1.0f;
                iTween.MoveTo(this.transform.gameObject, cameraOriginalPosition, returnTime);
                iTween.RotateTo(this.transform.gameObject, cameraOriginalRotation.eulerAngles, returnTime);
                playerAnimController.SadnessState(false);
            }
        }
    }
}
