using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    public static Player Player;
    public static Vector2 PlayerPosition { get { return Player.transform.position; } }

    public Player player;

    public void Initiate()
    {
        Player = player;
    }

    public void Restart()
    {
        player.Restart();
    }
}
