using TouchScript.Gestures;
using UnityEngine;

namespace TabletopCardCompanion.Card
{
    /// <summary>
    /// Do something when the card is tapped.
    /// </summary>
    [RequireComponent(typeof(TapGesture))]
    public class CardTapHandler : MonoBehaviour
    {
        private TapGesture tapGesture;

        private void OnEnable()
        {
            tapGesture = GetComponent<TapGesture>();
            tapGesture.Tapped += TappedHandler;
        }

        private void OnDisable()
        {
            tapGesture.Tapped -= TappedHandler;
        }

        private void TappedHandler(object sender, System.EventArgs e)
        {
            Debug.Log("Tapped");
        }
    }
}