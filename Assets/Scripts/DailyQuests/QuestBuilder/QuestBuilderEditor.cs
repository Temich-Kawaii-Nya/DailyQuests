using UnityEditor;
using UnityEngine;

namespace DailyQuests.Core.Editor
{
    internal class QuestBuilderEditor : EditorWindow
    {
        private int _conditionIndex;
        private string _questName = string.Empty;
        private string _questDescription = string.Empty;
        private string _questProgress = string.Empty;
        private bool _createNewQuest = false;
        private bool _showConditionPopup = false;

        private ConditionFinder _conditionFinder;
        private QuestBuilder _questBuilder;

        [MenuItem("Tools/Daily Quests/Quest Builder")]
        public static void ShowWindow()
        {
            GetWindow<QuestBuilderEditor>("Quest Builder");
        }

        public void Awake()
        {
            _conditionFinder = new ConditionFinder();
            _questBuilder = new QuestBuilder();
        }

        private void OnGUI()
        {
            GUILayout.Label("Quest Builder", EditorStyles.boldLabel);
            
            if (!_createNewQuest)
            {
                if (GUILayout.Button("Create New Quest"))
                {
                    _createNewQuest = true;
                }
                return;
            }

            _questName = EditorGUILayout.TextField("Quest Name", _questName);
            _questDescription = EditorGUILayout.TextField("Quest Description", _questDescription);
            _questProgress = EditorGUILayout.TextField("Quest Progress", _questProgress);

            if (GUILayout.Button("Add Condition"))
            {
                _showConditionPopup = true;
            }

            if (_showConditionPopup && _conditionFinder.ConditionToStringArray().Length > 0)
            {
                _conditionIndex = EditorGUILayout.Popup("Select Condition", _conditionIndex, _conditionFinder.ConditionToStringArray());
                GUILayout.Label($"Selected Condition: {_conditionFinder.ConditionToStringArray()[_conditionIndex]}");
            }

        }
    }
}
