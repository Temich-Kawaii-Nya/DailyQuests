using DailyQuests.Infrasructure.Contracts;
using Rewards;
using System;

namespace DailyQuests.Feature.Core
{
    internal sealed class QuestBuilder
    {
        private IDailyQuest DailyQuest;
       
        public QuestBuilder()
        {
            DailyQuest = new DailyQuestBase();
        }
        public QuestBuilder BuildName(string name)
        {
            DailyQuest.Name = name;
            return this;
        }
        public QuestBuilder BuildDescription(string description)
        {
            DailyQuest.Description = description;
            return this;
        }
        public QuestBuilder BuildProgress(float progress)
        {
            DailyQuest.Progress = progress;
            return this;
        }
        public QuestBuilder BuildCondition(IQuestCondition condition)
        {
            if (!DailyQuest.Conditions.ContainsKey(condition.GetType()))
                DailyQuest.Conditions.Add(condition.GetType(), new());
            DailyQuest.Conditions[condition.GetType()].Add(condition);
            return this;
        }
        public QuestBuilder BuildReward(Reward reward)
        {
            if (!DailyQuest.RewardsList.ContainsKey(reward.GetType()))
                DailyQuest.RewardsList.Add(reward.GetType(), new());
            DailyQuest.RewardsList[reward.GetType()].Add(reward);
            return this;
        }
        public IDailyQuest BuildQuest()
        {
            DailyQuest.Id = Guid.NewGuid();
            return DailyQuest;
        }
    }
}
