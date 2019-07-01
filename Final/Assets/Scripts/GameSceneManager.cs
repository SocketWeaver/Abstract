using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class GameSceneManager : MonoBehaviour
{
    // SceneSpanwer events
    public void OnSpawnerReady(bool finishedSceneSetup)
    {
        // scene has not been set up. spawn a car for the local player.
        if (!finishedSceneSetup)
        {
            /* 
                assign different spawn points for the players in the room
                This is okay for this tutorial as we only have 2 players in a room and we are not handling host migration.
                To properly assign spawn points, you should use roomPropertyAgent or custom room data.
            */
            if (NetworkClient.Instance.IsHost)
            {
                NetworkClient.Instance.LastSpawner.SpawnForPlayer(0, 1);
            }
            else
            {
                NetworkClient.Instance.LastSpawner.SpawnForPlayer(0, 0);
            }

            // tells the SceneSpawner the local player has finished scene setup.
            NetworkClient.Instance.LastSpawner.PlayerFinishedSceneSetup();
        }
    }
}
