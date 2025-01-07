using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus.Messaging;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    internal class GetDailyRequestHandler : Handler, IRequestHandler<GetQuestsListRequest, GetAllQuestsResponse>
    {
        public GetDailyRequestHandler(DailyQuestService dailyQuestService) : base(dailyQuestService)
        {
        }
        public async Task<GetAllQuestsResponse> HandleAsync(GetQuestsListRequest request)
        {
            var quests = await _dailyQuestService.GetAllQuests();
            return new GetAllQuestsResponse(quests);
        }
    }
}
