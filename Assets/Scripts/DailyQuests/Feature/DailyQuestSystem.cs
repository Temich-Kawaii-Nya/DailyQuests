using DailyQuests.Feature.Core;
using DailyQuests.Infrasructure.Contracts;
using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DailyQuests
{
    public sealed class DailyQuestSystem : MonoBehaviour
    {
        public List<IDailyQuest> DailyQuests { get; private set; }

        private DailyQuestService _dailyQuestService;

        private DailyQuestsConfig _config;

        private GetDailyRequestHandler _getDailyRequestHandler;
        private UpdateConditionHandler _updateConditionHandler;
        private QuestActivateHandler _questActivateHandler;
        private GetRandomQuestHandler _getRandomQuestHandler;

        private EventBus _eventBus;
        private void Awake()
        {
            _config = new();
            _dailyQuestService = new(_config);
            _getDailyRequestHandler = new(_dailyQuestService);
            _updateConditionHandler = new(_dailyQuestService);
            _questActivateHandler = new(_dailyQuestService);
            _getRandomQuestHandler = new(_dailyQuestService);
        }
        public void Construct(
            EventBus eventBus
            )
        {
            _eventBus = eventBus;
            RegisterHandlers();
        }
        private void RegisterHandlers()
        {
            _eventBus.Register(_updateConditionHandler);
            _eventBus.RegisterRequestHandler(_getDailyRequestHandler);
            _eventBus.Register<QuestStartEvent>(_questActivateHandler);
            _eventBus.Register<QuestStopEvent>(_questActivateHandler);
            _eventBus.RegisterRequestHandler(_getRandomQuestHandler);
        }
        public async Task UpdateQuest(Type type) 
        {
            await _dailyQuestService.UpdateQuestConditionAsync(type);
        }
    }
}