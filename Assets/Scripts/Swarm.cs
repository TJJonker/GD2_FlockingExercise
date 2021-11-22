using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm {
    public List<Agent> agents = new List<Agent>();

    public Swarm(int swarmCount, BoxCollider2D boundary, Sprite zombieSprite, Sprite regularSprite, GameObject agentPrefab)
    {
        for (int i = 0; i < swarmCount; i++)
        {
            Agent agent = GameObject.Instantiate(agentPrefab).GetComponent<Agent>();
            agent.Initialize((i > swarmCount * 0.7f), zombieSprite, regularSprite, boundary);
            agents.Add(agent);
        }
    }

    public void MoveAgents()
    {
        foreach (Agent a in agents)
        {
            a.Move(agents);
        }
    }
}
