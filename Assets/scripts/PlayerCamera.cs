using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public MapGeneratorMain map;
    public float speed = 10f;
    public Vector3 offset;

    private Vector3 ogOffset;

    public void Awake()
    {
        ogOffset = offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = new Vector3((player.position.x + offset.x), transform.position.y, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);

        transform.position = smoothedPos;      
    }

}
