using UnityEngine;
using System.Collections;

public class WallMagnet : MonoBehaviour
{
    public Polarity.Pole polarity;
    public float force = 180;

    private CircleCollider2D circle;

    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// The radius of the circle collider in world space
    /// </summary>
    public float Radius
    {
        get
        {
            return Mathf.Max(transform.localScale.x, transform.localScale.y) * circle.radius;
        }
    }
}
