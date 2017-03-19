using UnityEngine;
using UnityEngine.UI;
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
    public Text text;

    private Vector3 checkpointPos;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    private Polarity polarity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        polarity = GetComponent<Polarity>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        checkpointPos = transform.position;
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && controller.collisions.below)
            velocity.y = jumpVelocity;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        velocity += polarity.MagnetVelocity;
        controller.Move(velocity * Time.deltaTime);

        // Reset
        if (Input.GetButtonDown("Cancel"))
        {
            transform.position = checkpointPos;
            velocity = Vector3.zero;
        }

        //Cheats
        for (int i = 1; i <= 7; i++)
            if (Input.GetKeyDown(i.ToString()))
            {
                GameObject checkPoint = GameObject.Find("Checkpoint" + i);
                if (checkPoint != null)
                {
                    checkpointPos = checkPoint.transform.position;
                    transform.position = checkpointPos;
                    velocity = Vector3.zero;
                    break;
                }
            }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Checkpoint")
        {
            checkpointPos = coll.transform.position;
            print(coll.gameObject.name);

            if (coll.gameObject.name == "Checkpoint6")
                Camera.main.orthographicSize = 9;
            else if (coll.gameObject.name == "Checkpoint7")
                Camera.main.orthographicSize = 5;
            else if (coll.gameObject.name == "Checkpoint8")
                StartCoroutine(Fade());
        }
        else if (coll.tag == "Bullet")
        {
            transform.position = checkpointPos;
            velocity = Vector3.zero;
            GameObject.Destroy(coll.gameObject);
        }
    }

    IEnumerator Fade()
    {
        float f = 0;
        while (true)
        {
            Color c = Color.white;
            c.a = f;
            text.color = c;
            Camera.main.orthographicSize = 5 + f * 15;
            yield return new WaitForSeconds(0.02f);
            f += 0.01f;
        }
    }
}
