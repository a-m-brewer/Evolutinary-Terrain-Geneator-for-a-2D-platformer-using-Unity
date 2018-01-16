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
    public float mapSize;
    public float camSizeHalf;
    public float minCamPos;
    public float maxCamPos;

    private void Start()
    {
        camSizeHalf = transform.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
        floorPos = player.position.y;
        mapSize = (map.rSize.x * map.GetNumRooms()) - 0.5f;
        minCamPos = (camSizeHalf - offset.x) - 0.5f;
        maxCamPos = mapSize - camSizeHalf;
    }

    private void LateUpdate()
    {
        newOffY = offset.y - ((player.position.y - floorPos) * offSetModifierPercent);

        float xPos = Mathf.Clamp(player.position.x, minCamPos, maxCamPos - offset.x);

        Vector3 desiredPos = new Vector3(xPos + offset.x, (player.position.y + newOffY), transform.position.z);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);

        transform.position = smoothedPos;      
    }

}
