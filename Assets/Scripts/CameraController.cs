using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;

	void Start()
	{
		
	}

	void LateUpdate()
	{
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
	}
}
