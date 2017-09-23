using System.Collections.Generic;
using System.Linq;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;
using UnityEngine;

namespace TabletopCardCompanion.TouchScriptCustom.Gestures
{
    /// <summary>
    /// Point this to a Gesture you wish to make "exclusive". When the exclusive gesture becomes
    /// possible, all other gestures are cancelled.
    /// </summary>
    public class ExclusiveContinuousGesture : MonoBehaviour
    {
        /// <summary>
        /// The gesture to make "exclusive".
        /// </summary>
        public Gesture ExclusiveGesture; // TODO: accomodate multiple exclusive gestures

        [Tooltip("If true, ExclusiveGesture receives all pointers.")]
        public bool ReceiveAllPointers = true;

        [Tooltip("If true, ExclusiveGesture is friendly with all other gestures.")]
        public bool FriendlyWithAllGestures = true;

        /// <summary>
        /// All gestures which have received pointers.
        /// </summary>
        // TODO: store only gestures sharing pointers with ExclusiveGesture
        private IEnumerable<Gesture> _sharedGestures = new List<Gesture>();

        private void Start()
        {
            ExclusiveGesture.Delegate = new BooleanGestureDelegate(
                alwaysReceive: ReceiveAllPointers,
                alwaysRecognize: FriendlyWithAllGestures
                );
        }

        private void OnEnable()
        {
            ExclusiveGesture.StateChanged += StateChangedHandler;
            TouchManager.Instance.PointersPressed += PointersPressedHandler;
        }

        private void OnDisable()
        {
            ExclusiveGesture.StateChanged -= StateChangedHandler;

            var touchManager = TouchManager.Instance;
            if (touchManager != null)
            {
                touchManager.PointersPressed -= PointersPressedHandler;
            }
        }

        /// <summary>
        /// Record all gestures which received a pointer this frame.
        /// Also, cancel all other gestures if ExclusiveGesture becomes Possible.
        /// </summary>
        private void PointersPressedHandler(object sender, PointerEventArgs e)
        {
            foreach (Pointer pointer in e.Pointers)
            {
                var hit = pointer.GetOverData();
                var target = hit.Target;

                var gestures = target.GetComponentsInParent<Gesture>();
                _sharedGestures = _sharedGestures.Concat(gestures);
            }

            // When ExclusiveGesture becomes Possible, cancel all other gestures.
            // Note: Cancellation happens here instead of StateChangedHandler to prevent other
            //       gestures from acting at all.
            switch (ExclusiveGesture.State)
            {
                case Gesture.GestureState.Possible:
                case Gesture.GestureState.Began:
                    CancelGestures();
                    break;
            }
        }

        /// <summary>
        /// When ExclusiveGesture has ended, clear the list of shared gestures.
        /// </summary>
        private void StateChangedHandler(object sender, System.EventArgs e)
        {
            switch (ExclusiveGesture.State)
            {
                case Gesture.GestureState.Ended:
                case Gesture.GestureState.Cancelled:
                case Gesture.GestureState.Failed:
                    _sharedGestures = new List<Gesture>();
                    break;
            }
        }

        /// <summary>
        /// Cancel all other gestures.
        /// </summary>
        private void CancelGestures()
        {
            foreach (Gesture gesture in _sharedGestures)
            {
                if (gesture != ExclusiveGesture)
                {
                    gesture.Cancel();
                }
            }
        }
    }
}