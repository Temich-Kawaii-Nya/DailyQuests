using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Newtonsoft.Json;
using DailyQuests.Infrasructure.Contracts;

namespace DailyQuests.Feature.Core.Editor
{
    internal sealed class QuestBuilderEditor : EditorWindow
    {
        private int _conditionIndex;
        private string _questName = string.Empty;
        private string _questDescription = string.Empty;
        private string _questProgress = string.Empty;
        private bool _createNewQuest = false;
        private bool _showConditionPopup = false;

        private IDataContext _repository;

        private IDailyQuest _editingQuest;

        private List<object> _conditions;
        private List<IDailyQuest> _dailyQuests;

        private Dictionary<string, string> _fieldValues;
        private ConditionFinder _conditionFinder;
        private QuestBuilder _questBuilder;

        private ReorderableList _questList;

        private List<bool> _expendList;

        [MenuItem("Tools/Daily Quests/Quest Builder")]
        public static void ShowWindow()
        {
            GetWindow<QuestBuilderEditor>("Quest Builder");
        }
        private void UpdateList(List<IDailyQuest> newList)
        {
            UnityEditorMainThreadUtility.Enqueue(() => {
                if (newList == null || newList.Count == 0)
                {
                    _dailyQuests.Clear();
                    _questList.list = _dailyQuests;
                    _expendList.Clear();
                    Repaint();
                    return;
                }

                _dailyQuests = newList;
                _questList.list = _dailyQuests;
                _expendList = new List<bool>(_dailyQuests.Count);

                for (int i = 0; i < _dailyQuests.Count; i++)
                {
                    _expendList.Add(false);
                }

                Repaint(); 
            });
        }
        public async void Awake()
        {
            _conditionFinder = new ConditionFinder();
            _questBuilder = new QuestBuilder();
            _fieldValues = new Dictionary<string, string>();
            _expendList = new List<bool>();
            _conditions = new List<object>();
            _dailyQuests = new List<IDailyQuest>();
            _conditionFinder.Find();

            //_repository = new ServerRepository(new DailyQuestsConfig()); // TODO FROM CONFIG
            _repository = new PlayerPrefsDataContext();

            int selectedIndex = 0;
            _questList = new ReorderableList(_dailyQuests, typeof(IDailyQuest), true, true, false, true)
            {
                drawHeaderCallback = rect => { 
                    EditorGUI.LabelField(rect, "Daily Quests");
                    if (_dailyQuests.Count == 0 || selectedIndex < 0 || selectedIndex >= _dailyQuests.Count)
                    {
                        return;
                    }
                    var quest = _dailyQuests[selectedIndex];
                    if (GUI.Button(new Rect(rect.x + rect.width - 105, EditorGUIUtility.singleLineHeight, 50, EditorGUIUtility.singleLineHeight), "Edit"))
                    {
                        _editingQuest = quest;
                        LoadQuestForEditing(quest);
                    }

                    if (GUI.Button(new Rect(rect.x + rect.width - 50, EditorGUIUtility.singleLineHeight, 25, EditorGUIUtility.singleLineHeight), "^"))
                    {
                        _expendList[selectedIndex] = !_expendList[selectedIndex];
                    }
                },
                
                elementHeightCallback = index =>
                {
                    if (_expendList[index])
                    {
                        return EditorGUIUtility.singleLineHeight * 4 + 10;
                    }
                    return EditorGUIUtility.singleLineHeight;
                },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var quest = _dailyQuests[index];
                    rect.y += 2;


                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        $"Name: {quest.Name}");

                    if (_expendList[index])
                    {
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight),
                        $"Description: {quest.Description}");

