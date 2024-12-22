using UnityEngine;
using GlobalClasses;
using TMPro;
using System.Collections.Generic;

public class NpcTalkingState : IState
{
    private NpcController npcController;
    private DialogueNode currentNode;
    private GameObject currentDialogueUI;

    public NpcTalkingState(NpcController npcController)
    {
        this.npcController = npcController;
    }

    public void OnEnter()
    {
        Debug.Log("NPC has entered Talking State.");
        PlayTalkingAnimation();
        InitializeDialogue();
    }

    public void OnExit()
    {
        Debug.Log("NPC is exiting Talking State.");
        HideDialogueUI();
        StopTalkingAnimation();
    }

    public void OnUpdate()
    {
        CharacterController2D player;
        if (!npcController.IsPlayerInTalkingRange(out player))
        {
            npcController.ChangeState(new NpcIdleState(npcController));
        }
    }

    private void InitializeDialogue()
    {
        var npcDialogue = npcController.npcDialogueData;
        if (npcDialogue != null && npcDialogue.DialogueLines.Count > 0)
        {
            currentNode = npcDialogue.DialogueLines[0]; // Start with the first dialogue node
            ShowCurrentDialogue();
        }
    }

    private void ShowCurrentDialogue()
    {
        if (currentNode == null)
        {
            Debug.LogWarning("Current dialogue node is null.");
            return;
        }

        var interactingPlayer = npcController.GetInteractingPlayer();
        if (interactingPlayer == null)
        {
            Debug.LogWarning("No interacting player found to display dialogue.");
            return;
        }

        int playerId = interactingPlayer.PlayerId;
        var dialogueUI = npcController.GetDialogueUIForPlayer(playerId);
        var dialogueNPCUI = UnityEngine.Object.Instantiate(npcController.npcDialogueUIPrefab); // Instantiate NPC dialogue panel if not found

        // Find TMP_Text in the NPC dialogue UI prefab
        var dialogueNPCText = dialogueNPCUI.GetComponentInChildren<TMP_Text>();
        if (dialogueNPCText != null)
        {
            dialogueNPCText.text = currentNode.Text; // Set NPC text
            Debug.Log($"Displaying dialogue for Player {playerId}: {currentNode.Text}");
        }
        else
        {
            Debug.LogWarning($"No TMP_Text component found in npcDialogueUIPrefab for Player {playerId}.");
        }

        // Now instantiate and position player response panels
        int responseIndex = 0;
        foreach (var choice in currentNode.Choices)
        {
            var responsePanel = UnityEngine.Object.Instantiate(npcController.responsePanelPrefab); // Instantiate response panel

            // Find the TMP_Text for the choice
            var choiceText = responsePanel.GetComponentInChildren<TMP_Text>();
            if (choiceText != null)
            {
                choiceText.text = choice.Text;
            }

            // Position the panels (e.g., to the right of the NPC)
            responsePanel.transform.SetParent(dialogueUI.transform); // Attach to dialogue UI
            responsePanel.transform.localPosition = new Vector3(1000, 100 - responseIndex * 120, 0); // Adjust as needed
            responsePanel.transform.localScale = new Vector3(1, 1, 1); // Adjust as needed

            // Push the responsePanels into the PlayerNPCInteractionState list for responsePanels
            if (interactingPlayer.GetCurrentState() is PlayerNpcInteractionState playerNpcInteractionState)
            {
                playerNpcInteractionState.AddResponsePanel(responsePanel);
            }

            responseIndex++;
        }

        // Position the NPC dialogue panel (goes in the middle)
        dialogueNPCUI.transform.SetParent(dialogueUI.transform);
        dialogueNPCUI.transform.localPosition = new Vector3(0, 0, 0); // Adjust as needed
        dialogueNPCUI.transform.localScale = new Vector3(1, 1, 1);
    }




    public void OnChoiceSelected(DialogueBranch selectedChoice)
    {
        DialogueNode nextNode = FindDialogueNodeById(selectedChoice.NextDialogueId);

        if (nextNode != null)
        {
            currentNode = nextNode;
            ClearDialogueUI();
            ShowCurrentDialogue();
        }
        else
        {
            npcController.ChangeState(new NpcIdleState(npcController)); // End dialogue if no next node
        }
    }

    public DialogueNode GetCurrentDialogueNode()
    {
        return currentNode;
    }


    private DialogueNode FindDialogueNodeById(string id)
    {
        if (npcController.npcDialogueData == null)
            return null;

        foreach (var dialogueNode in npcController.npcDialogueData.DialogueLines)
        {
            if (dialogueNode.Id == id)
            {
                return dialogueNode;
            }
        }

        return null;
    }

    private void HideDialogueUI()
    {
        ClearDialogueUI();
    }

    private void ClearDialogueUI()
    {
        var interactingPlayer = npcController.GetInteractingPlayer();
        if (interactingPlayer != null)
        {
            int playerId = interactingPlayer.PlayerId;
            var dialogueUI = npcController.GetDialogueUIForPlayer(playerId);

            if (dialogueUI != null)
            {
                // Destroy all child objects of the dialogue UI (NPC text + player responses)
                foreach (Transform child in dialogueUI.transform)
                {
                    Object.Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogWarning("No dialogue UI found for Player.");
            }
            
            if (interactingPlayer.GetCurrentState() is PlayerNpcInteractionState playerNpcInteractionState)
            {
                playerNpcInteractionState.ClearResponsePanels();
            }

        }
        else
        {
            Debug.LogWarning("No interacting player found to clear dialogue UI.");
        }
    }

    private void PlayTalkingAnimation()
    {
        Debug.Log("Playing talking animation.");
        var animator = npcController.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
        }
    }

    private void StopTalkingAnimation()
    {
        Debug.Log("Stopping talking animation.");
        var animator = npcController.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
        }
    }
}
