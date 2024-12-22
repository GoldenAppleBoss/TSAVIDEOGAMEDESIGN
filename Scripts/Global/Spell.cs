using System;

namespace GlobalClasses
{
    [System.Serializable]
    public class Spell
    {
        public string Name;
        public string CallbackFunction;
        public string Type;

        public Spell(string name, string callbackFunction, string type)
        {
            Name = name;
            CallbackFunction = callbackFunction;
            Type = type;
        }
    }
}