using System;

namespace GlobalClasses
{
    [System.Serializable]
    public class Quest
    {
        public int Id;
        public string Name;
        public string Description;
        public string Location;
        public string Image;

        public Quest(int id, string name, string description, string location, string image)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            Image = image;
        }
    }
}