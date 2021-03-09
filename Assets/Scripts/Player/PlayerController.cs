using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Transform player;
    public Animator animator;
    public float speed = 3f;
    public Joystick joystick;

    private float horizontalAxis;
    private float verticalAxis;
    private Vector2 movement;
    public Rigidbody2D myRigidBody;

    public bool stun = false;


    private void Start()
    {
        if (FindObjectsOfType<PlayerController>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stun)
        {
            horizontalAxis = joystick.Horizontal * speed;
            verticalAxis = joystick.Vertical * speed;
            movement = new Vector2(horizontalAxis, verticalAxis);
            MovePlayer(movement);
        }
        else
        {
            myRigidBody.velocity = Vector2.zero;
        }
    }


    void MovePlayer(Vector2 direction)
    {
        direction = CapSpeed(direction);
        myRigidBody.velocity = new Vector2(direction.x * speed, direction.y * speed);

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        if (direction.x == 1 || direction.x == -1 || direction.y == 1 || direction.y == -1)
        {
            animator.SetFloat("lastMoveX", direction.x);
            animator.SetFloat("lastMoveY", direction.y);
        }
    }

    private static Vector2 CapSpeed(Vector2 direction)
    {
        if (direction.x >= 1) { direction.x = 1; }
        if (direction.y >= 1) { direction.y = 1; }
        if (direction.x <= -1) { direction.x = -1; }
        if (direction.y <= -1) { direction.y = -1; }
        return direction;
    }

    public void StopMovement()
    {
        MovePlayer(new Vector2(0, 0));
    }
}
