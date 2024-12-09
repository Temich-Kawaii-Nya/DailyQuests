using DailyQuests;
using DailyQuests.Feature;
using DailyQuests.Infrasructure.Contracts;
using DailyQuests.Infrasructure.Messaging;
using GameCore.EventBus;
using GameCore.EventBus.Messaging;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private DailyQuestSystem _dailyQuestSystem;

    private EventBus _eventBus;
    private GetDailyRequestHandler _handler;

    private void Awake()
    {
        _eventBus = new EventBus();
        _handler = new GetDailyRequestHandler(_dailyQuestSystem);
        _eventBus.RegisterRequestHandler(_handler);
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var list = await _eventBus.SendRequest<GetQuestsListRequest, List<IDailyQuest>>(new GetQuestsListRequest());
            foreach (var quest in list)
            {
                Debug.Log($"Quest: {quest.Name}"); 
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
        }
    }
}
