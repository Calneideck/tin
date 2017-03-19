using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Material red, blue;

    private Vector2 velocity;
    private Polarity.Pole polarity;
    private bool hasBeenAffected;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
    }

    public void SetDir(Vector2 dir, Polarity.Pole polarity)
    {
        velocity = dir.normalized * speed;
        this.polarity = polarity;
        if (polarity == Polarity.Pole.BLUE)
            GetComponent<Renderer>().material = blue;
        else if (polarity == Polarity.Pole.RED)
            GetComponent<Renderer>().material = red;
    }

    void Update()
	{
        transform.Translate(velocity * Time.deltaTime);
	}

    public void AddMagnetForce(Vector2 force)
    {
        velocity += force;
        hasBeenAffected = true;
    }

    public Polarity.Pole Pole
    {
        get { return polarity; }
    }

    public bool HasBeenAffected
    {
        get { return hasBeenAffected; }
    }
}
