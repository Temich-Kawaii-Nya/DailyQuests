using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus.Messaging;

namespace DailyQuests.Feature.Core
{
    internal sealed class UpdateConditionHandler : Handler, IEventReceiver<UpdateQuestConditionEvent>
    {
        public UpdateConditionHandler(DailyQuestService dailyQuestService) : base(dailyQuestService)
        {
        }

        public async void OnEvent(UpdateQuestConditionEvent eventMessage)
        {
            await _dailyQuestService.UpdateQuestConditionAsync(eventMessage.conditionType);
        }
    }
}
