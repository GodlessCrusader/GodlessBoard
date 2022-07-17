

namespace GodlessBoard.GameModel
{
    public class Player
    {
        public Player(string name, Role role)
        {
            GameName = name;
            Role = role;
        }

        public string GameName { get; set; }
        public Role Role { get; set; }
        public List<Token>? ControlledTokens { get; set; }
    }
    public enum Role
    {
        Owner = 0,
        GameMaster = 1,
        Player = 2,
        Spectator = 3
    }
}

