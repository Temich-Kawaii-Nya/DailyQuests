using System;
using Unity.Plastic.Newtonsoft.Json;

namespace DailyQuests
{
    [Serializable]
    public class CollectItemsCondition : IQuestCondition
    {     
        public Type ConditionType { get { return GetType(); } }

        public bool IsComplited { get => _targetCount == _currentCount; }
        [JsonProperty]
        private int _targetCount;
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
    }
    [Serializable]
    public class KillEnemyCondition : IQuestCondition
    {
        public Type ConditionType { get; }

        public bool IsComplited { get; set; }
    }
}
