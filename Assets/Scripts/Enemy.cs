using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootCooldown;
    public float shootDistance;
    public bool targetPlayer = true;
    public Polarity.Pole polarity;

    private Transform player;

	void Start()
	{
        player = GameObject.Find("Tin").transform;
        StartCoroutine(Shoot());
	}

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            if (targetPlayer)
            {
                if ((player.position - transform.position).magnitude < shootDistance)
                {
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<Bullet>().SetDir(player.transform.position - transform.position, polarity);
                    yield return new WaitForSeconds(shootCooldown);
                }
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetDir(new Vector2(-1, 0.2f), polarity);
                yield return new WaitForSeconds(shootCooldown);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Bullet")
            if (coll.GetComponent<Bullet>().HasBeenAffected)
            {
                StopAllCoroutines();
                GameObject.Destroy(gameObject);
                GameObject.Destroy(coll.gameObject);
            }
    }
}
