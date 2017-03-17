using UnityEngine;
using System.Collections;

public class WallMagnet : MonoBehaviour
{
    public Polarity.Pole polarity;

    private CircleCollider2D circle;

    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }

    public float Radius
    {
        get
        {
            return Mathf.Max(transform.localScale.x, transform.localScale.y) * circle.radius;
        }
    }
}