                        EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width, EditorGUIUtility.singleLineHeight),
                            $"Progress: {quest.Progress}");
                    }
                },
                onSelectCallback = index => 
                {
                    selectedIndex = index.index;
                },
                onReorderCallbackWithDetails = (list, oldIndex, newIndex) =>
                {
                    var firstOld = _expendList[oldIndex];
                    var secondOld = _expendList[newIndex];

                    _expendList[oldIndex] = secondOld;
                    _expendList[newIndex] = firstOld;

                    _repository.SaveDailyQuests(_dailyQuests);
                },
                onRemoveCallback = list =>
                {
                    _expendList.RemoveAt(list.index);
                    _repository.SaveDailyQuests(_dailyQuests);
                }
            };
            await _repository.GetDailyQuests(list =>
            {
                UpdateList(list);
            });
        }

        private void OnGUI()
        {
            GUILayout.Label("Quest Builder", EditorStyles.boldLabel);

            _questList.DoLayoutList();

            if (!_createNewQuest)
            {
                if (GUILayout.Button("Create New Quest"))
                {
                    _createNewQuest = true;
                }
                return;
            }

            // Fields for creating or editing a quest
            _questName = EditorGUILayout.TextField("Quest Name", _questName);
            _questDescription = EditorGUILayout.TextField("Quest Description", _questDescription);
            _questProgress = EditorGUILayout.TextField("Quest Progress", _questProgress);

            if (_conditions.Count > 0)
            {
                GUILayout.Space(10);
                foreach (var item in _conditions)
                {
                    var con = item as IQuestCondition;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(con.ConditionType.Name);
                    if (GUILayout.Button("-", GUILayout.Width(20f), GUILayout.Height(20)))
                    {
                        _conditions.Remove(item);
                        GUILayout.EndHorizontal();
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add Condition"))
            {
                _showConditionPopup = true;
            }

            if (_showConditionPopup && _conditionFinder.ConditionToStringArray().Length > 0)
            {
                _conditionIndex = EditorGUILayout.Popup("Select Condition", _conditionIndex, _conditionFinder.ConditionToStringArray());

                var type = _conditionFinder.Condition[_conditionIndex];

                foreach (var item in _conditionFinder.ConditionFilds[type])
                {
                    if (!_fieldValues.ContainsKey(item.Name))
                    {
                        _fieldValues[item.Name] = string.Empty;
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(item.Name, GUILayout.Width(150));
                    _fieldValues[item.Name] = EditorGUILayout.TextField(_fieldValues[item.Name]);
                    GUILayout.EndHorizontal();
                }
                foreach (var item in _conditionFinder.ConditionProperties[type])
                {
                    if (!_fieldValues.ContainsKey(item.Name))
                    {
                        _fieldValues[item.Name] = string.Empty;
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(item.Name, GUILayout.Width(150));
                    _fieldValues[item.Name] = EditorGUILayout.TextField(_fieldValues[item.Name]);
                    GUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Create Condition"))
                {
                    var condition = Activator.CreateInstance(type);

                    if (condition != null)
                    {
                        foreach (var field in _conditionFinder.ConditionFilds[type])
                        {
                            if (_fieldValues.TryGetValue(field.Name, out string value))
                            {
                                var convertedValue = Convert.ChangeType(value, field.FieldType);
                                field.SetValue(condition, convertedValue);
                            }
                        }
                        foreach (var property in _conditionFinder.ConditionProperties[type])
                        {
                            if (_fieldValues.TryGetValue(property.Name, out string value))
                            {
                                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                property.SetValue(condition, convertedValue);
                            }
                        }

                        _conditions.Add(condition);

                        _fieldValues.Clear();
                        _showConditionPopup = false;
                    }
                }
            }

            if (GUILayout.Button(_editingQuest == null ? "Create Quest" : "Save Quest"))
            {
                if (_editingQuest == null)
                {
                    _questBuilder = new QuestBuilder();
                    _questBuilder.BuildName(_questName);
                    _questBuilder.BuildDescription(_questDescription);
                    _questBuilder.BuildProgress(float.Parse(_questProgress));
                    foreach (var condition in _conditions)
                    {
                        var con = condition as IQuestCondition;
                        _questBuilder.BuildCondition(con);
                    }
                    var newQuest = _questBuilder.BuildQuest();
                    _dailyQuests.Add(newQuest);
                    _expendList.Add(false);

                    _repository.SaveDailyQuests(_dailyQuests);

                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    string serializedQuests = JsonConvert.SerializeObject(_dailyQuests, settings);

                    Debug.Log(serializedQuests);
                }
                else
                {
                    _editingQuest.Name = _questName;
                    _editingQuest.Description = _questDescription;
                    _editingQuest.Progress = float.Parse(_questProgress);
                    _editingQuest = null;

                    _repository.SaveDailyQuests(_dailyQuests);
                }

                _questName = string.Empty;
                _questDescription = string.Empty;
                _questProgress = string.Empty;
                _conditions.Clear();
                _createNewQuest = false;
            }
        }
        private void LoadQuestForEditing(IDailyQuest quest)
        {
            _questName = quest.Name;
            _questDescription = quest.Description;
            _questProgress = quest.Progress.ToString();
            _createNewQuest = true;
        }
    }
}
