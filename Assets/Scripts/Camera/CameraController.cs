using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.PSGCamera
{
    public class CameraController : MonoBehaviour
    {
        #region serialized variables

        [Header("Camera pan bounds")]
        [SerializeField]
        private float boundsXMin;
        [SerializeField]
        private float boundsXMax;

        [SerializeField]
        private float boundsZMin;
        [SerializeField]
        private float boundsZMax;

        [Header("Camera pan settings")]
        [SerializeField]
        private float cameraPanSpeed;

        [SerializeField]
        private float distanceFromEdge;

        [Header("Camera zoom settings")]
        [SerializeField]
        public float minFovScale;

        [SerializeField]
        public float maxFovScale;

        [SerializeField]
        public float fovChangeSpeed;

        [SerializeField]
        public float fovMultiplier;

        #endregion

        #region private variables

        private Vector2 screenSize;
        private Vector2 cameraMovement;

        private float currentFovScale = 1;

        private Camera mainCamera;

        private float currentBoundsXMin;
        private float currentBoundsXMax;
        private float currentBoundsZMin;
        private float currentBoundsZMax;

        Vector3 originalPosition;

        #endregion

        #region Event handlers

        public EventHandler OnCameraPan;
        public EventHandler OnCameraZoom;

        #endregion

        #region properties

        #endregion

        void Start()
        {
            originalPosition = transform.position;

            screenSize = new Vector2(Screen.width, Screen.height);
            mainCamera = Camera.main;
        }

        void Update()
        {
            SetCameraMovement();
            SetCameraZoom();

            transform.Translate(cameraMovement * Time.deltaTime * cameraPanSpeed);

            mainCamera.fieldOfView = currentFovScale * fovMultiplier;
        }
        #region Private methods

        /// <summary>
        /// Sets camera movement variable based on controls.
        /// </summary>
        private void SetCameraMovement()
        {
            cameraMovement = Vector2.zero;

            // Check if we are out of bounds
            if (transform.position.x < currentBoundsXMin + originalPosition.x)
                cameraMovement.x = -transform.position.x + currentBoundsXMin + originalPosition.x;
            else if (transform.position.x > currentBoundsXMax + originalPosition.x)
                cameraMovement.x = -transform.position.x + currentBoundsXMax + originalPosition.x;

            if (transform.position.z < currentBoundsZMin + originalPosition.z)
                cameraMovement.y = -transform.position.z + currentBoundsZMin + originalPosition.z;
            else if (transform.position.z > currentBoundsZMax + originalPosition.z)
                cameraMovement.y = -transform.position.z + currentBoundsZMax + originalPosition.z;

            // Only pan by mouse if we are inside the bounds
            if (cameraMovement == Vector2.zero)
            {
                var mousePosition = Input.mousePosition;

                if (mousePosition.x <= distanceFromEdge && transform.position.x > currentBoundsXMin + originalPosition.x)
                    cameraMovement.x = -cameraPanSpeed;
                else if (mousePosition.x >= screenSize.x - distanceFromEdge && transform.position.x < currentBoundsXMax + originalPosition.x)
                    cameraMovement.x = cameraPanSpeed;

                if (mousePosition.y <= distanceFromEdge - distanceFromEdge && transform.position.z > currentBoundsZMin + originalPosition.z)
                    cameraMovement.y = -cameraPanSpeed;
                else if (mousePosition.y >= screenSize.y - distanceFromEdge && transform.position.z < currentBoundsZMax + originalPosition.z)
                    cameraMovement.y = cameraPanSpeed;
            } else
            {
                Debug.Log("Out of bounds");
            }

            if (cameraMovement != Vector2.zero)
                OnCameraPan?.Invoke(this, null);
        }


        private void SetCameraZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            var previousFovScale = currentFovScale;

            currentFovScale = Mathf.Clamp(currentFovScale - scroll * fovChangeSpeed * Time.deltaTime, minFovScale, maxFovScale);

            currentBoundsXMin = boundsXMin - boundsXMin * currentFovScale;
            currentBoundsXMax = boundsXMax - boundsXMax * currentFovScale;
            currentBoundsZMin = boundsZMin - boundsZMin * currentFovScale;
            currentBoundsZMax = boundsZMax - boundsZMax * currentFovScale;

            if (previousFovScale != currentFovScale)
                OnCameraZoom?.Invoke(this, null);
        }

        #endregion
    }
}