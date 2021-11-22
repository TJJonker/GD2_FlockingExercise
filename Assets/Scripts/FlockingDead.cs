using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingDead : MonoBehaviour {

    private float timer;
    private Swarm swarm;

    [SerializeField]
    private Sprite regularSprite;

    [SerializeField]
    private Sprite zombieSprite;

    [SerializeField]
    private GameObject agentPrefab;

    [SerializeField]
    private BoxCollider2D boundary;

    [SerializeField]
    private int swarmCount;

    public void Start()
    {
        swarm = new Swarm(swarmCount, boundary, zombieSprite, regularSprite, agentPrefab);

        timer = 0;
    }

    public void Update()
    {
        swarm.MoveAgents();
    }
}
