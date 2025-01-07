using DailyQuests.Infrasructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    internal sealed class DailyQuestService
    {
        private readonly IQuestRepository _questRepository;
        private readonly DailyQuestsConfig _config;
        internal DailyQuestService(
            DailyQuestsConfig config
            )
        {
            _config = config;
            _questRepository = new QuestRepository(_config);
        }
        public async void SetQuestActive(Guid id)
        {
            var quest = await _questRepository.GetQuestById(id);
            quest.IsActive = true;
        }
        public async void DeactivateQuest(Guid id)
        {
            var quest = await _questRepository.GetQuestById(id);
            quest.IsActive = false;
        }
        public async Task<List<IDailyQuest>> GetAllQuests()
        {
            var quests = await _questRepository.GetAllQuests();
            return quests.Values.ToList();
        }
        public async Task<bool> GetRandomQuests(int amount, Func<IDailyQuest, object> sortCondition = null, Action<List<IDailyQuest>> callback = null)
        {
            var quests = await _questRepository.GetAllQuests();

            sortCondition ??= _ => Guid.NewGuid();

            amount = Math.Min(amount, quests.Count);

            await Task.Run(() =>
            {
                 callback?.Invoke(quests.Values.OrderBy(sortCondition).Take(amount).ToList());
                return true;
            });
            return false;
        }
        public async Task<bool> UpdateQuestConditionAsync(Type type, QuestConditionParams p = null)
        {
            await Task.Run(async () =>
            {
                var quests = await _questRepository.GetAllQuests();
                foreach (var quest in quests.Values)
                {
                    if (!quest.IsActive)
                    {
                        continue;
                    }
                    if (!quest.Conditions.ContainsKey(type)) 
                    {
                        continue;
                    }
                    foreach (var con in quest.Conditions[type])
                    { 
                        con.UpdateCondition(p);
                    }
                }
            });
            return true;
        }
    }
}