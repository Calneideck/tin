using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Transform door;

    private bool active = true;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!active)
            return;

        if (coll.tag == "Bullet")
        {
            StartCoroutine(MoveDoor());
            transform.Translate(Vector3.down * 0.2f);
            coll.gameObject.GetComponent<MonoBehaviour>().StopAllCoroutines();
            GameObject.Destroy(coll.gameObject);
            active = false;
        }
    }

    IEnumerator MoveDoor()
    {
        for (int i = 0; i < 60; i++)
        {
            door.Translate(Vector3.down * 0.1f);
            yield return 0.05f;
        }
    }
}