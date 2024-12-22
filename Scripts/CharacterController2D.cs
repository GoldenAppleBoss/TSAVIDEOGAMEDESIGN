using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using GlobalClasses;

[System.Serializable]
public class KeyMapping
{
    public string ActionName;
    public KeyCode Key;
}

public class CharacterController2D : MonoBehaviour
{
    // Player Refs
    public int PlayerId;
    private Player gPlayer;

    // Components
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_Transform;

    // Movement
    public float Speed = 5f;
    public float JumpForce = 300f;
    public bool isGrounded = true;
    private bool canWarp = false;
    public GameObject currentPortkey;

    // Key Mapping
    public List<KeyMapping> KeyMappings;
    public Dictionary<string, KeyCode> PlayerControls;

    // Relics, Echos, Eons
    public List<Echo> Echos = new List<Echo>();  // Use Echo
    public List<Eon> Eons = new List<Eon>();  // Use Eon
    public Spell CurrentSpell;
    public int currentEonIndex = 0;
    public int currentEchoIndex = 0;

    // Menus
    public GameObject MenuUI;
    public GameObject InventoryUIPrefab;
    public GameObject QuestMenuUIPrefab;

    // Camera
    public CinemachineVirtualCamera[] cinemachineCams;

    // Reference to the NPC
    public NpcController currentNpc;
    private bool canTalkToCurrentNPC = false;

    // State Stack
    private StateStack stateStack;
    private PlayerNpcInteractionState playerNpcInteractionState;
    private PlayerEchosAndEonsState playerEchosAndEonsState;
    private PlayerMenuState playerMenuState;
    private PlayerMovementState playerMovementState;

    void Start()
    {
        // Initialize gPlayer using PlayerId and search from Instance.Players
        gPlayer = Global.Instance.gPlayers.Find(player => player.Id == PlayerId.ToString());

        // If gPlayer is null, log a warning message
        if (gPlayer == null)
        {
            Debug.LogWarning("Player with ID " + PlayerId + " not found in Global Players.");
            return;
        }

        // Get echos and eons of player
        Echos = gPlayer.Echos;
        Eons = gPlayer.Eons;

        // Get refs
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Transform = GetComponent<Transform>();

        // Initialize KeyMappings to Dictionary
        PlayerControls = new Dictionary<string, KeyCode>();
        foreach (var mapping in KeyMappings)
        {
            if (!PlayerControls.ContainsKey(mapping.ActionName))
            {
                PlayerControls.Add(mapping.ActionName, mapping.Key);
            }
        }

        // Cache Cinemachine cameras for performance
        cinemachineCams = FindObjectsOfType<CinemachineVirtualCamera>();

        // Create the state stack
        stateStack = new StateStack();

        // Initialize and add the basic states
        playerMovementState = new PlayerMovementState(this);
        playerNpcInteractionState = new PlayerNpcInteractionState(this);
        playerEchosAndEonsState = new PlayerEchosAndEonsState(this, PlayerControls);
        playerMenuState = new PlayerMenuState(this, InventoryUIPrefab, QuestMenuUIPrefab, stateStack);

        // Start with movement and echos&eons states by default
        stateStack.Push(playerMovementState);
        stateStack.Push(playerEchosAndEonsState);

        // Disable UI
        InventoryUIPrefab.SetActive(false);
        QuestMenuUIPrefab.SetActive(false);
    }

    void Update()
    {
        // If we can warp and we hit press the warp button, then push the playerWarpingState
        if (canWarp && PlayerControls.ContainsKey("ShiftTimelines") && Input.GetKeyDown(PlayerControls["ShiftTimelines"]))
        {
            if (!stateStack.ContainsExclusiveState()) // Make sure there's no exclusive state
            {
                stateStack.Push(new PlayerWarpingState(this, currentPortkey, stateStack));
                return; // Avoid further updates if the state was pushed
            }
        }

        // If we press the Menu button, then push the playerMenuState
        if (PlayerControls.ContainsKey("ToggleMenu") && Input.GetKeyDown(PlayerControls["ToggleMenu"]))
        {
            if (!stateStack.ContainsExclusiveState()) // Make sure there's no exclusive state
            {
                stateStack.Push(playerMenuState);
                return; // Avoid further updates if the state was pushed
            }
        }

        // If we can talk to an NPC and press the button to talk to the NPC, then push the PlayerNPCInteractionsState
        if (canTalkToCurrentNPC && stateStack.Peek() != playerNpcInteractionState && PlayerControls.ContainsKey("TalkToNPC") && Input.GetKeyDown(PlayerControls["TalkToNPC"]))
        {
            stateStack.Push(playerNpcInteractionState);
        }

        // Update the states in the stack
        bool exclusiveStateActive = stateStack.Peek() is PlayerNpcInteractionState ||
                                    stateStack.Peek() is PlayerWarpingState ||
                                    stateStack.Peek() is PlayerMenuState;

        // Loop through the states in the stack and update them
        foreach (var state in stateStack.GetStates())
        {
            // If there's an exclusive state at the top, only update that state
            if (exclusiveStateActive && state == stateStack.Peek())
            {
                state.OnUpdate();
                break;  // Exit after updating the exclusive state
            }

            // Otherwise, update all non-exclusive states
            if (!exclusiveStateActive)
            {
                state.OnUpdate();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portkey"))
        {
            var timelineShifting = collision.GetComponent<TimelineShifting>();
            if (timelineShifting != null)
            {
                canWarp = true;
                currentPortkey = collision.gameObject;
            }
            else
            {
                Debug.LogWarning("TimelineShifting component not found on Portkey object.");
            }
        }
        else if (collision.CompareTag("NPCSightRadius"))
        {
            var npcController = collision.GetComponentInParent<NpcController>();
            if (npcController != null)
            {
                currentNpc = npcController;
                currentNpc.SetInteractingPlayer(this);
                Debug.Log($"Player {PlayerId} entered NPC {npcController.npcId}'s sight radius.");
            }
        }
        else if (collision.CompareTag("NPCTalkingRadius"))
        {
            var npcController = collision.GetComponentInParent<NpcController>();
            if (npcController != null && currentNpc != null)
            {
                canTalkToCurrentNPC = true;
                Debug.Log("Player can now talk to NPC.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portkey"))
        {
            canWarp = false;
            currentPortkey = null;
        }
        if (collision.CompareTag("NPCTalkingRadius"))
        {
            canTalkToCurrentNPC = false;
            Debug.Log("Player is no longer in the talking radius of NPC.");
        }
    }

    public IState GetCurrentState()
    {
        return stateStack.Peek();
    }
}