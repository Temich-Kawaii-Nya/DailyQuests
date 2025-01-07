using GameCore.EventBus.Messaging;
using System;

namespace DailyQuests.Infrasructure.Messaging
{
    public struct QuestStartEvent : IEvent
    {
        public Guid id;
        public QuestStartEvent(Guid id) 
        {
            this.id = id;
        }
    }
    public struct QuestStopEvent : IEvent
    {
        public Guid id;
        public QuestStopEvent(Guid id)
        {
            this.id = id;
        }
    }
}
