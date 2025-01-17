using Rewards;
using System;
using System.Collections.Generic;

namespace DailyQuests.Infrasructure.Contracts
{
    [Serializable]
    public class DailyQuestBase : IDailyQuest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Progress { get; set; }
        public bool IsActive { get; set; }
        public bool IsComplited { get; set; }
        public Dictionary<Type, List<IQuestCondition>> Conditions { get; set; } = new();
        public Dictionary<Type, List<Reward>> RewardsList { get; set; } = new();
        public Guid Id { get; set; }
    }

}