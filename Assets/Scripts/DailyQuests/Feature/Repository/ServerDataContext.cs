using DailyQuests.Infrasructure.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DailyQuests.Feature.Core
{
    internal sealed class ServerDataContext : IDataContext
    {
        private readonly DailyQuestsConfig _cfg;
        public ServerDataContext(
            DailyQuestsConfig cfg
            ) 
        {
            _cfg = cfg;
        }
        public async Task<bool> GetDailyQuests(Action<List<IDailyQuest>> callback = null)
        {
            var url = _cfg.ServerUrl + _cfg.GetQuestEndPoint;
            using (var request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();

                switch (request.result)
                {
                    case UnityWebRequest.Result.InProgress:
                        return false;
                    case UnityWebRequest.Result.Success:
                        var settings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        };
                        callback?.Invoke(JsonConvert.DeserializeObject<List<IDailyQuest>>(request.downloadHandler.text, settings));
                        return true;
                    case UnityWebRequest.Result.ConnectionError:

                        return false;
                    case UnityWebRequest.Result.ProtocolError:
                        return false;
                    case UnityWebRequest.Result.DataProcessingError:
                        return false;
                    default:
                        return false;
                }
            }
        }
        public Task<bool> SaveDailyQuests(List<IDailyQuest> quests)
        {
                return Task.FromResult(true);
        }
    }
}