using System.Collections.Generic;
using UnityEngine;
using GlobalClasses;

public class PlayerEchosAndEonsState : IState
{
     private CharacterController2D characterController;
     private Dictionary<string, KeyCode> controls;

     public PlayerEchosAndEonsState(CharacterController2D characterController, Dictionary<string, KeyCode> playerControls)
     {
          this.characterController = characterController;
          this.controls = playerControls;
     }

     public void OnEnter()
     {
          Debug.Log("Entered Echos and Eons State");
     }

     public void OnExit()
     {
          Debug.Log("Exited Echos and Eons State");
     }

     public void OnUpdate()
     {
          HandleInput();
     }

     private void HandleInput()
     {
          if (characterController.CurrentSpell.Type == "Eon")
          {
               HandleEonInput();
          }
          else if (characterController.CurrentSpell.Type == "Echo")
          {
               HandleEchoInput();
          }

          if (Input.GetKeyDown(GetKey("SetToEon")))
          {
               SwitchToEon();
          }
          else if (Input.GetKeyDown(GetKey("SetToEcho")))
          {
               SwitchToEcho();
          }
     }

     private void HandleEonInput()
     {
          if (Input.GetKeyDown(GetKey("AttackEon")))
          {
               Debug.Log("Eon attack executed.");
          }

          if (Input.GetKeyDown(GetKey("SetToNextEon")))
          {
               characterController.currentEonIndex =
                   (characterController.currentEonIndex + 1) % characterController.Eons.Count;

               var nextEon = characterController.Eons[characterController.currentEonIndex];
               characterController.CurrentSpell = new Spell(nextEon.Name, nextEon.CallbackFunction, "Eon");

               Debug.Log($"Switched to next Eon: {characterController.CurrentSpell.Name}");
          }
     }

     private void HandleEchoInput()
     {
          if (Input.GetKeyDown(GetKey("AttackEcho")))
          {
               Debug.Log("Echo attack executed.");
          }

          if (Input.GetKeyDown(GetKey("SetToNextEcho")))
          {
               characterController.currentEchoIndex =
                   (characterController.currentEchoIndex + 1) % characterController.Echos.Count;

               var nextEcho = characterController.Echos[characterController.currentEchoIndex];
               characterController.CurrentSpell = new Spell(nextEcho.Name, nextEcho.CallbackFunction, "Echo");

               Debug.Log($"Switched to next Echo: {characterController.CurrentSpell.Name}");
          }
     }

     private void SwitchToEon()
     {
          if (characterController.Eons.Count > 0)
          {
               var currentEon = characterController.Eons[characterController.currentEonIndex];
               characterController.CurrentSpell = new Spell(currentEon.Name, currentEon.CallbackFunction, "Eon");

               Debug.Log($"Switched to Eon mode: {characterController.CurrentSpell.Name}");
          }
     }

     private void SwitchToEcho()
     {
          if (characterController.Echos.Count > 0)
          {
               var currentEcho = characterController.Echos[characterController.currentEchoIndex];
               characterController.CurrentSpell = new Spell(currentEcho.Name, currentEcho.CallbackFunction, "Echo");

               Debug.Log($"Switched to Echo mode: {characterController.CurrentSpell.Name}");
          }
     }

     private KeyCode GetKey(string action)
     {
          return controls.ContainsKey(action) ? controls[action] : KeyCode.None;
     }
}
