using System;
using System.Collections.Generic;

namespace Undo
{
    public class UndoStateManagerSingleton : ScriptableSingleton<UndoStateManagerSingleton>
    {
        // TODO(bengreenier): could abstract this further
        public Stack<IList<BuildableObject>> UndoStack = new Stack<IList<BuildableObject>>(Constants.MAX_UNDO_STACK_SIZE);
    }
}