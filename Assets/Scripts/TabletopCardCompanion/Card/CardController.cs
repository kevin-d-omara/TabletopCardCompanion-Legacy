using TouchScript.Gestures;
using UnityEngine;

namespace TabletopCardCompanion
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TapGesture))]
    public class CardController : MonoBehaviour
    {
        [SerializeField] private TwoSidedImage twoSidedImage;

        // Components
        private BoxCollider2D boxCollider;
        private TapGesture tapGesture;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            tapGesture = GetComponent<TapGesture>();
        }

        private void OnEnable()
        {
            tapGesture.Tapped += TappedHandler;
            twoSidedImage.SizeChanged += SizeChangedHandler;
        }

        private void OnDisable()
        {
            tapGesture.Tapped -= TappedHandler;
            twoSidedImage.SizeChanged -= SizeChangedHandler;
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
    }
}