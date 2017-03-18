using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Polarity.Pole polarity;

    private Vector2 velocity;

    public void SetDir(Vector2 dir)
    {
        velocity = dir.normalized * speed;
    }

	void Update()
	{
        transform.Translate(velocity * Time.deltaTime);
	}

    public void AddMagnetForce(Vector2 force)
    {
        velocity += force;
    }
}
