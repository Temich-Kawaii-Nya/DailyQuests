using System;

namespace DailyQuests
{
    public interface QuestCondition
    {
        public Type ConditionType { get; }
        public bool IsComplited { get; }
    }
}
