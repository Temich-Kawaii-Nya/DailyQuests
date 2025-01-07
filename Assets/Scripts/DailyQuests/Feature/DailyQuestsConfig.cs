using UnityEngine;

namespace DailyQuests
{
    public class DailyQuestsConfig : ScriptableObject
    {
        public DataContextType RepositoryType { get; private set; } = DataContextType.PlayerPrefs;
        public string ServerUrl { get; private set; } = "http://localhost:8080";
        public string GetQuestEndPoint { get; private set; } = "/quests";
    }
}