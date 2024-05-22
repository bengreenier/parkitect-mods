using System;
using System.Linq;
using UnityEngine;

namespace Undo
{
    public class UndoActivationBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            InputManager.Instance.registerKeyGroup(Constants.KEY_GROUP);
            InputManager.Instance.registerKeyMapping(Constants.MODIFIER_KEY);
            InputManager.Instance.registerKeyMapping(Constants.UNDO_KEY);
        }

        private void OnDestroy()
        {
            InputManager.Instance.unregisterKeyMapping(Constants.UNDO_KEY);
            InputManager.Instance.unregisterKeyMapping(Constants.MODIFIER_KEY);
            InputManager.Instance.unregisterKeyGroup(Constants.KEY_GROUP);
        }

        private void Update()
        {
            // undo
            if (InputManager.getKey(Constants.MODIFIER_KEY.keyIdentifier) &&
                InputManager.getKeyDown(Constants.UNDO_KEY.keyIdentifier))
            {
                if (UndoStateManagerSingleton.Instance.UndoStack.Count > 0)
                {
                    UndoMod.Logger.Log("Undo has something to act on");
                    var objects = UndoStateManagerSingleton.Instance.UndoStack.Pop();

                    UndoMod.Logger.Log($"Undo is removing {string.Join(",", objects.Select(o => o.name))}");

                    DestructManager.Instance.destruct(objects);
                }

            }
        }
    }
}