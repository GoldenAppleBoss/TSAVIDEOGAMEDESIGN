using System;

namespace GlobalClasses
{
    [System.Serializable]
    public class Echo
    {
        public string Name;
        public string CallbackFunction;

        public Echo(string name, string callbackFunction)
        {
            Name = name;
            CallbackFunction = callbackFunction;
        }
    }
}
