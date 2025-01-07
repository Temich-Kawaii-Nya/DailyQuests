namespace DailyQuests.Feature.Core
{
    internal abstract class Handler
    {
        protected readonly DailyQuestService _dailyQuestService;
        public Handler(
            DailyQuestService dailyQuestService
            )
        {
            _dailyQuestService= dailyQuestService;
        }
    }
}
