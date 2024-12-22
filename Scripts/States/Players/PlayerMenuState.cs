using System.Collections.Generic;
using UnityEngine;
using GlobalClasses;

public class PlayerMenuState : IState
{
     // UI References
     public GameObject _InventoryUIPrefab;
     public GameObject _QuestMenuUIPrefab;

     // Global Data References
     public Inventory gInventory;
     public List<Quest> gQuests;

     // StateStack
     public CharacterController2D _controller;
     private StateStack _stateStack;

     // Sub statemachine
     private StateMachine stateMachine;
     private InventoryState inventoryState;
     private QuestMenuState questMenuState;

     // Current menu type
     public enum MenuType { None, Inventory, QuestMenu }
     public MenuType currentMenu = MenuType.None;

     // bools
     public bool isToggled;

     public PlayerMenuState(CharacterController2D controller, GameObject InventoryUIPrefab, GameObject QuestMenuUIPrefab, StateStack stateStack)
     {
          _controller = controller;
          _stateStack = stateStack;
          _InventoryUIPrefab = InventoryUIPrefab;
          _QuestMenuUIPrefab = QuestMenuUIPrefab;
          gInventory = Global.Instance.gInventory;
          gQuests = Global.Instance.gQuests;
     }

     public void OnEnter()
     {
          // Open the default menu (Inventory or Quest)
          stateMachine = new StateMachine();
          inventoryState = new InventoryState(this);
          questMenuState = new QuestMenuState(this);
          ToggleMenu(MenuType.Inventory);
     }

     public void OnUpdate()
     {
          // Handle input for closing the menu (e.g., pressing Escape or the "ToggleMenu" button)
          if (_controller.PlayerControls.ContainsKey("ToggleMenu") && Input.GetKeyDown(_controller.PlayerControls["ToggleMenu"]))
          {
               OnExit();
          }

          // Update the logic for the open menu
          if (currentMenu == MenuType.Inventory)
          {
               // Update inventory UI and handle inventory-related inputs
               stateMachine.Update();

               // Logic for switching between menus
               if (_controller.PlayerControls.ContainsKey("SwitchMenuLeft") && Input.GetKeyDown(_controller.PlayerControls["SwitchMenuLeft"]))
               {
                    ToggleMenu(MenuType.QuestMenu);
               }
          }
          else if (currentMenu == MenuType.QuestMenu)
          {
               // Update quest menu UI and handle quest-related inputs
               // Logic for switching between menus
               if (_controller.PlayerControls.ContainsKey("SwitchMenuRight") && Input.GetKeyDown(_controller.PlayerControls["SwitchMenuRight"]))
               {
                    ToggleMenu(MenuType.Inventory);
               }
          }

     }

     public void OnExit()
     {
          // Close the active menu
          CloseAllMenus();

          // Transition back to the previous state (pops the PlayerMenuState from the state stack)
          _stateStack.Pop();
     }

     private void ToggleMenu(MenuType menu)
     {
          // Close any open menu before opening the new one
          CloseAllMenus();

          switch (menu)
          {
               case MenuType.Inventory:
                    _InventoryUIPrefab.SetActive(true);
                    currentMenu = MenuType.Inventory;
                    stateMachine.ChangeState(inventoryState);
                    break;

               case MenuType.QuestMenu:
                    _QuestMenuUIPrefab.SetActive(true);
                    currentMenu = MenuType.QuestMenu;
                    stateMachine.ChangeState(questMenuState);
                    break;
          }
     }

     private void CloseAllMenus()
     {
          stateMachine.PopState();
          currentMenu = MenuType.None;
     }
}