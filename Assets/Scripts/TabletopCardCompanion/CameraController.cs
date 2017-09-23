using TouchScript.Gestures.TransformGestures;
using UnityEngine;

namespace TabletopCardCompanion
{
    /// <summary>
    /// Controls the main camera, which responds to the following behavior:
    /// Touch
    ///     Zoom (2-finger) (pinch/pull)
    ///     Pan  (2-finger) (drag both fingers in same direction)
    ///     Pan  (1-finger) (drag; only works if starting touch point hits no other objects)
    /// Keyboard/Mouse
    ///     Zoom (Scroll Wheel)
    ///     Pan  (WASD and Arrow Keys)
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera _cam; // Note: multiplayer: 1 personal camera per player, ~9 shared cameras/views

        [Header("Table")]
        [SerializeField] private GameObject table;

        [Header("Pan")]
        // TODO: make pan be 1:1 with pointer movement, regardless of screen size/dpi
        [SerializeField] [Range(0.0f, 1.0f)] private float PanSpeedTouch = 0.05f;
        [SerializeField] [Range(1.0f, 100.0f)] private float PanSpeedKeyboard = 1f;
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

        /// <summary>
        /// Pan the camera by touch input.
        /// </summary>
        public void OneFingerPanHandler(object sender, System.EventArgs e)
        {
            Pan(-_oneFingerPanGesture.DeltaPosition, PanSpeedTouch);
        }

        /// <summary>
        /// Pan and Zoom the camera by touch input.
        /// </summary>
        public void ClusteredMoveHandler(object sender, System.EventArgs e)
        {
            Pan(-_clusteredMoveGesture.DeltaPosition, PanSpeedTouch);
            Zoom(_clusteredMoveGesture.DeltaScale);
        }

        /// <summary>
        /// Pan and Zoom the camera by keyboard/mouse input.
        /// </summary>
        private void Update()
        {
            var dx = Input.GetAxis("Horizontal");
            var dy = Input.GetAxis("Vertical");
            Pan(new Vector3(dx, dy, 0f), PanSpeedKeyboard);

            var dS = Input.GetAxis("Mouse ScrollWheel");
            Zoom(dS + 1.0f);
        }

        /// <summary>
        /// Pan the camera in a direction.
        /// </summary>
        /// <param name="deltaPosition">Direction & magnitude to move the camera.</param>
        /// <param name="speed">Multiplier to affect distance moved.</param>
        private void Pan(Vector3 deltaPosition, float speed)
        {
            var multiplier = speed * (_cam.orthographicSize * PanZoomLevelMultiplier);
            var newPosition = _cam.transform.position + deltaPosition * multiplier;
            newPosition.x = Bound(newPosition.x, -panBounds.x, panBounds.x);
            newPosition.y = Bound(newPosition.y, -panBounds.y, panBounds.y);
            _cam.transform.position = newPosition;
        }

        /// <summary>
        /// Zoom the camera in or out.
        /// </summary>
        /// <param name="deltaScale">Relative amount to zoom; should be approximately 1.0.</param>
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