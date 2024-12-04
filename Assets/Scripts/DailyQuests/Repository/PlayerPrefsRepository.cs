using DailyQuests.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace DailyQuests
{
    public class PlayerPrefsRepository : IRepository
    {
        private const string QUESTS_SAVE_KEY = "daily_quests_save_key";
        public async Task<bool> GetDailyQuests(Action<List<DailyQuest>> callback = null)
        {
            return await Task.Run(() =>
            {
                string json = null;
                UnityEditorMainThreadDispatcher.Enqueue(() =>
                {
                    json = PlayerPrefs.GetString(QUESTS_SAVE_KEY, "[]");
                    Debug.Log(json);
                });

                while (json == null)
                {
                    Task.Yield(); 
                }
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                var list = JsonConvert.DeserializeObject<List<DailyQuest>>(json, settings);
                callback?.Invoke(list);


                return true;
            });
        }
        public async Task<bool> SaveDailyQuests(List<DailyQuest> quests)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            string serializedQuests = JsonConvert.SerializeObject(quests, Formatting.Indented, settings);

            Debug.Log(serializedQuests);

            await Task.Run(() =>
            {
                UnityEditorMainThreadDispatcher.Enqueue(() =>
                {
                    PlayerPrefs.SetString(QUESTS_SAVE_KEY, serializedQuests);
                    PlayerPrefs.Save();

                });
            });
            return true;
        }
    }
}
