using UnityEngine;
using System.Collections.Generic;

public class Polarity : MonoBehaviour
{
    public enum Pole { NONE, RED, BLUE };

    public float magnetForce = 180;
    public float magnetBulletForce = 50;
    public float magnetRadius;
    public LayerMask bulletMask;

    private GameObject red;
    private GameObject blue;
    private bool lTriggerDown, rTriggerDown;
    private Pole polarity = Pole.NONE;
    private List<WallMagnet> magnets = new List<WallMagnet>();

    void Start()
	{
        red = GameObject.Find("RedMagnet");
        red.SetActive(false);
        blue = GameObject.Find("BlueMagnet");
        blue.SetActive(false);
    }

	void Update()
	{
        ChangePolarity();
        AffectBullets();
    }

    void ChangePolarity()
    {
        if (Input.GetAxisRaw("Fire1") == 1 && !lTriggerDown)
        {
            red.SetActive(true);
            polarity = Pole.RED;
            lTriggerDown = true;
            blue.SetActive(false);
        }

        if (Input.GetAxisRaw("Fire2") == 1 && !rTriggerDown)
        {
            blue.SetActive(true);
            rTriggerDown = true;
            polarity = Pole.BLUE;
            red.SetActive(false);
        }

        if (Input.GetAxisRaw("Fire1") == 0 && lTriggerDown)
        {
            red.SetActive(false);
            if (!rTriggerDown)
                polarity = Pole.NONE;
            lTriggerDown = false;
        }
        
        if (Input.GetAxisRaw("Fire2") == 0 && rTriggerDown)
        {
            blue.SetActive(false);
            if (!lTriggerDown)
                polarity = Pole.NONE;
            rTriggerDown = false;
        }
    }

    void AffectBullets()
    {
        if (polarity != Pole.NONE)
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, magnetRadius, bulletMask))
            {
                float pct = 1 - Mathf.Clamp01((collider.transform.position - transform.position).magnitude / magnetRadius);
                if (polarity == collider.GetComponent<Bullet>().Pole)
                    collider.GetComponent<Bullet>().AddMagnetForce((collider.transform.position - transform.position).normalized * magnetBulletForce * pct * Time.deltaTime);
                else
                    collider.GetComponent<Bullet>().AddMagnetForce((transform.position - collider.transform.position).normalized * magnetBulletForce * pct * Time.deltaTime);
            }
    }

    public Vector3 MagnetVelocity
    {
        get
        {
            Vector3 velocity = Vector3.zero;
            foreach (WallMagnet magnet in magnets)
            {
                float pct = 1 - Mathf.Clamp01((magnet.transform.position - transform.position).magnitude / magnet.Radius);
                if (magnet.polarity == polarity && polarity != Pole.NONE)
                {
                    // Repel
                    velocity += (transform.position - magnet.transform.position).normalized * pct * magnet.force * Time.deltaTime;
                }
                else if (polarity != Pole.NONE)
                {
                    // Attract
                    velocity += (magnet.transform.position - transform.position).normalized * pct * magnet.force * Time.deltaTime;
                }
            }
            return velocity;
        }
    }

    public Pole PlayerPolarity
    {
        get
        {
            return polarity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WallMagnet magnet = collision.GetComponent<WallMagnet>();
        if (magnet != null)
            magnets.Add(magnet);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        WallMagnet magnet = collision.GetComponent<WallMagnet>();
        if (magnet != null && magnets.Contains(magnet))
            magnets.Remove(magnet);
    }
}
