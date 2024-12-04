using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyQuests.Core
{
    public interface IRepository
    {
        Task<bool> GetDailyQuests(Action<List<DailyQuest>> callback = null);
        Task<bool> SaveDailyQuests(List<DailyQuest> quests);
    }
}

