using UnityEngine;
using TMPro;
using System.Collections.Generic;
using GlobalClasses;

public class QuestMenuState : IState
{
    private PlayerMenuState stateManager;
    private GameObject questMenuInstance;

    public QuestMenuState(PlayerMenuState stateManager)
    {
        this.stateManager = stateManager;
    }

    public void OnEnter()
    {
        // Instantiate the prefab and setup UI
        if (stateManager._QuestMenuUIPrefab != null)
        {
            questMenuInstance = GameObject.Instantiate(stateManager._QuestMenuUIPrefab, stateManager._controller.MenuUI.transform);
            questMenuInstance.SetActive(true);
        }
        else
        {
            Debug.LogError("_QuestMenuUIPrefab is not assigned in PlayerMenuState!");
            return;
        }

        InitializeQuestMenu();
    }

    public void OnExit()
    {
        if (questMenuInstance != null)
        {
            GameObject.Destroy(questMenuInstance);
            questMenuInstance = null;
        }
    }

    public void OnUpdate()
    {
        // Handle quest menu updates (e.g., navigation or input handling)
    }

    private void InitializeQuestMenu()
    {
        if (questMenuInstance == null)
        {
            Debug.LogError("QuestMenuInstance is null!");
            return;
        }

        Transform questListTransform = questMenuInstance.transform.Find("QuestList");
        if (questListTransform == null)
        {
            Debug.LogError("QuestList GameObject not found in QuestMenuInstance!");
            return;
        }

        // Clear existing quests
        foreach (Transform child in questListTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Populate the quest list
        foreach (var quest in stateManager.gQuests)
        {
            CreateQuestPanel(quest, questListTransform);
        }

        // Optionally view details of the first quest
        if (stateManager.gQuests.Count > 0)
        {
            ViewQuestDetails(stateManager.gQuests[0]);
        }
    }

    private void CreateQuestPanel(Quest quest, Transform parentTransform)
    {
        GameObject questPanel = new GameObject($"Quest_{quest.Id}");
        questPanel.transform.SetParent(parentTransform);
        questPanel.transform.localScale = Vector3.one;

        RectTransform rectTransform = questPanel.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        TextMeshProUGUI questNameText = questPanel.AddComponent<TextMeshProUGUI>();
        questNameText.text = quest.Name;
        questNameText.fontSize = 24;
        questNameText.alignment = TextAlignmentOptions.Center;
        questNameText.color = Color.black;
    }

    private void ViewQuestDetails(Quest quest)
    {
        if (questMenuInstance == null)
        {
            Debug.LogError("QuestMenuInstance is null!");
            return;
        }

        Transform questDetailsTransform = questMenuInstance.transform.Find("QuestDetails");
        if (questDetailsTransform == null)
        {
            Debug.LogError("QuestDetails GameObject not found in QuestMenuInstance!");
            return;
        }

        TextMeshProUGUI questName = questDetailsTransform.Find("QuestName")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI questDescription = questDetailsTransform.Find("QuestDescription")?.GetComponent<TextMeshProUGUI>();

        if (questName == null || questDescription == null)
        {
            Debug.LogError("One or more child components are missing in QuestDetails!");
            return;
        }

        questName.text = quest.Name;
        questDescription.text = quest.Description;
    }
}
