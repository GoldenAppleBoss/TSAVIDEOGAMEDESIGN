using UnityEngine;
using System.Collections.Generic;
using GlobalClasses;

[System.Serializable]
public class PlayerDialogueUI
{
    public int playerId;           // Player ID
    public GameObject dialogueUI;  // The player's dialogue UI prefab
}

public class NpcController : MonoBehaviour
{
    // NPC Data
    public int npcId;
    public DialogueNpc npcDialogueData;

    // Trigger Zones
    public GameObject canSeeTrigger;
    public GameObject canTalkTrigger;

    // State Machine
    public StateMachine stateMachine;

    // Player Interaction Tracking
    public List<PlayerDialogueUI> playerDialogueUIs = new List<PlayerDialogueUI>(); // Expose this list in Unity Inspector
    public GameObject npcDialogueUIPrefab; // Prefab for NPC dialogue panel
    public GameObject responsePanelPrefab; // Prefab for player responses

    private CharacterController2D interactingPlayer;

    private void Awake()
    {
        stateMachine = new StateMachine();

        // Ensure prefabs are assigned
        if (npcDialogueUIPrefab == null)
        {
            Debug.LogError("NPC Dialogue UI Prefab is not assigned in the Inspector.");
        }

        if (responsePanelPrefab == null)
        {
            Debug.LogError("Response Panel Prefab is not assigned in the Inspector.");
        }
    }

    private void Start()
    {
        LoadNpcDialogueData();
        ChangeState(new NpcIdleState(this));
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool IsPlayerInTalkingRange(out CharacterController2D player)
    {
        return IsPlayerInRange(canTalkTrigger, out player);
    }

    public bool IsPlayerInSightRange(out CharacterController2D player)
    {
        return IsPlayerInRange(canSeeTrigger, out player);
    }

    private bool IsPlayerInRange(GameObject trigger, out CharacterController2D player)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            trigger.transform.position,
            trigger.GetComponent<CircleCollider2D>().radius * Vector2.one,  // Use radius for CircleCollider2D
            0f
        );

        foreach (var collider in colliders)
        {
            var playerController = collider.GetComponent<CharacterController2D>();
            if (playerController != null)
            {
                player = playerController;
                return true;
            }
        }
        player = null;
        return false;
    }

    public void RegisterPlayerDialogueUI(int playerId, GameObject ui)
    {
        // Register dialogue UI for a player
        PlayerDialogueUI existingUI = playerDialogueUIs.Find(uiItem => uiItem.playerId == playerId);
        if (existingUI == null)
        {
            // If no existing UI found, create a new entry
            playerDialogueUIs.Add(new PlayerDialogueUI { playerId = playerId, dialogueUI = ui });
        }
        else
        {
            // If UI exists, update it
            existingUI.dialogueUI = ui;
        }
    }

    public GameObject GetDialogueUIForPlayer(int playerId)
    {
        var playerUI = playerDialogueUIs.Find(ui => ui.playerId == playerId);
        return playerUI != null ? playerUI.dialogueUI : null;
    }

    public void SetInteractingPlayer(CharacterController2D player)
    {
        interactingPlayer = player;
    }

    public CharacterController2D GetInteractingPlayer()
    {
        return interactingPlayer;
    }

    private void LoadNpcDialogueData()
    {
        if (Global.Instance.npcDialoguesById.TryGetValue(npcId.ToString(), out DialogueNpc dialogue))
        {
            npcDialogueData = dialogue;
        }
        else
        {
            Debug.LogWarning($"No dialogue data found for NPC ID: {npcId}");
        }
    }
}
