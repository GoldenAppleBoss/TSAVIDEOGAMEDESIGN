using System.Collections.Generic;
using System;

namespace GlobalClasses
{
    public class StateMachine
    {
        private Stack<IState> stateStack;

        public StateMachine()
        {
            stateStack = new Stack<IState>();
        }

        public void ChangeState(IState newState)
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().OnExit();
            }

            stateStack.Push(newState);
            newState.OnEnter();
        }

        public void Update()
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().OnUpdate();
            }
        }

        public void PopState()
        {
            if (stateStack.Count > 0)
            {
                IState currentState = stateStack.Pop();
                currentState.OnExit();
            }
        }

        public IState CurrentState()
        {
            return stateStack.Count > 0 ? stateStack.Peek() : null;
        }
    }
}