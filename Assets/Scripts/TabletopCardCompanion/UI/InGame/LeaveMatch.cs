using System;
using UnityEngine;

namespace TabletopCardCompanion.UI.InGame
{
    public class LeaveMatch : MonoBehaviour
    {
        public static event EventHandler<EventArgs> ClickedLeaveMatch;

        public void ClickLeaveMatch()
        {
            ClickedLeaveMatch?.Invoke(this, EventArgs.Empty);
        }
    }
}