using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D myRigidBody;

    public bool isWalking;
    public bool dontWalk;

    public float walkTime;
    public float waitTime;
    private float walkCounter;
    private float waitCounter;

    private int walkDirection;

    public Collider2D walkZone;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone;

    private void Start()
    {
        dontWalk = false;
        myRigidBody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }
    }

    private void Update()
    {
        if (dontWalk)
        {
            myRigidBody.velocity = Vector2.zero;
            return;
        }
        else
        {
            if (isWalking)
            {
                walkCounter -= Time.deltaTime;

                Move(walkDirection);

                if (walkCounter <= 0)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }

            }
            else
            {
                waitCounter -= Time.deltaTime;

                myRigidBody.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    ChooseDirection();
                }
            }
        }
    }

    private void Move(int walkDirection)
    {
        SetWalkZone(walkZone);

        switch (walkDirection)
        {
            case 0:
                if(hasWalkZone && transform.position.y > maxWalkPoint.y)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
                else
                {
                    myRigidBody.velocity = new Vector2(0, moveSpeed);
                }
                break;
            case 1:
                if (hasWalkZone && transform.position.x > maxWalkPoint.x)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
                else
                {
                    myRigidBody.velocity = new Vector2(moveSpeed, 0);
                }
                break;
            case 2:
                if (hasWalkZone && transform.position.y < minWalkPoint.y)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
                else
                {
                    myRigidBody.velocity = new Vector2(0, -moveSpeed);
                }
                break;
            case 3:
                if (hasWalkZone && transform.position.x < minWalkPoint.x)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
                else
                {
                    myRigidBody.velocity = new Vector2(-moveSpeed, 0);
                }
                break;
        }
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    public void DontWalk()
    {
        dontWalk = true;
    }

    public void Walk()
    {
        dontWalk = false;
    }

    public void SetWalkZone(Collider2D zone)
    {
        walkZone = zone;
        CalculateWalkZoneBounds();
    }

    public void CalculateWalkZoneBounds()
    {
        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }
    }
}
