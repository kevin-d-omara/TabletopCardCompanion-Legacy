using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

namespace TabletopCardCompanion.GameElement
{
    /// <summary>
    /// Base class for all TabletopCardCompanion game pieces.
    /// </summary>
    [RequireComponent(typeof(TapGesture))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TransformGesture))]
    [RequireComponent(typeof(Transformer))]
    public class GameElementController : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// Height to raise objects when lifted.
        /// </summary>
        protected static readonly float LIFT_HEIGHT = 5f;

        /// <summary>
        /// Vertical buffer between objects on top of each other.
        /// </summary>
        protected static readonly float HEIGHT_BUFFER = 0.01f;

        #endregion

        #region Events



        #endregion

        #region Public Properties

        public ToggleOptions Toggles { get; protected set; } = new ToggleOptions();

        #endregion

        #region Private/Protected Variables

        // Components
        protected TapGesture tapGesture;
        protected BoxCollider2D boxCollider;
        protected TransformGesture transformGesture;

        // Child Game Objects
        [SerializeField] protected GameObject canvasObject;
        [SerializeField] protected GameObject imageObject;

        // Child Components
        protected TwoSidedImage twoSidedImage;

        #endregion

        #region Public Methods



        #endregion

        #region Unity Methods

        protected void Awake()
        {
            // Components
            tapGesture = GetComponent<TapGesture>();
            boxCollider = GetComponent<BoxCollider2D>();
            transformGesture = GetComponent<TransformGesture>();

            // Child Components
            twoSidedImage = imageObject.GetComponent<TwoSidedImage>();
        }

        protected void OnEnable()
        {
            tapGesture.Tapped += TappedHandler;
            twoSidedImage.SizeChanged += SizeChangedHandler;
            transformGesture.StateChanged += TransformStateChangedHandler;
        }

        protected void OnDisable()
        {
            tapGesture.Tapped -= TappedHandler;
            twoSidedImage.SizeChanged -= SizeChangedHandler;
            transformGesture.StateChanged -= TransformStateChangedHandler;
        }

        #if UNITY_STANDALONE
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
        #endif

        #endregion

        #region Protected Methods



        #endregion

        #region Callbacks



        #endregion

        #region Private Methods



        #endregion

        #region Event Handlers

        /// <summary>
        /// Flip the card over when tapped.
        /// </summary>
        protected void TappedHandler(object sender, System.EventArgs e)
        {
            twoSidedImage.Flip();
            // TODO: magnify object instead of flipping
        }

        /// <summary>
        /// Resize collider to match the new image.
        /// </summary>
        protected void SizeChangedHandler(object sender, Bounds bounds)
        {
            boxCollider.size = bounds.size;
        }

        #endregion

        #region Temporary

        // Object Movement -------------------------------------------------------------------------
        // \!/ This is going to be moved to a new TouchScript Behavior.
        protected bool isMoving;

        /// <summary>
        /// Object is picked up, moved, or set down.
        /// </summary>
        protected void TransformStateChangedHandler(object sender, GestureStateChangeEventArgs gestureStateChangeEventArgs)
        {
            switch (gestureStateChangeEventArgs.State)
            {
                case Gesture.GestureState.Possible:
                    if (Toggles.Locked) transformGesture.Cancel();
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

        private void Move() { }

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

        #endregion
    }
}