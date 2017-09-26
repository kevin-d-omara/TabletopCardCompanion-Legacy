using System;
using UnityEngine;
using UnityEngine.UI;

namespace TabletopCardCompanion
{
    /// <summary>
    /// Represents a two-sided image (i.e. a playing card or board tile).
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class TwoSidedImage : MonoBehaviour
    {
        public event EventHandler<Bounds> SizeChanged;

        [SerializeField] private Sprite front;
        [SerializeField] private Sprite back;
        private bool isFront = true;

        // Components
        private RectTransform rectTransform;
        private Image image;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        private void Start()
        {
            image.sprite = front;
            image.SetNativeSize();
            SizeChanged?.Invoke(this, front.bounds);
        }

        /// <summary>
        /// "Flips" over the object in a way that looks physical.
        /// </summary>
        public void Flip()
        {
            var newSprite = isFront ? back : front;
            isFront = !isFront;

            // (?) scale x or rotate?
            // Lerp rectTransform.x to zero.
            // Image.sprite = newSprite
            // var newWidth = newSprite.width
            // Lerp rectTransform.x to newWidth.

            // Temporary hack:
            image.sprite = newSprite;
            image.SetNativeSize();
            SizeChanged?.Invoke(this, newSprite.bounds);
        }
    }
}