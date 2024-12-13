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
        public void BuildName(string name)
        {
            DailyQuest.Name = name;
        }
        public void BuildDescription(string description)
        {
            DailyQuest.Description = description;
        }
        public void BuildProgress(float progress)
        {
            DailyQuest.Progress = progress;
        }
        public void BuildCondition(IQuestCondition condition)
        {
            if (!DailyQuest.Conditions.ContainsKey(condition.GetType()))
                DailyQuest.Conditions.Add(condition.GetType(), new());
            DailyQuest.Conditions[condition.GetType()].Add(condition);
        }
        public void BuildReward(Reward reward)
        {
            if (!DailyQuest.RewardsList.ContainsKey(reward.GetType()))
                DailyQuest.RewardsList.Add(reward.GetType(), new());
            DailyQuest.RewardsList[reward.GetType()].Add(reward);
        }
        public IDailyQuest BuildQuest()
        {
            DailyQuest.Id = Guid.NewGuid();
            return DailyQuest;
        }
    }
}
