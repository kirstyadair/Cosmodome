using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public List<GameObject> shipObjects;
    public Transform sourceShip;
    public List<Transform> shipPositions;
    public Camera mainCamera;

    public float dampeningTime;//The time taken for the camera to reajust
    public float screenEdgeBuff;//The space between the edge of the screen and any objects at the top or bottom of the screen
    public float minZoomDistance;
    
    [Header("Camera's minimum and maximum positions")]
    public float minX;
    public float maxX;

    [Header("Center Point")]
    public float centerX;
    public float centerY;
    public float centerZ;



    public List<float> distances;
    public float currentMaxDistance;
    
    private float zoomSpeed; //Speed for the smoothing of the orphographic
    private Vector3 moveVelocity; //Speed for the smoothing of the camera movement
    //public Vector3 centerPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private Vector3 FindCenter(List<Transform>targets)
    {
        Vector3 center;
        Vector3 minPoint = targets[0].position;
        Vector3 maxPoint = targets[0].position;

        for(int i = 1; i<targets.Count; i++)
        {
            
            Vector3 pos = targets[i].position;

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

        return center;

    }

    void FindDistance(List<Transform>targets)
    {
        distances.Clear();
        foreach(Transform transform in targets)
        {
            float dist = Vector3.Distance(sourceShip.position, transform.position);
            distances.Add(dist);
        }

        distances.Sort();
        currentMaxDistance = distances[targets.Count - 1];
    }

    private void Move(Camera cam)
    {

        Vector3 centerPoint = FindCenter(shipPositions);

        centerX = centerPoint.x;
        centerY = centerPoint.y;
        centerZ = centerPoint.z;
        FindDistance(shipPositions);
        if(currentMaxDistance>6)
        {
            centerPoint.x = Mathf.Clamp(centerPoint.x, minX, maxX);
            Vector3 cameraDestination = centerPoint - cam.transform.forward *6* minZoomDistance;
            
            
            Vector3 smoothMove = Vector3.Lerp(cam.transform.position, cameraDestination, dampeningTime);
            cam.transform.position = smoothMove;
            

        }
        else
        {
            Vector3 cameraDestination = centerPoint - cam.transform.forward * currentMaxDistance * minZoomDistance;
          
            Vector3 smoothMove = Vector3.Lerp(cam.transform.position, cameraDestination, dampeningTime);
            cam.transform.position = smoothMove;
        }
        
        

       
        
    }


    private void Zoom()
    {
        
    }


    
    public void SetStartPositionAndSize()
    {
        FindCenter(shipPositions);


        

    }
    // Update is called once per frame
    void LateUpdate()
    {
        
        Move(mainCamera);
        
       

    }
}
