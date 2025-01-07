using System;

namespace DailyQuests.Infrasructure.Contracts
{
    public interface IQuestCondition
    {
        [QuestFieldIgnore]
        public Type ConditionType { get; }
        [QuestFieldIgnore]
        public bool IsComplited { get; }
        public void UpdateCondition(QuestConditionParams par = null);
    }
    public abstract class QuestConditionParams
    {

    }
}
