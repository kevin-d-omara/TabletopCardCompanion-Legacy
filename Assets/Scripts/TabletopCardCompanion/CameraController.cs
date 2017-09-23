using TouchScript.Gestures.TransformGestures;
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
        [SerializeField] private Camera _cam; // Note: multiplayer: 1 personal camera per player, ~9 shared cameras/views

        [Header("Table")]
        [SerializeField] private GameObject table;

        [Header("Pan")]
        // TODO: make pan be 1:1 with pointer movement, regardless of screen size/dpi
        [SerializeField] [Range(0.0f, 1.0f)] private float PanSpeed = 0.05f;
        [SerializeField] [Range(0.0f, 1.0f)] private float PanZoomLevelMultiplier = 0.05f;
        [SerializeField] private Vector2 panBounds;

        [Header("Zoom")]
        [SerializeField] private Vector2 zoomBounds;

        [Header("Gestures")]
        [SerializeField] private ScreenTransformGesture _oneFingerPanGesture;
        [SerializeField] private ScreenTransformGesture _clusteredMoveGesture;

        private void Start()
        {
            var tableSprite = table.GetComponent<SpriteRenderer>().sprite;
            panBounds = tableSprite.bounds.extents; // TODO: make TableController script w/ callback for updating table sprite
        }

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
            var newPosition = _cam.transform.position - deltaPosition * multiplier;
            newPosition.x = Bound(newPosition.x, -panBounds.x, panBounds.x);
            newPosition.y = Bound(newPosition.y, -panBounds.y, panBounds.y);
            _cam.transform.position = newPosition;
        }

        /// <summary>
        /// Zoom the camera in the standard way: pinch == zoom out, pull == zoom in
        /// </summary>
        private void Zoom(float deltaScale)
        {
            var newSize = _cam.orthographicSize * (2f - deltaScale); // reflect about 1.0
            _cam.orthographicSize = Bound(newSize, zoomBounds.x, zoomBounds.y);
        }

        /// <summary>
        /// Make sure value is between min and max.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Maximum value allowed.</param>
        /// <param name="max">Minimum value allowed.</param>
        /// <returns><c>value</c> if it is between min and max; <c>min</c> or <c>max</c> otherwise.</returns>
        private float Bound(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}