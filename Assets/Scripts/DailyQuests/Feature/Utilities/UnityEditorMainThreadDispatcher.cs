#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Concurrent;

namespace DailyQuests.Feature.Core
{
    public class UnityEditorMainThreadDispatcher
    {
        private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        public static void Enqueue(Action action)
        {
            _actions.Enqueue(action);
#if UNITY_EDITOR
            EditorApplication.update += ExecuteActionsInEditor;
#endif
        }

#if UNITY_EDITOR
        private static void ExecuteActionsInEditor()
        {
            while (_actions.TryDequeue(out var action))
            {
                action.Invoke();
            }
            EditorApplication.update -= ExecuteActionsInEditor;
        }
#endif
    }
}
