using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    public class Player
    {
        public string Name { get; set; }

        public int Points { get; private set; }

        public Player(string name, int score)
        {
            Name = name;
            Points = score;
        }

        public void AddScore(int score)
        {
            Points += score;
        }
    }
}