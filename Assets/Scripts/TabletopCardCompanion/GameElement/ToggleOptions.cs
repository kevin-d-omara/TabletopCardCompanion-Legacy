namespace TabletopCardCompanion.GameElement
{
    /// <summary>
    /// Collection of boolean switches defining behavior for a game element.
    /// </summary>
    public class ToggleOptions
    {
        /// <summary>
        /// Freeze the object in place, stopping all movement.
        /// </summary>
        public bool Locked { get; protected set; }

        /// <summary>
        /// Should the object snap to any grid points?
        /// </summary>
        public bool Grid { get; set; } = true;

        /// <summary>
        /// Should the object snap to any snap points?
        /// </summary>
        public bool Snap { get; set; } = true;

        /// <summary>
        /// When hovering over, tooltips will appear for this object (Name, Description, Icon).
        /// </summary>
        public bool Tooltip { get; set; } = true;

        /// <summary>
        /// When picked up, objects above this one will be attached to it.
        /// </summary>
        public bool Sticky { get; set; } = true;

        /// <summary>
        /// Should this object go into player hands?
        /// </summary>
        public bool Hands { get; set; }

        /// <summary>
        /// Should the object survive loading between saves and mods?
        /// </summary>
        public bool Persistent { get; set; }

        /// <summary>
        /// Should this object recieve grid lines projected onto it?
        /// </summary>
        public bool GridProjection { get; set; }
    }
}
