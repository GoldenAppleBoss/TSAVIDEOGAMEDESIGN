using UnityEngine;
using GlobalClasses;

public class NpcIdleState : IState
{
    private NpcController npcController;
    private bool isPlayerInSightRange;

    public NpcIdleState(NpcController npcController)
    {
        this.npcController = npcController;
    }

    public void OnEnter()
    {
        Debug.Log("NPC has entered Idle State.");
        PlayIdleAnimation();
        UpdateIndicator(); // Perform initial visibility check
    }

    public void OnExit()
    {
        Debug.Log("NPC is exiting Idle State.");
        StopIdleAnimation();
        HideIndicator(); // Hide the indicator when exiting idle
    }

    public void OnUpdate()
    {
        HandlePlayerVisibility();
    }

    private void HandlePlayerVisibility()
    {
        CharacterController2D player;
        bool playerCurrentlyInSight = npcController.IsPlayerInSightRange(out player);

        if (playerCurrentlyInSight != isPlayerInSightRange)
        {
            isPlayerInSightRange = playerCurrentlyInSight;
            UpdateIndicator();
        }
    }

    private void UpdateIndicator()
    {
        if (isPlayerInSightRange)
        {
            ShowIndicator();
        }
        else
        {
            HideIndicator();
        }
    }

    private void ShowIndicator()
    {
        Debug.Log("Showing interaction indicator.");
        // Instantiate or enable the indicator above the NPC here
    }

    private void HideIndicator()
    {
        Debug.Log("Hiding interaction indicator.");
        // Destroy or disable the indicator here
    }

    private void PlayIdleAnimation()
    {
        Debug.Log("Playing idle animation.");
        // Trigger idle animation using Animator
    }

    private void StopIdleAnimation()
    {
        Debug.Log("Stopping idle animation.");
        // Stop idle animation
    }
}
