using GameCore.EventBus.Messaging;
using System;
namespace DailyQuests.Infrasructure.Messaging
{
    public struct UpdateQuestConditionEvent : IEvent
    {
        public readonly Type conditionType;

        public UpdateQuestConditionEvent(Type conditionType)
        {
            this.conditionType = conditionType;
        }
    }
}
