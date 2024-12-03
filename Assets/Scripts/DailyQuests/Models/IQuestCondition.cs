using System;

namespace DailyQuests
{
    public interface IQuestCondition
    {
        [QuestFieldIgnore]
        public Type ConditionType { get; }
        [QuestFieldIgnore]
        public bool IsComplited { get; }
    }
    public class QuestFieldIgnoreAttribute : Attribute
    {
    }
}
