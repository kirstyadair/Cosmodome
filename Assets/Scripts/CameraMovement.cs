﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;
    float startFov;

    public List<GameObject> shipObjects;
    public GameObject sourceShip;

    public Camera mainCamera;

    public float dampeningTime;//The time taken for the camera to reajust
    public float dampeningTimeOut;
    public float zoomFactor;

    float lastZoomAmount;

    public float minZoom;
    public float maxZoom;

    [Header("Camera's minimum and maximum positions")]
    public float minX;
    public float maxX;

    public float minCenterZ;
    public float maxCenterZ;
    

    [Header("Center Point")]
    public float centerX;
    public float centerY;
    public float centerZ;

    public GameObject center;

    ScoreManager sm;
    public List<float> distances;
    public float currentMaxDistance;
    Vector3 centerPoint;

    private float zoomSpeed; //Speed for the smoothing of the orphographic
    private Vector3 moveVelocity; //Speed for the smoothing of the camera movement
    //public Vector3 centerPoint;

    Coroutine _lerpToPointCoroutine; // to ensure we only have one running

    void Start()
    {
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        startFov = mainCamera.fieldOfView;
    }

    // Start is called before the first frame update
    void Awake()
    {
        sm = ScoreManager.Instance;

        ScoreManager.OnStateChanged += OnStateChange;
        ScoreManager.OnPlayerEliminated += OnPlayerEliminated;
        mainCamera = GetComponentInChildren<Camera>();

        UpdatePlayerList();




    }

    void OnDisable() {
        ScoreManager.OnStateChanged -= OnStateChange;
        ScoreManager.OnPlayerEliminated -= OnPlayerEliminated;
    }

    void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.INGAME || newState == GameState.COUNTDOWN)
        {
            ResetCamera();
            UpdatePlayerList();
        }
    }

    void OnPlayerEliminated()
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach (var player in shipObjects.ToArray())
        {
            if (!player.activeSelf)
            {
                shipObjects.Remove(player);
            }

        }
        if (sourceShip == null)
        {
            sourceShip = shipObjects[0];
        }
        if (!sourceShip.activeSelf)
        {
            sourceShip = shipObjects[0];
        }
    }

    public void ResetCamera()
    {
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        mainCamera.fieldOfView = startFov;
    }

    private Vector3 FindCenter(List<GameObject> targets)
    {
        Vector3 center;
        Vector3 minPoint = targets[0].transform.position;
        Vector3 maxPoint = targets[0].transform.position;

        for (int i = 1; i < targets.Count; i++)
        {

            Vector3 pos = targets[i].transform.position;

            if (pos.x < minPoint.x)
                minPoint.x = pos.x;
            if (pos.x > maxPoint.x)
                maxPoint.x = pos.x;
            if (pos.y < minPoint.y)
                minPoint.y = pos.y;
            if (pos.y > maxPoint.y)
                maxPoint.y = pos.y;
            if (pos.z < minPoint.z)
                minPoint.z = pos.z;
            if (pos.z > maxPoint.z)
                maxPoint.z = pos.z;
        }


        center = minPoint + .5f * (maxPoint - minPoint);

        //center.y = transform.position.y;
        //center.z = transform.position.z;

        center.z = Mathf.Clamp(center.z, minCenterZ, maxCenterZ);

        return center;

    }

    void FindDistance(List<GameObject> targets)
    {


        distances.Clear();

        foreach (GameObject ship in shipObjects)
        {
            float dist = Vector3.Distance(centerPoint, ship.transform.position);
            distances.Add(dist);
        }

        distances.Sort();
        currentMaxDistance = distances[targets.Count - 1];
    }

    private void Move(Camera cam)
    {

        centerPoint = FindCenter(shipObjects);

        centerX = centerPoint.x;
        centerY = centerPoint.y;
        centerZ = centerPoint.z;
        FindDistance(shipObjects);

        // centerPoint.x = Mathf.Clamp(centerPoint.x, minX, maxX);
        // centerPoint.z = Mathf.Clamp(centerPoint.z, -16f, 40f);

        float zoomAmount = Mathf.Clamp(currentMaxDistance * zoomFactor, minZoom, maxZoom);

        Vector3 cameraDestination = centerPoint - cam.transform.forward * zoomAmount;



        //cam.transform.position = cameraDestination;
      


        Vector3 smoothMove = Vector3.Lerp(cam.transform.position, cameraDestination, dampeningTime);


        cam.transform.position = smoothMove;


       center.transform.position = centerPoint;


    }




    public void SetStartPositionAndSize()
    {
        FindCenter(shipObjects);




    }

    /// <summary>
    /// Starts a coroutine that lerps the camera to the given position and rotation in the specified time
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void LerpToPoint(Vector3 position, Quaternion rotation, float time) {
        if (_lerpToPointCoroutine != null) StopCoroutine(_lerpToPointCoroutine);
        _lerpToPointCoroutine = StartCoroutine(LerpToPointCoroutine(position, rotation, time));
    }

    private IEnumerator LerpToPointCoroutine(Vector3 position, Quaternion rotation, float time) {
        float timePassed = 0;
        Vector3 startPosition = this.transform.position;
        Quaternion startRotation = this.transform.rotation;

        while (timePassed < time) {
            float t = timePassed / time;

            transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0.0f, 1.0f, t));
            transform.rotation = Quaternion.Slerp(startRotation, rotation, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
            timePassed += Time.deltaTime;
        }

        transform.position = position;
        transform.rotation = rotation;

        _lerpToPointCoroutine = null;;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // disable camera control when we are not in game
        if (!sm.isCameraEnabled) return;

        Move(mainCamera);



    }
}
