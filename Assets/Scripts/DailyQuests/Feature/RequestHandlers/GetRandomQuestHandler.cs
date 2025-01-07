using DailyQuests.Infrasructure.Contracts;
using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    internal sealed class GetRandomQuestHandler : Handler, IRequestHandler<GetQuestRequest, GetQuestResponse>
    {
        public GetRandomQuestHandler(DailyQuestService dailyQuestService) : base(dailyQuestService)
        {

        }

        public async Task<GetQuestResponse> HandleAsync(GetQuestRequest request)
        { 
            List<IDailyQuest> list = new List<IDailyQuest>();
            
            await _dailyQuestService.GetRandomQuests(request.amount, request.sortCondition, quests => list = quests);

            return new GetQuestResponse(list);
        }
    }
}
