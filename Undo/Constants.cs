using UnityEngine;

namespace Undo
{
    public class Constants
    {
        public const string Title = "Undo";

        public const int MAX_UNDO_STACK_SIZE = 1000;
        
        public static readonly KeyGroup KEY_GROUP = new KeyGroup("com.bengreenier.undo")
        {
            keyGroupName = Title
        };

        public static readonly KeyMapping MODIFIER_KEY  = new KeyMapping("com.bengreenier.undo-modifier",
            Application.platform == RuntimePlatform.OSXPlayer ? KeyCode.LeftCommand : KeyCode.LeftControl,
            KeyCode.None)
        {
            keyGroupIdentifier = KEY_GROUP.keyGroupIdentifier,
            canRebind = true,
            keyDescription = "Modifier key that must be held for activation.",
            keyName = "Modifier"
        };

        public static readonly KeyMapping UNDO_KEY = new KeyMapping("com.bengreenier.undo-undo",
            Application.platform == RuntimePlatform.OSXPlayer ? KeyCode.LeftCommand : KeyCode.LeftControl,
            KeyCode.None)
        {
            keyGroupIdentifier = KEY_GROUP.keyGroupIdentifier,
            canRebind = true,
            keyDescription = "Activation key that triggers undo when pressed.",
            keyName = "Undo"
        };
    }
}