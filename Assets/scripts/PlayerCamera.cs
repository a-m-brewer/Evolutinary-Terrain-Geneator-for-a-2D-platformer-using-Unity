using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private GameObject player;
    public float xMinCamera;
    public float xMaxCamera;
    public float yMinCamera;
    public float yMaxCamera;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        //MoveCamera();
    }

    // locks the camera to the  edges set of the level
    void MoveCamera()
    {
        float x = Mathf.Clamp(player.transform.position.x, xMinCamera, xMaxCamera);
        float y = Mathf.Clamp(player.transform.position.y, yMinCamera, yMaxCamera);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }
}
