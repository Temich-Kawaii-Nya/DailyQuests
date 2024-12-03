using System;
using System.Collections.Generic;

namespace DailyQuests
{
    public interface DailyQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Progress { get; set; }
        public Dictionary<Type, List<IQuestCondition>> Conditions { get; set; }
    }

}
