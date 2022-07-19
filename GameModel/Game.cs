﻿

using GodlessBoard.Models;

namespace GodlessBoard.GameModel
{
    public class Game
    {
        public Game(string name, Player owner)
        {
            Name = name;
            Owner = owner;
            Players = new List<Player> { owner };
            Tabs = new List<Board>() { new Board("Main") };

        }
        public string Name { get; set; }
        public Player Owner { get; set; }
        public List<Player> Players { get; set; }
        public List<Board> Tabs { get; set; }
        public List<object>? Medias { get; set; }
        public void Start()
        {

        }

    }

}