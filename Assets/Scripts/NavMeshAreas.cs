using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAreas : MonoBehaviour
{
    public static int Walkable { get; private set; }
    public static int NotWalkable { get; private set; }
    public static int Jump { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public static void Initialize()
    {
        Walkable = NavMesh.GetAreaFromName("walkable");
        NotWalkable = NavMesh.GetAreaFromName("Not Walkable");
        Jump = NavMesh.GetAreaFromName("Jump");
    }
}
