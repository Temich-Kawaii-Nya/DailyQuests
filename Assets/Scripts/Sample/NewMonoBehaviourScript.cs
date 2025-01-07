using DailyQuests;
using DailyQuests.Feature;
using DailyQuests.Infrasructure;
using DailyQuests.Infrasructure.Contracts;
using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private DailyQuestSystem _dailyQuestSystem;

    private EventBus _eventBus;

    private List<IDailyQuest> _quests;

    private void Awake()
    {
        _eventBus = new EventBus();
        _quests = new List<IDailyQuest>();
        _dailyQuestSystem.Construct(_eventBus);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var response = await _eventBus.SendRequest<GetQuestsListRequest, GetAllQuestsResponse>(new GetQuestsListRequest());

            _quests = response.quests;

            foreach (var quest in response.quests)
            {
                Debug.Log($"Quest: {quest.Name}"); 
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _eventBus.TriggerEvent(new UpdateQuestConditionEvent(typeof(CollectItemsCondition)));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            var id = _quests.First().Id;
            _eventBus.TriggerEvent(new QuestStartEvent(id));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var response = await _eventBus.SendRequest<GetQuestRequest, GetQuestResponse>(new GetQuestRequest(3));

            foreach (var quest in response.quests)
            {
                Debug.Log($"Quest: {quest.Name}");
            }
        }
    }
}
