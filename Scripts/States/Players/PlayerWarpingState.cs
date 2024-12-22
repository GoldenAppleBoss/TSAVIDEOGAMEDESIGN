using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using GlobalClasses;
public class PlayerWarpingState : IState
{
     private CharacterController2D _controller;
     private GameObject _currentPortkey;
     private StateStack _stateStack; // Reference to the state stack

     public PlayerWarpingState(CharacterController2D controller, GameObject currentPortkey, StateStack stateStack)
     {
          _controller = controller;
          _currentPortkey = currentPortkey;
          _stateStack = stateStack;
     }

     public void OnEnter()
     {
          Debug.Log("Entering WarpingState.");
          var timelineShifting = _currentPortkey?.GetComponent<TimelineShifting>();
          if (timelineShifting != null)
          {
               // Immediately warp to the endpoint
               WarpToEndpoint(timelineShifting.endPoint, timelineShifting.spawnConfig.offsetX, timelineShifting.spawnConfig.offsetY);
          }
          else
          {
               Debug.LogWarning("No valid endpoint found on the Portkey.");
          }

          // Call OnExit immediately after the warp
          OnExit();
     }

     public void OnUpdate()
     {
          // No updates required for immediate warping
     }

     public void OnExit()
     {
          Debug.Log("Exiting WarpingState.");
          // Pop this state from the stack
          _stateStack.Pop();
     }

     private void WarpToEndpoint(GameObject endpoint, float offsetX, float offsetY)
     {
          if (endpoint == null)
          {
               Debug.LogWarning("Endpoint is null. Cannot warp.");
               return;
          }

          Vector3 endpointPosition = endpoint.transform.position;
          Vector3 newPosition = new Vector3(
              endpointPosition.x + offsetX,
              endpointPosition.y + offsetY,
              endpointPosition.z
          );

          // Immediately move the player to the new position
          _controller.transform.position = newPosition;

          // Ensure the camera follows the player right away
          AlignCamerasWithTarget();
     }

     private void AlignCamerasWithTarget()
     {
          foreach (var cam in _controller.cinemachineCams)
          {
               if (cam.Follow == _controller.transform)
               {
                    cam.ForceCameraPosition(_controller.transform.position, _controller.transform.rotation);
               }
          }
     }
}
