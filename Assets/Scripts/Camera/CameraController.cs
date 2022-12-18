using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.PSGCamera
{
    public class CameraController : MonoBehaviour
    {
        #region serialized variables

        [Header("Camera pan settings")]
        [SerializeField]
        private float cameraPanSpeed;

        [SerializeField]
        private float distanceFromEdge;

        [SerializeField]
        private AnimationCurve xPositionBounds;

        [SerializeField]
        private AnimationCurve zUpPositionBounds;

        [SerializeField]
        private AnimationCurve zDownPositionBounds;

        [Header("Zoom settings")]
        [SerializeField]
        private float fovChangeSpeed;

        [SerializeField]
        private AnimationCurve fovCurve;

        [SerializeField]
        private AnimationCurve heightCurve;

        [SerializeField]
        private AnimationCurve rotationCurve;

        [SerializeField]
        [Range(0, 1)]
        private float startingZoom;

        #endregion

        #region private variables

        private Vector2 screenSize;
        private Vector2 cameraMovement;

        private Camera mainCamera;

        Vector3 originalPosition;
        Quaternion originalRotation;


        Vector3? previousMousePosition;

        private float currentZoom;

        Vector3 targetPosition;
        Quaternion targetRotation;
        float targetFOV;

        float boundsZUp;
        float boundsZDown;
        float boundsX;
        float height;

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
            originalRotation = transform.rotation;

            screenSize = new Vector2(Screen.width, Screen.height);
            mainCamera = Camera.main;

            currentZoom = startingZoom;

            UpdateCameraZoom();
        }

        void Update()
        {
            SetCameraZoom();
            SetCameraMovement();
        }

        #region Private methods

        /// <summary>
        /// Sets camera movement variable based on controls.
        /// </summary>
        private void SetCameraMovement()
        {
            cameraMovement = Vector2.zero;
            var mousePosition = Input.mousePosition;

            // Save mouse drag position
            if (Input.GetMouseButtonDown(2))
                previousMousePosition = mousePosition;

            if (Input.GetMouseButtonUp(2))
            {
                previousMousePosition = null;
            }

            // Check mouse pan
            if (previousMousePosition != null)
            {
                cameraMovement = (previousMousePosition.Value - mousePosition).normalized * cameraPanSpeed * 6; // Hard coded multiplier
                previousMousePosition = mousePosition;
            }
            // Check screen edges
            else
            {
                if (mousePosition.x <= distanceFromEdge)
                    cameraMovement.x = -cameraPanSpeed;
                else if (mousePosition.x >= screenSize.x - distanceFromEdge)
                    cameraMovement.x = cameraPanSpeed;

                if (mousePosition.y <= distanceFromEdge - distanceFromEdge)
                    cameraMovement.y = -cameraPanSpeed;
                else if (mousePosition.y >= screenSize.y - distanceFromEdge)
                    cameraMovement.y = cameraPanSpeed;
            }

            // Apply bounds
            targetPosition = transform.position + new Vector3(cameraMovement.x, 0, cameraMovement.y);
            targetPosition.x = Mathf.Clamp(targetPosition.x, originalPosition.x - boundsX, originalPosition.x + boundsX);
            targetPosition.y = height;
            targetPosition.z = Mathf.Clamp(targetPosition.z, originalPosition.z - boundsZDown, originalPosition.z + boundsZUp);
            
            // Apply movement, rotation and FOV
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraPanSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * cameraPanSpeed);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * cameraPanSpeed);

            if (cameraMovement != Vector2.zero)
                OnCameraPan?.Invoke(this, null);
        }


        private void SetCameraZoom()
        {
            var lastZoom = currentZoom;

            currentZoom = Mathf.Clamp(currentZoom + Input.GetAxis("Mouse ScrollWheel") * fovChangeSpeed * Time.deltaTime, 0, 1);

            if(lastZoom != currentZoom)
            {
                UpdateCameraZoom();
                OnCameraZoom?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Y is position, X and Z are abs. bounds.
        /// </summary>
        private void UpdateCameraZoom()
        {
            boundsX = xPositionBounds.Evaluate(currentZoom);
            boundsZUp = zUpPositionBounds.Evaluate(currentZoom);
            boundsZDown = zDownPositionBounds.Evaluate(currentZoom);
            height = originalPosition.y + heightCurve.Evaluate(currentZoom);


            targetFOV = fovCurve.Evaluate(currentZoom);
            targetRotation = Quaternion.Euler(originalRotation.eulerAngles + new Vector3(rotationCurve.Evaluate(currentZoom), 0, 0));
        }

        #endregion
    }
}