using DailyQuests.Infrasructure.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    public interface IQuestRepository
    {
        public Task<Dictionary<Guid, IDailyQuest>> GetAllQuests();
        public Task SaveAllQuests();
        public Task<bool> LoadAllQuestsFromContext();
        public Task<IDailyQuest> GetQuestById(Guid id);
    }
    internal sealed class QuestRepository : IQuestRepository
    {
        private readonly DailyQuestsConfig _cfg;
        private Dictionary<Guid, IDailyQuest> _quests;
        public QuestRepository(
            DailyQuestsConfig cfg
            ) 
        {
            _cfg = cfg;
        }
        public async Task<Dictionary<Guid, IDailyQuest>> GetAllQuests()
        {
            if (_quests == null)
                await LoadAllQuestsFromContext();

            return _quests;
        }

        public async Task<IDailyQuest> GetQuestById(Guid id)
        {
            if (_quests == null)
                await LoadAllQuestsFromContext();
            return _quests[id];
        }

        public async Task<bool> LoadAllQuestsFromContext()
        {
            _quests ??= new();
            _quests.Clear();

            var repository = CreateDataContext();
            if (await repository.GetDailyQuests(list =>
            {
                foreach (var item in list)
                {
                    _quests.Add(item.Id, item);
                }
            }))
            {
                return true;
            }
            return false;
        }

        public Task SaveAllQuests()
        {
            throw new NotImplementedException();
        }
        private IDataContext CreateDataContext()
        {
            switch (_cfg.RepositoryType)
            {
                case DataContextType.PlayerPrefs:
                    return new PlayerPrefsDataContext();
                case DataContextType.Server:
                    return new ServerDataContext(_cfg);
                case DataContextType.JsonFile:
                    return new PlayerPrefsDataContext();
                default:
                    return new PlayerPrefsDataContext();
            }
        }
    }
}
