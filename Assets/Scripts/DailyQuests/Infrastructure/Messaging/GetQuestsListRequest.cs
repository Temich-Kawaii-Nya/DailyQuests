using DailyQuests.Infrasructure.Contracts;
using GameCore.EventBus.Messaging;
using System.Collections.Generic;
namespace DailyQuests.Infrasructure.Messaging
{
    public class GetQuestsListRequest : IRequest
    {

    }
    public class GetAllQuestsResponse : IResponse 
    {
        public readonly List<IDailyQuest> quests;

        public GetAllQuestsResponse(List<IDailyQuest> list)
        {
            quests = list;
        }
    }
}
