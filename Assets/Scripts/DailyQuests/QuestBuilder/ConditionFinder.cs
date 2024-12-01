using DailyQuests;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ConditionFinder
{
    public List<Type> Condition;
    public ConditionFinder()
    {
        Condition = new List<Type>();
        Find();
    }

    public string[] ConditionToStringArray()
    {
        return Condition.Select(type => type.Name).ToArray();
    }

    public void Find()
    {
        Debug.Log("Start");
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes()) 
            {
                if (typeof(QuestCondition).IsAssignableFrom(type) && type != typeof(QuestCondition))
                {
                    Condition.Add(type);
                    Debug.Log("Find");
                }
            }
        }
    }
}
