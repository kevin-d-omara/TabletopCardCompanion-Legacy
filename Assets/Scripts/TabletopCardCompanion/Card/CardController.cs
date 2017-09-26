using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

namespace TabletopCardCompanion.Card
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TapGesture))]
    [RequireComponent(typeof(TransformGesture))]
    public class CardController : MonoBehaviour
    {
        [SerializeField] private TwoSidedImage twoSidedImage;

        // Components
        private BoxCollider2D boxCollider;
        private TapGesture tapGesture;
        private TransformGesture transformGesture;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            tapGesture = GetComponent<TapGesture>();
            transformGesture = GetComponent<TransformGesture>();
        }

        private void OnEnable()
        {
            tapGesture.Tapped += TappedHandler;
            twoSidedImage.SizeChanged += SizeChangedHandler;
            transformGesture.StateChanged += TransformStateChangedHandler;
        }

        private void OnDisable()
        {
            tapGesture.Tapped -= TappedHandler;
            twoSidedImage.SizeChanged -= SizeChangedHandler;
            transformGesture.StateChanged -= TransformStateChangedHandler;
        }

        /// <summary>
        /// Check for keyboard / mouse input while the mouse is hovering over this object.
        /// </summary>
        private void OnMouseOver()
        {
            if (Input.GetButtonDown("Flip"))
            {
                twoSidedImage.Flip();
            }
        }

        /// <summary>
        /// Flip the card over when tapped.
        /// </summary>
        private void TappedHandler(object sender, System.EventArgs e)
        {
            twoSidedImage.Flip();
        }

        /// <summary>
        /// Resize the BoxCollider2D size to match the new image.
        /// </summary>
        private void SizeChangedHandler(object sender, Bounds bounds)
        {
            boxCollider.size = bounds.size;
        }



        // Object movement -------------------------------------------------------------------------

        public static readonly float LIFT_HEIGHT = 5f;     // height to raise objects when lifted
        public static readonly float HEIGHT_BUFFER = 0.01f;  // vertical buffer between objects on top of each other

        // Toggles
        /// <summary>
        /// Freezes the object in place stopping all physics interactions.
        /// </summary>
        public bool TransformLocked { get; private set; }

        /// <summary>
        /// When picked up objects above this one will be attached to it.
        /// </summary>
        public bool IsSticky { get; private set; } = true;

        private bool isMoving;

        /// <summary>
        /// Object is picked up, moved, or set down.
        /// </summary>
        private void TransformStateChangedHandler(object sender, GestureStateChangeEventArgs gestureStateChangeEventArgs)
        {
            switch (gestureStateChangeEventArgs.State)
            {
                case Gesture.GestureState.Possible:
                    if (TransformLocked) transformGesture.Cancel();
                    break;
                case Gesture.GestureState.Changed:
                    if (!isMoving) PickUp();
                    Move();
                    break;
                case Gesture.GestureState.Ended:
                case Gesture.GestureState.Cancelled:
                    SetDown();
                    break;
                case Gesture.GestureState.Failed:
                case Gesture.GestureState.Idle:
                    if (gestureStateChangeEventArgs.PreviousState == Gesture.GestureState.Possible) SetDown();
                    break;
            }
        }

        private void PickUp()
        {
            isMoving = true;
            transform.position += Vector3.back * LIFT_HEIGHT;
        }

        private void Move() {}

        private void SetDown()
        {
            isMoving = false;
            var cardBelow = Physics2D.OverlapBox(transform.position, boxCollider.size / 2f, angle: 0f,
                layerMask: Physics2D.DefaultRaycastLayers,
                minDepth: transform.position.z + 0.001f);

            if (cardBelow != null)
            {
                var heightBetween = cardBelow.transform.position.z - transform.position.z;
                transform.position += Vector3.forward * (heightBetween - HEIGHT_BUFFER);
            }
            else
            {
                transform.position += Vector3.forward * -transform.position.z;
            }
        }
    }
}