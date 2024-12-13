using DailyQuests.Infrasructure.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    internal interface IDataContext
    {
        Task<bool> GetDailyQuests(Action<List<IDailyQuest>> callback = null);
        Task<bool> SaveDailyQuests(List<IDailyQuest> quests);
    }
}

