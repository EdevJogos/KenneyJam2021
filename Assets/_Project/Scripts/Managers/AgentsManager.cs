using UnityEngine;
using static PoolDatabase;

public class AgentsManager : MonoBehaviour
{
    public static Player Player;
    public static Vector2 PlayerPosition { get { return Player.transform.position; } }

    public Player player;

    public void Initiate()
    {
        Player = player;
    }

    private void Start()
    {
        CreatePool(Prefabs.LASER, 10);
    }

    public void Restart()
    {
        player.Restart();
    }
}
