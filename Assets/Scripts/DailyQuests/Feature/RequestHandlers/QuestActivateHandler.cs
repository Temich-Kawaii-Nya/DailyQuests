using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus.Messaging;

namespace DailyQuests.Feature.Core
{
    internal class QuestActivateHandler : Handler, IEventReceiver<QuestStartEvent>, IEventReceiver<QuestStopEvent>
    {
        public QuestActivateHandler(DailyQuestService dailyQuestService) : base(dailyQuestService)
        {
        }

        public void OnEvent(QuestStartEvent eventMessage)
        {
            _dailyQuestService.SetQuestActive(eventMessage.id);
        }
        public void OnEvent(QuestStopEvent eventMessage)
        {
            _dailyQuestService.DeactivateQuest(eventMessage.id);
        }
    }
}
 