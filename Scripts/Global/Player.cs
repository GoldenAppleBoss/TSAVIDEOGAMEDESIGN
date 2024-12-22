using System;
using System.Collections.Generic;
using GlobalClasses;

namespace GlobalClasses
{
    [System.Serializable]
    public class Player
    {
        public string Id;
        public List<Echo> Echos = new List<Echo>();
        public List<Eon> Eons = new List<Eon>();
        public int Health;

        public Player(string id, int health)
        {
            Id = id;
            Health = health;
        }

        // Add methods to manage Echos and Eons, if necessary
    }
}