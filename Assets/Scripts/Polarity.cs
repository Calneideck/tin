using UnityEngine;
using System.Collections.Generic;

public class Polarity : MonoBehaviour
{
    public enum Pole { NONE, RED, BLUE };

    public float magnetForce = 1;

    private GameObject red;
    private GameObject blue;
    private Pole polarity = Pole.NONE;
    private List<WallMagnet> magnets = new List<WallMagnet>();
    private Controller2D controller;

    void Start()
	{
        red = GameObject.Find("RedMagnet");
        red.SetActive(false);
        blue = GameObject.Find("BlueMagnet");
        blue.SetActive(false);
        controller = GetComponent<Controller2D>();
    }

	void Update()
	{
        ChangePolarity();

    }

    void ChangePolarity()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            red.SetActive(true);
            polarity = Pole.RED;
            blue.SetActive(false);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            blue.SetActive(true);
            polarity = Pole.BLUE;
            red.SetActive(false);
        }

        if (Input.GetButtonUp("Fire1"))
            if (polarity == Pole.RED)
            {
                red.SetActive(false);
                polarity = Pole.NONE;
            }
        
        if (Input.GetButtonUp("Fire2"))
            if (polarity == Pole.BLUE)
            {
                blue.SetActive(false);
                polarity = Pole.NONE;
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
                    velocity += (transform.position - magnet.transform.position).normalized * pct * magnetForce * Time.deltaTime;
                }
                else if (polarity != Pole.NONE)
                {
                    // Attract
                    velocity += (magnet.transform.position - transform.position).normalized * pct * magnetForce * Time.deltaTime;
                }
            }
            return velocity;
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
