using DailyQuests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ConditionFinder
{
    public List<Type> Condition;
    public Dictionary<Type, List<FieldInfo>> ConditionFilds;
    public Dictionary<Type, List<PropertyInfo>> ConditionProperties;
    public ConditionFinder()
    {
        Condition = new List<Type>();
        ConditionFilds = new Dictionary<Type, List<FieldInfo>>();
        ConditionProperties = new Dictionary<Type, List<PropertyInfo>>();
    }

    public string[] ConditionToStringArray()
    {
        return Condition.Select(type => type.Name).ToArray();
    }
    [InitializeOnLoadMethod]
    public void Find()
    {
        Debug.Log("Start");
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes()) 
            {
                if (typeof(IQuestCondition).IsAssignableFrom(type) && type != typeof(IQuestCondition))
                {
                    Condition.Add(type);

                    ConditionFilds.Add(type, new List<FieldInfo>());
                    ConditionProperties.Add(type, new List<PropertyInfo>());

                    foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false) ||
                            field.IsDefined(typeof(QuestFieldIgnoreAttribute), false))
                        {
                            continue;
                        }
                        var iCondition = type.GetInterface("IQuestCondition");
                        var interfaceField = iCondition.GetField(field.Name,
                                                        BindingFlags.Public | 
                                                        BindingFlags.NonPublic | 
                                                        BindingFlags.Instance);
                        if (interfaceField != null && interfaceField.IsDefined(typeof(QuestFieldIgnoreAttribute), false))
                        {
                            continue;
                        }

                        ConditionFilds[type].Add(field);
                    }
                    foreach (var field in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.IsDefined(typeof(QuestFieldIgnoreAttribute), true))
                        {
                            continue;
                        }
                        var iCondition = type.GetInterface("IQuestCondition");
                        var interfaceField = iCondition.GetProperty(field.Name,
                                                        BindingFlags.Public |
                                                        BindingFlags.NonPublic |
                                                        BindingFlags.Instance);
                        if (interfaceField != null && interfaceField.IsDefined(typeof(QuestFieldIgnoreAttribute), false))
                        {
                            continue;
                        }
                        ConditionProperties[type].Add(field);
                    }
                }
            }
        }
    }
}
