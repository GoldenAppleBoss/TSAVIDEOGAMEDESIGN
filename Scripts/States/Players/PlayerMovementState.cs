using System.Collections.Generic;
using UnityEngine;
using GlobalClasses;
public class PlayerMovementState : IState
{
     private CharacterController2D _controller;
     private Rigidbody2D _rigidbody2D;
     private Transform _transform;
     private float _speed;
     private float _jumpForce;
     private bool _isGrounded;
     private bool _facingRight;

     public PlayerMovementState(CharacterController2D controller)
     {
          _controller = controller;
          _rigidbody2D = controller.GetComponent<Rigidbody2D>();
          _transform = controller.transform;
          _speed = controller.Speed;
          _jumpForce = controller.JumpForce;
          _isGrounded = controller.isGrounded;
          _facingRight = true;
     }

     public void OnEnter()
     {
          Debug.Log("Entered PlayerMovementState.");
     }

     public void OnUpdate()
     {
          HandleMovement();
          _isGrounded = _controller.isGrounded;
     }

     public void OnExit()
     {
          Debug.Log("Exiting PlayerMovementState.");
     }

     private void HandleMovement()
     {
          float moveX = 0f;

          // Horizontal movement
          if (_controller.PlayerControls.ContainsKey("Left") && Input.GetKey(_controller.PlayerControls["Left"]))
          {
               moveX = -1f;
          }
          else if (_controller.PlayerControls.ContainsKey("Right") && Input.GetKey(_controller.PlayerControls["Right"]))
          {
               moveX = 1f;
          }

          _rigidbody2D.velocity = new Vector2(moveX * _speed, _rigidbody2D.velocity.y);

          // Flip the character if needed
          if ((moveX > 0 && !_facingRight) || (moveX < 0 && _facingRight))
          {
               Flip();
          }

          // Jumping
          if (_controller.PlayerControls.ContainsKey("Jump") && Input.GetKeyDown(_controller.PlayerControls["Jump"]) && _isGrounded)
          {
               _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
          }

          // Check for falling
          // if (_rigidbody2D.velocity.y < 0 && !_isGrounded)
          // {
          //      Debug.Log("Player is falling.");
          // }
     }

     private void Flip()
     {
          _facingRight = !_facingRight;
          Vector3 theScale = _transform.localScale;
          theScale.x *= -1;
          _transform.localScale = theScale;
     }
}
