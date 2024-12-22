using System.Collections.Generic;
using System;

namespace GlobalClasses
{
    public class StateStack
    {
        private Stack<IState> states;

        public StateStack()
        {
            states = new Stack<IState>();
        }

        // Push state into the stack, but handle exclusive states
        // Push state into the stack, but handle exclusive states
        public void Push(IState newState)
        {
            // Check if the state is exclusive and if any exclusive state is already in the stack
            if (IsExclusiveState(newState) && ContainsExclusiveState())
            {
                return; // Don't push the state if there's already an exclusive state
            }

            // Push the new state onto the stack
            states.Push(newState);

            // Call OnEnter for the new state
            newState.OnEnter();
        }

        public void Pop()
        {
            if (states.Count > 0)
            {
                var state = states.Pop();
            }
        }

        // Peek at the top state without removing it
        public IState Peek()
        {
            return states.Count > 0 ? states.Peek() : null;
        }

        // Check if there's any exclusive state in the stack
        public bool ContainsExclusiveState()
        {
            foreach (var state in states)
            {
                if (IsExclusiveState(state))
                {
                    return true;
                }
            }
            return false;
        }

        // Check if a state is considered exclusive (NPC interactions, warping, menu)
        private bool IsExclusiveState(IState state)
        {
            return state is PlayerNpcInteractionState || state is PlayerWarpingState || state is PlayerMenuState;
        }

        // Get all states in the stack (useful for updating)
        public List<IState> GetStates()
        {
            return new List<IState>(states);
        }
    }
}
