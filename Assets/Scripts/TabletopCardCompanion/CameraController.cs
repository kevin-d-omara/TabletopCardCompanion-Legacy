using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures.TransformGestures.Clustered;
using UnityEngine;

namespace TabletopCardCompanion
{
    /// <summary>
    /// Controls the main camera, which responds to the following behavior:
    ///     Zoom (2-finger) (pinch/pull)
    ///     Pan  (2-finger) (drag both fingers in same direction)
    ///     Pan  (1-finger) (drag; only works if starting touch point hits no other objects)
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera _cam;

        [Header("Pan")]
        // TODO: make pan be 1:1 with pointer movement, regardless of screen size/dpi
        [Range(0.0f, 1.0f)] public float PanSpeed = 0.05f;
        [Range(0.0f, 1.0f)] public float PanZoomLevelMultiplier = 0.05f;

        [Header("Zoom")]
        [SerializeField] private float minZoom;
        [SerializeField] private float maxZoom;

        [Header("Gestures")]
        [SerializeField] private ScreenTransformGesture _oneFingerPanGesture;
        [SerializeField] private ScreenTransformGesture _clusteredMoveGesture;

        private void OnEnable()
        {
            _oneFingerPanGesture.Transformed  += OneFingerPanHandler;
            _clusteredMoveGesture.Transformed += ClusteredMoveHandler;
        }

        private void OnDisable()
        {
            _oneFingerPanGesture.Transformed -= OneFingerPanHandler;
            _clusteredMoveGesture.Transformed -= ClusteredMoveHandler;
        }

        public void OneFingerPanHandler(object sender, System.EventArgs e)
        {
            Pan(_oneFingerPanGesture.DeltaPosition);
        }

        public void ClusteredMoveHandler(object sender, System.EventArgs e)
        {
            Pan(_clusteredMoveGesture.DeltaPosition);
            Zoom(_clusteredMoveGesture.DeltaScale);
        }

        /// <summary>
        /// Pan the camera opposite of the direction moved.
        /// </summary>
        private void Pan(Vector3 deltaPosition)
        {
            var multiplier = PanSpeed * (_cam.orthographicSize * PanZoomLevelMultiplier);
            _cam.transform.position -= deltaPosition * multiplier;
        }

        /// <summary>
        /// Zoom the camera in the standard way: pinch == zoom out, pull == zoom in
        /// </summary>
        private void Zoom(float deltaScale)
        {
            var newSize = _cam.orthographicSize * (2f - deltaScale); // reflect about 1.0
            if (newSize < minZoom) newSize = minZoom;
            if (newSize > maxZoom) newSize = maxZoom;
            _cam.orthographicSize = newSize;
        }
    }
}