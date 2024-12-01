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
        public void BuildCondition(QuestCondition condition)
        {
            DailyQuest.Conditions.Add(condition);
        }
        public DailyQuest BuildQuest()
        {
            return DailyQuest;
        }
    }
}
