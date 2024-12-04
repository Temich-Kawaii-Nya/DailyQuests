using DailyQuests.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DailyQuests
{
    public class DailyQuestSystem : MonoBehaviour
    {
        public DailyQuestSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DailyQuestSystem();
                return _instance;
            }
        }

        public List<DailyQuest> DailyQuests { get; private set; }

        private static DailyQuestSystem _instance;

        private DailyQuestService _dailyQuestService;

        private DailyQuestsConfig _config;

        private void Awake()
        {
            _config = new();
            _dailyQuestService = new(_config);
        }
        public void LoadAllQuests()
        {
            StartCoroutine(LoadQuestsAsync());
        }
        private IEnumerator<Task<bool>> LoadQuestsAsync()
        {
            yield return _dailyQuestService.LoadAllQuestsFromRepository(list => DailyQuests = list);
        }
        
    }
    internal sealed class DailyQuestService
    {
        private readonly DailyQuestsConfig _config;
        internal DailyQuestService(
            DailyQuestsConfig config
            )
        {
            _config = config;
        }
        public async Task<bool> LoadAllQuestsFromRepository(Action<List<DailyQuest>> callback = null)
        {
            var repository = CreateRepository();
            var quests = new List<DailyQuest>();
            if (await repository.GetDailyQuests(callback))
            {
                return true;
            }
            return false;
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