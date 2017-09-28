using TabletopCardCompanion.GameElement;
using UnityEngine;

namespace TabletopCardCompanion
{
    public class CardController : TwoSidedGameElementController
    {
        #region Constants



        #endregion

        #region Events



        #endregion

        #region Public Properties



        #endregion

        #region Private/Protected Variables



        #endregion

        #region Public Methods

        /// <summary>
        /// Tap or Untap the card.
        /// </summary>
        public void Tap()
        {
            Debug.Log("Tap");
            // TODO: store "tap state" so Tap() can toggle it. Perhaps store cached rotation too.
        }

        #endregion

        #region Unity Methods



        #endregion

        #region Protected Methods



        #endregion

        #region Callbacks



        #endregion

        #region Private Methods



        #endregion

        #region Event Handlers



        #endregion
    }
}