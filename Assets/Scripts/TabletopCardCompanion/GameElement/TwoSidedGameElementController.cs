using UnityEngine;

namespace TabletopCardCompanion.GameElement
{
    [RequireComponent(typeof(TwoSidedImage))]
    public class TwoSidedGameElementController : GameElementController
    {
        #region Constants



        #endregion

        #region Events



        #endregion

        #region Public Properties



        #endregion

        #region Private/Protected Variables

        // Child Game Objects
        [SerializeField] protected GameObject canvasObject;
        [SerializeField] protected GameObject imageObject;

        // Child Components
        protected TwoSidedImage twoSidedImage;

        #endregion

        #region Public Methods

        public override void Flip()
        {
            twoSidedImage.Flip();
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            // Child Components
            twoSidedImage = imageObject.GetComponent<TwoSidedImage>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            twoSidedImage.SizeChanged += SizeChangedHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            twoSidedImage.SizeChanged -= SizeChangedHandler;
        }

        #endregion

        #region Protected Methods



        #endregion

        #region Callbacks



        #endregion

        #region Private Methods



        #endregion

        #region Event Handlers

        /// <summary>
        /// Resize collider to match the new image.
        /// </summary>
        protected void SizeChangedHandler(object sender, Bounds bounds)
        {
            boxCollider.size = bounds.size;
        }

        #endregion
    }
}