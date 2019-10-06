using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NRand;

public class PathNode : MonoBehaviour
{
    static List<PathNode> m_nodes = new List<PathNode>();

    private void Awake()
    {
        m_nodes.Add(this);
    }

    private void OnDestroy()
    {
        m_nodes.Remove(this);
    }

    public static PathNode GetRandomNode()
    {
        if (m_nodes.Count == 0)
            return null;

        return m_nodes[new UniformIntDistribution(0, m_nodes.Count - 1).Next(new StaticRandomGenerator<MT19937>())];
    }
}
