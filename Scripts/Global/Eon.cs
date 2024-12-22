using System;

namespace GlobalClasses
{
    [System.Serializable]
    public class Eon
    {
        public string Name;
        public string CallbackFunction;

        public Eon(string name, string callbackFunction)
        {
            Name = name;
            CallbackFunction = callbackFunction;
        }
    }
}