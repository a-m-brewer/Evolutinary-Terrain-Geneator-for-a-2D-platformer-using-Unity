using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for moving the camera
public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public MapGeneratorMain map;
    public float speed = 10f;
    public Vector3 offset;

    public float floorPos = 0f;
    public float newOffY = 0;
    [Range(0f, 1f)]
    public float offSetModifierPercent;
    public float distFromEnd;

    private void Start()
    {
        floorPos = player.position.y;
        Debug.Log(floorPos);
    }

    private void LateUpdate()
    {
        distFromEnd = 95f - player.position.x;
        newOffY = offset.y - ((player.position.y - floorPos) * offSetModifierPercent);
        Vector3 desiredPos = new Vector3((player.position.x + offset.x), (player.position.y + newOffY), transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);

        transform.position = smoothedPos;      
    }

}
