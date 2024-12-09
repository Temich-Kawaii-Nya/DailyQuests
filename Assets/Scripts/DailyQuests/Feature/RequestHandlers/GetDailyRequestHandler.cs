using DailyQuests.Infrasructure.Messaging;
using DailyQuests.Infrasructure.Contracts;
using GameCore.EventBus.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Feature
{
    public class GetDailyRequestHandler : IRequestHandler<GetQuestsListRequest, List<IDailyQuest>>
    {
        private readonly DailyQuestSystem _dailyQuestSystem;

        public GetDailyRequestHandler(
            DailyQuestSystem dailyQuestSystem
            )
        {
            _dailyQuestSystem = dailyQuestSystem;   
        }

        public async Task<List<IDailyQuest>> HandleAsync(GetQuestsListRequest request)
        {
            return await _dailyQuestSystem.LoadQuestsAsync();
        }
    }
}
