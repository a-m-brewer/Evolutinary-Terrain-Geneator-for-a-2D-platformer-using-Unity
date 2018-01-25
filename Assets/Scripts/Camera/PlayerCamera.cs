using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for moving the camera
public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public MapGeneratorMain map;
    public float speed = 10f;
    public Vector3 offset;
    [Range(0f, 1f)]
    public float offSetModifierPercent;

    private float floorPos = 0f;
    private float mapSize;
    private float minCamPos;
    private float maxCamPos;

    private void Start()
    {
        CameraSetup(Camera.main, player, map, offset);
    }

    private void LateUpdate()
    {
        float newOffY = GetOffsetY(offset, player, floorPos, offSetModifierPercent);

        float xPos = GetCameraXPos(player, minCamPos, maxCamPos, offset);

        Vector3 desiredPos = GetDesiredCameraPosition(xPos, offset, player, newOffY);

        Vector3 smoothedPos = GetSmoothedCameraPosition(desiredPos, speed);

        SetNewPosition(smoothedPos);
    }

    private void CameraSetup(Camera cam, Transform p, MapGeneratorMain m, Vector3 off)
    {
        floorPos = GetFloorPosition(p);
        minCamPos = GetMinCamPos(off, cam);
        maxCamPos = GetMaxCamPos(m, cam);
    }

    private float GetHalfCameraSize(Camera cam)
    {
        return cam.orthographicSize * Screen.width / Screen.height;
    }

    private float GetFloorPosition(Transform p)
    {
        return p.position.y;
    }

    private float GetMinCamPos(Vector3 off, Camera cam)
    {
        return (GetHalfCameraSize(cam) - off.x) - 0.5f;
    }

    private float GetMaxCamPos(MapGeneratorMain m, Camera cam)
    {
        return m.GetMapSize() - GetHalfCameraSize(cam);
    }

    private float GetOffsetY(Vector3 off, Transform p, float fp, float offsetPercent)
    {
        return off.y - ((p.position.y - fp) * offsetPercent);
    }

    private float GetCameraXPos(Transform p, float minC, float maxC, Vector3 off)
    {
        return Mathf.Clamp(p.position.x, minC, maxC - off.x);
    }

    private Vector3 GetDesiredCameraPosition(float x, Vector3 off, Transform p, float offY)
    {
        return new Vector3(x + off.x, (p.position.y + offY), transform.position.z);
    }

    private Vector3 GetSmoothedCameraPosition(Vector3 dp, float s)
    {
        return Vector3.Lerp(transform.position, dp, s * Time.deltaTime);
    }

    private void SetNewPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

}
