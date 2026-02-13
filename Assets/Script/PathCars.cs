using System;
using System.Collections.Generic;
using UnityEngine;

public class PathCars : MonoBehaviour
{
    [SerializeField] private Color gizmosColor;
    public List<Transform> nodes;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        nodes = new List<Transform>();
        var tempNodes = GetComponentsInChildren<Transform>();

        foreach (var tempNode in tempNodes)
        {
            if (tempNode == transform) continue;
            nodes.Add(tempNode);
            Gizmos.DrawWireSphere(tempNode.position, 1f);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            var targetNode = i + 1 == nodes.Count ? 0 : i + 1;

            Gizmos.DrawLine(nodes[i].position, nodes[targetNode].position);
        }
    }
}