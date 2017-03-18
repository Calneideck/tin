using UnityEngine;
using System.Collections;

// https://github.com/SebLague/2DPlatformer-Tutorial

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    private Vector3 startPos;

    Controller2D controller;
    private Polarity polarity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        polarity = GetComponent<Polarity>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        startPos = transform.position;
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            velocity.y = jumpVelocity;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        velocity += polarity.MagnetVelocity;
        controller.Move(velocity * Time.deltaTime);

        // Reset
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = startPos;
            velocity = Vector3.zero;
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }
}
