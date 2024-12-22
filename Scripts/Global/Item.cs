using System;

namespace GlobalClasses
{
    [System.Serializable]
    public class Item
    {
        public string Id;
        public string Name;
        public string Image;
        public string Description;
        public string OnUseCallback;

        public Item(string id, string name, string image, string description, string onUseCallback)
        {
            Id = id;
            Name = name;
            Image = image;
            Description = description;
            OnUseCallback = onUseCallback;
        }
    }
}