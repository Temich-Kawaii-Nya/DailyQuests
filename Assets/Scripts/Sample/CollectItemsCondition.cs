using System;
using Newtonsoft.Json;
using UnityEngine;
using DailyQuests.Infrasructure.Contracts;

[Serializable]
    public class CollectItemsCondition : IQuestCondition
    {     
        public Type ConditionType { get { return GetType(); } }

        public bool IsComplited { get; private set; }
        [JsonProperty]
        private int _targetCount = 0;
        [JsonProperty]
        private int _currentCount;

        public void AddCount(int amount)
        {
            if (_currentCount + amount > _targetCount)
            {
                _currentCount = _targetCount;
            }
            else
            {
                _currentCount += amount;
            }
        }
        public void UpdateCondition(QuestConditionParams par = null)
        {
            if (IsComplited)
            {
                return;
            }
            _currentCount++;
            if (_currentCount >= _targetCount)
            {
                IsComplited = true;
            }
            Debug.Log(_currentCount + "/" + _targetCount);
        }
    }
    [Serializable]
    public class KillEnemyCondition : IQuestCondition
    {
        public Type ConditionType { get; }

        public bool IsComplited { get; set; }

        public void UpdateCondition(QuestConditionParams par = null)
        {
            throw new NotImplementedException();
        }
    }
