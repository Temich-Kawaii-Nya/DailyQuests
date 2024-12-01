using System;

namespace DailyQuests
{
    public class CollectItemsCondition : QuestCondition
    {
        public Type ConditionType => GetType();

        public bool IsComplited { get => _targetCount == _currentCount; }

        private int _targetCount;

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
    public class KillEnemyCondition : QuestCondition
    {
        public Type ConditionType => throw new NotImplementedException();

        public bool IsComplited => throw new NotImplementedException();
    }
}
