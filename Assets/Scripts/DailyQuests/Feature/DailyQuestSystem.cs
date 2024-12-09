using DailyQuests.Feature;
using DailyQuests.Feature.Core;
using DailyQuests.Infrasructure.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DailyQuests
{
    public class DailyQuestSystem : MonoBehaviour
    {
        public List<IDailyQuest> DailyQuests { get; private set; }

        private DailyQuestService _dailyQuestService;

        private DailyQuestsConfig _config;
        private void Awake()
        {
            _config = new();
            _dailyQuestService = new(_config);
        }
        public async Task<List<IDailyQuest>> LoadQuestsAsync()
        {
            await _dailyQuestService.LoadAllQuestsFromRepository(list => DailyQuests = list);
            return DailyQuests;
        }
        public async Task UpdateQuest(Type type) 
        {
            Debug.Log("dfsdfs");
            await _dailyQuestService.UpdateQuestConditionAsync(type);
        }
        
    }
    internal sealed class DailyQuestService
    {
        private List<IDailyQuest> _dailyQuests;
        private readonly DailyQuestsConfig _config;
        internal DailyQuestService(
            DailyQuestsConfig config
            )
        {
            _config = config;
            _dailyQuests = new List<IDailyQuest>();
        }
        public async Task<bool> LoadAllQuestsFromRepository(Action<List<IDailyQuest>> callback = null)
        {
            var repository = CreateRepository();
            if (await repository.GetDailyQuests(list =>
            {
                callback.Invoke(list);
                _dailyQuests = list;
            }))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateQuestConditionAsync(Type type, QuestConditionParams p = null)
        {
            if (_dailyQuests == null)
                await LoadAllQuestsFromRepository();

            await Task.Run(() =>
            {
                foreach (var quest in _dailyQuests)
                {
                    //if (!quest.IsActive)
                    //{
                    //    continue;
                    //}
                    if (!quest.Conditions.ContainsKey(type)) 
                    {
                        continue;
                    }
                    Debug.Log("ddf");
                    foreach (var con in quest.Conditions[type])
                    { 
                        con.UpdateCondition(p);
                    }
                }
            });
            return true;
        }
        private IRepository CreateRepository()
        {
            switch (_config.RepositoryType)
            {
                case RepositoryType.PlayerPrefs:
                    return new PlayerPrefsRepository();
                case RepositoryType.Server:
                    return new PlayerPrefsRepository();
                case RepositoryType.JsonFile:
                    return new PlayerPrefsRepository();
                default:
                    return new PlayerPrefsRepository();
            }
        }
    }
    public class DailyQuestsConfig
    {
        public RepositoryType RepositoryType { get; private set; } = RepositoryType.PlayerPrefs;
    }
    public enum RepositoryType
    {
        PlayerPrefs,
        Server,
        JsonFile
    }
}