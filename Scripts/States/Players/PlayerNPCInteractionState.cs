using System.Collections.Generic;
using UnityEngine;
using GlobalClasses;
using TMPro;

public class PlayerNpcInteractionState : IState
{
     private CharacterController2D characterController;
     private List<GameObject> responsePanels; // Holds reference to player response UI panels
     private int selectedIndex; // Tracks the currently selected response

     public PlayerNpcInteractionState(CharacterController2D controller)
     {
          characterController = controller;
          responsePanels = new List<GameObject>();
          selectedIndex = 0;
     }

     public void OnEnter()
     {
          Debug.Log($"Player {characterController.PlayerId} has started an NPC interaction.");

          if (characterController.currentNpc != null && characterController.currentNpc.IsPlayerInTalkingRange(out var player) && player == characterController)
          {
               Debug.Log($"Player {characterController.PlayerId} started conversation with NPC {characterController.currentNpc.npcId}");
               characterController.currentNpc.ChangeState(new NpcTalkingState(characterController.currentNpc));

               // Initialize response selection
               HighlightSelectedResponse();
          }
     }

     public void OnExit()
     {
          Debug.Log($"Player {characterController.PlayerId} has finished the NPC interaction.");
     }

     public void OnUpdate()
     {
          HandleInput();
     }

     private void HandleInput()
     {
          if (characterController.PlayerControls.ContainsKey("MenuUp") && Input.GetKeyDown(characterController.PlayerControls["MenuUp"]))
          {
               Debug.Log("Up");
               ChangeSelectedResponse(-1);
          }
          if (characterController.PlayerControls.ContainsKey("MenuDown") && Input.GetKeyDown(characterController.PlayerControls["MenuDown"]))
          {
               Debug.Log("Down");
               ChangeSelectedResponse(1);
          }
          if (characterController.PlayerControls.ContainsKey("ConfirmResponse") && Input.GetKeyDown(characterController.PlayerControls["ConfirmResponse"]))
          {
               Debug.Log("confirmed");
               ConfirmSelection();
          }
     }

     private void ChangeSelectedResponse(int direction)
     {
          if (responsePanels.Count == 0) return;

          Debug.Log("response changed");

          // Remove highlight from the current selection
          UnhighlightResponse(selectedIndex);

          // Update the selected index with wrapping
          selectedIndex = (selectedIndex + direction + responsePanels.Count) % responsePanels.Count;

          // Highlight the new selection
          HighlightSelectedResponse();
     }

     private void HighlightSelectedResponse()
     {
          if (responsePanels.Count > 0 && selectedIndex >= 0 && selectedIndex < responsePanels.Count)
          {
               var textComponent = responsePanels[selectedIndex].GetComponentInChildren<TMP_Text>();
               if (textComponent != null)
               {
                    textComponent.color = Color.yellow; // Highlight the selected response
               }
          }
     }

     private void UnhighlightResponse(int index)
     {
          if (index >= 0 && index < responsePanels.Count)
          {
               var textComponent = responsePanels[index].GetComponentInChildren<TMP_Text>();
               if (textComponent != null)
               {
                    textComponent.color = Color.white; // Remove highlight
               }
          }
     }

     private void ConfirmSelection()
     {
          if (responsePanels.Count == 0 || selectedIndex < 0 || selectedIndex >= responsePanels.Count) return;

          Debug.Log($"Player {characterController.PlayerId} selected response {selectedIndex}.");

          // Safely cast the current state to NpcTalkingState
          if (characterController.currentNpc.stateMachine.CurrentState() is NpcTalkingState npcTalkingState)
          {
               // Retrieve the current dialogue node and selected choice
               var currentNode = npcTalkingState.GetCurrentDialogueNode();
               if (currentNode != null && selectedIndex < currentNode.Choices.Count)
               {
                    var selectedResponse = currentNode.Choices[selectedIndex];
                    npcTalkingState.OnChoiceSelected(selectedResponse);

                    HighlightSelectedResponse();
               }
          }
          else
          {
               Debug.LogWarning("The NPC is not in the NpcTalkingState. Cannot process the player's choice.");
          }
     }

     public void AddResponsePanel(GameObject responsePanel)
     {
          responsePanels.Add(responsePanel);
     }

     public void ClearResponsePanels()
     {
          responsePanels = new List<GameObject>();
     }
}
