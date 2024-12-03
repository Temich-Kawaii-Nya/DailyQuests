namespace DailyQuests.Core
{
    public class QuestBuilder
    {
        private DailyQuest DailyQuest;

        public QuestBuilder()
        {
            DailyQuest = new DefaultDailyQuest();
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
        public DailyQuest BuildQuest()
        {
            return DailyQuest;
        }
    }
}
