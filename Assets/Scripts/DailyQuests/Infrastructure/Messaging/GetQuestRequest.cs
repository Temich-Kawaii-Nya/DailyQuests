using DailyQuests.Infrasructure.Contracts;
using GameCore.EventBus.Messaging;
using System;
using System.Collections.Generic;

namespace DailyQuests.Infrasructure.Messaging
{
    public class GetQuestRequest : IRequest
    {
        public int amount;

        public Func<IDailyQuest, object> sortCondition;

        public GetQuestRequest(int amount, Func<IDailyQuest, object> sortCondition = null) 
        {
            this.amount = amount;
            this.sortCondition = sortCondition;
        }
    }
    public class GetQuestResponse : IResponse
    {
        public List<IDailyQuest> quests;
        public GetQuestResponse(List<IDailyQuest> quests)
        {
            this.quests = quests;
        }
    }
}
