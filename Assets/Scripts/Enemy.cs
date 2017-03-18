using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootCooldown;
    public float shootDistance;

    private Transform player;

	void Start()
	{
        player = GameObject.Find("Tin").transform;
        StartCoroutine(ShootAtPlayer());
	}

	void Update()
	{

	}

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            if ((player.position - transform.position).magnitude < shootDistance)
            {
                GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetDir(player.position - transform.position);
                yield return new WaitForSeconds(shootCooldown);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
