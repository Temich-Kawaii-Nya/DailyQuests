using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyQuests.Infrasructure.Contracts;
using Newtonsoft.Json;
using UnityEngine;

namespace DailyQuests.Feature.Core
{
    internal sealed class PlayerPrefsDataContext : IDataContext
    {
        private const string QUESTS_SAVE_KEY = "daily_quests_save_key";
        public async Task<bool> GetDailyQuests(Action<List<IDailyQuest>> callback = null)
        {
            return await Task.Run(() =>
            {
                string json = null;
                UnityEditorMainThreadUtility.Enqueue(() =>
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
                var list = JsonConvert.DeserializeObject<List<IDailyQuest>>(json, settings);
                callback?.Invoke(list);


                return true;
            });
        }
        public async Task<bool> SaveDailyQuests(List<IDailyQuest> quests)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            string serializedQuests = JsonConvert.SerializeObject(quests, Formatting.Indented, settings);

            Debug.Log(serializedQuests);

            await Task.Run(() =>
            {
                UnityEditorMainThreadUtility.Enqueue(() =>
                {
                    PlayerPrefs.SetString(QUESTS_SAVE_KEY, serializedQuests);
                    PlayerPrefs.Save();

                });
            });
            return true;
        }
    }
}
