using System;

namespace GlobalClasses
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}