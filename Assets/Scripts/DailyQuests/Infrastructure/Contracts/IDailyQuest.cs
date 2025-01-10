using Rewards;
using System;
using System.Collections.Generic;

namespace DailyQuests.Infrasructure.Contracts
{
    public interface IDailyQuest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Progress { get; set; }
        public bool IsActive { get; set; }
        public bool IsComplited { get; set; }
        public Dictionary<Type, List<IQuestCondition>> Conditions { get; set; }
        public Dictionary<Type, List<Reward>> RewardsList { get; set; }
    }
}
