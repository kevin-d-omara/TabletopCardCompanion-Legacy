using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;

namespace TabletopCardCompanion.TouchScriptCustom.Gestures
{
    /// <summary>
    /// Simple implementation of <see cref="IGestureDelegate"/> that flatly returns a boolean value
    /// for each of the three methods.
    /// </summary>
    public class BooleanGestureDelegate : IGestureDelegate
    {
        public readonly bool AlwaysReceive;
        public readonly bool AlwaysBegin;
        public readonly bool AlwaysRecognize;

        public BooleanGestureDelegate(
            bool alwaysReceive = true,
            bool alwaysBegin = true,
            bool alwaysRecognize = true)
        {
            AlwaysReceive   = alwaysReceive;
            AlwaysBegin     = alwaysBegin;
            AlwaysRecognize = alwaysRecognize;
        }

        public bool ShouldReceivePointer(Gesture gesture, Pointer pointer)
        {
            return AlwaysReceive;
        }

        public bool ShouldBegin(Gesture gesture)
        {
            return AlwaysBegin;
        }

        public bool ShouldRecognizeSimultaneously(Gesture first, Gesture second)
        {
            return AlwaysBegin;
        }
    }
}