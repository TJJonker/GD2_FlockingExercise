using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    private static float sight = 100f;
    private static float space = 150f;
    private static float movementSpeed = 75f;
    private static float rotateSpeed = 3f;
    private static float distToBoundary = 100f;

    private BoxCollider2D boundary;

    private Vector2 dir;

    public bool isZombie;
    public Vector2 position;
    public SpriteRenderer sprRenderer;

    private Sprite zombieSprite;

    public void Initialize(bool zombie, Sprite zombieSprite, Sprite regularSprite, BoxCollider2D boundary)
    {
        position = new Vector2(Random.Range(boundary.bounds.min.x + distToBoundary, boundary.bounds.max.x - distToBoundary), Random.Range(boundary.bounds.min.y + distToBoundary, boundary.bounds.max.y - distToBoundary));
        transform.position = position;

        this.boundary = boundary;

        isZombie = zombie;
        
        sprRenderer = GetComponent<SpriteRenderer>();

        this.zombieSprite = zombieSprite;

        dir = new Vector2();

        if (isZombie)
            sprRenderer.sprite = zombieSprite;
        else
            sprRenderer.sprite = regularSprite;
    }

    public void Move(List<Agent> agents)
    {
        //Agents flock, zombie's hunt 
        if (!isZombie) Flock(agents);
        else Hunt(agents);
        CheckBounds();
        CheckSpeed();

        position.x += dir.x;
        position.y += dir.y;

        Vector2 direction = (Vector3)position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        transform.position = position;
    }

    private void Flock(List<Agent> agents)
    {
        foreach (Agent a in agents)
        {
            if (a == this) continue;
            float distance = Distance(position, a.position);
            if (!a.isZombie)
            {
                if (distance < space)
                {
                    // Separation
                    dir += (position - a.position);   
                }
                else if (distance < sight)
                {
                    // Cohesion
                    //dX += TODO
                    //dY += TODO
                }
                if (distance < sight)
                {
                    // Alignment
                    //dX += TODO
                    //dY += TODO
                }
            }
            if (a.isZombie && distance < sight)
            {
                // Evade
                dir += position - a.position;    
            }
        }
    }

    private void Hunt(List<Agent> agents)
    {
        float range = float.MaxValue;
        Agent prey = null;
        foreach (Agent a in agents)
        {
            if (!a.isZombie)
            {
                float distance = Distance(position, a.position);
                if (distance < sight && distance < range)
                {
                    range = distance;
                    prey = a;
                }
            }
        }
        if (prey != null)
        {
            // Move towards prey.
            dir += (prey.position - position);  
        }
    }

    private static float Distance(Vector2 p1, Vector2 p2)
    {
        float val = Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2);
        return Mathf.Sqrt(val);
    }

    private void CheckBounds()
    {
        if (position.x < boundary.bounds.min.x + distToBoundary)
            dir.x += boundary.bounds.min.x + distToBoundary - position.x;
        if (position.y < boundary.bounds.min.y + distToBoundary)
            dir.y += boundary.bounds.min.y + distToBoundary - position.y;

        if (position.x > boundary.bounds.max.x - distToBoundary)
            dir.x += boundary.bounds.max.x - distToBoundary - position.x;
        if (position.y > boundary.bounds.max.y - distToBoundary)
            dir.y += boundary.bounds.max.y - distToBoundary - position.y;
    }

    private void CheckSpeed()
    {
        float s;
        if (!isZombie) s = movementSpeed * Time.deltaTime;
        else s = movementSpeed / 3f * Time.deltaTime; //Zombies are slower

        float val = Distance(Vector2.zero, dir);
        if (val > s)
        {
            dir = dir * s / val;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Agent otherAgent = other.gameObject.GetComponent<Agent>();

        if(otherAgent != null)
        {
            // if im not a zombie and the other is, become a zombie
            if (otherAgent.isZombie && !this.isZombie)
                BecomeZombie();
        }
    }

    private void BecomeZombie()
    {
        isZombie = true;
        sprRenderer.sprite = zombieSprite;
    }
}
