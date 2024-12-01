using System.Collections.Generic;

namespace DailyQuests
{
    public class DefaultDailyQuest : DailyQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Progress { get; set; }
        public List<QuestCondition> Conditions { get; set; }
    }
}
