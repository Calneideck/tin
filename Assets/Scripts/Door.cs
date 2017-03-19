using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    public GameObject[] enemies;

    IEnumerator Start()
    {
        bool enemiesAllDead = false;
        while (!enemiesAllDead)
        {
            enemiesAllDead = true;
            for (int i = 0; i < enemies.Length; i++)
                if (enemies[i] != null)
                {
                    enemiesAllDead = false;
                    break;
                }

            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 60; i++)
        {
            transform.Translate(Vector3.down * 0.1f);
            yield return 0.05f;
        }
    }
}
