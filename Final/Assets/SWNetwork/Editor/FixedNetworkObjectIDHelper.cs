using System.Collections;
using System.Collections.Generic;
using SWNetwork;
using UnityEditor;
using UnityEngine;

public class FixedNetworkObjectIDHelper : MonoBehaviour
{
    [MenuItem("SocketWeaver/Auto Assign Fixed NetworkObjectID")]
    static void AutoAssignFixedNetworkGameObjectID()
    {
        NetworkID[] networkIDs = FindObjectsOfType<NetworkID>();

        ushort fixedNetworkGameObjectID = 0;

        foreach (NetworkID networkID in networkIDs)
        {
            // skip player controlled networked gameobjects
            if (networkID.IsPlayer)
            {
                continue;
            }

            // skip dynamically spawned networked gameobjects
            if (networkID.IsDynamicallySpawned)
            {
                continue;
            }

            fixedNetworkGameObjectID++;

            networkID.fixedNetworkObjectID = (ushort)fixedNetworkGameObjectID;

            if(fixedNetworkGameObjectID >= 4999)
            {
                Debug.LogError("Exceeded the maximum number(4999) of static Non-Player networked GameObjects.");
                break;
            }
        }

        Debug.Log("Found " + fixedNetworkGameObjectID  + " fixed non-player NetworkIDs in the scene.");
    }
}
