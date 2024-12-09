using DailyQuests.Infrasructure.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Feature.Core
{
    public interface IRepository
    {
        Task<bool> GetDailyQuests(Action<List<IDailyQuest>> callback = null);
        Task<bool> SaveDailyQuests(List<IDailyQuest> quests);
    }
}

