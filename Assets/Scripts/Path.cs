using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Path : MonoBehaviour
{
    [Header("Required Objects")]
    [SerializeField] Transform pathParent;
    [SerializeField] GameObject[] PathObject;
    [SerializeField] GameObject PathObjectCheckPoint;
    [SerializeField] CiCamMovement cinemachineCameraMovement;

    GameObject pathObjectRef;

    [Header("Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] int maxSpeed = 30;
    [SerializeField] int minSpeed = 5;
    float physicsZvalue = -9.81f;
    int startingTileAmount = 12;
    int normalTileCount = 0;
    int checkPointInsVal = 15;
    int Level = 0;

    List<GameObject> pathFloors = new List<GameObject>();

    void Start()
    {
        Physics.gravity = new Vector3(Physics.gravity.x,Physics.gravity.y,physicsZvalue);
        for (int i=0;i<startingTileAmount;i++)
        {            
            TileStartInst();
        }
    }

    void TileStartInst()
    {
        float InsPos = TilePos();
        Vector3 instantiationPos = new Vector3(transform.position.x,transform.position.y,InsPos);
        GameObject nextTile = NextTileMethod();
        pathObjectRef = Instantiate(nextTile,instantiationPos,Quaternion.identity,pathParent);
        pathFloors.Add(pathObjectRef);
        LevelReturner();
        normalTileCount++;   
    }

    float TilePos()
    {
        float InsPos;
        if(pathFloors.Count == 0)
        {
            InsPos = transform.position.z;
        }
        else 
        {
            InsPos = pathFloors[pathFloors.Count -1].transform.position.z + 20;
        }
        return InsPos;
    }

    public GameObject NextTileMethod()
    {
        GameObject nextTile;
        if(normalTileCount % checkPointInsVal == 0 && normalTileCount != 0)
        {
            nextTile = PathObjectCheckPoint;
        }
        else
        {
            nextTile = PathObject[Random.Range(0,PathObject.Length)];
        }
        return nextTile;
    }

    public void SpeedModifier(float speedRef)
    {
        float zoomSpeed = speedRef*2f;
        cinemachineCameraMovement.Cinemachine(zoomSpeed);
        speed += speedRef;
        if(speed != 0)
        {
            speed = Mathf.Clamp(speed,minSpeed,maxSpeed);
        }
        else
        {
            StartCoroutine(SpeedManager(speedRef));
        }
        speedReturn();
        Physics.gravity = new Vector3(Physics.gravity.x,Physics.gravity.y,physicsZvalue - speedRef);

    }

    IEnumerator SpeedManager(float speedRef)
    {
        float t = 0;
        int endPoint = 0;
        while(t<1)
        {
            t += Time.deltaTime;
            speed = Mathf.Lerp(-speedRef,endPoint,t);
            yield return null;
        }
        speed = 0;
    }

    public float speedReturn()
    {
        return speed;
    }

    void Update()
    {
        float speedRef = speed * Time.deltaTime;
        for(int j = 0 ; j<pathFloors.Count ; j++)
        {
            GameObject pathFloor = pathFloors[j];
            pathFloor.transform.Translate(0,0,-1*speedRef);
            if(pathFloor.transform.position.z < Camera.main.transform.position.z - 20)
            {
                GameObject pathObject = pathFloor;
                pathFloors.Remove(pathObject);
                Destroy(pathObject);
                GameObject nextTile;
                float InsPos = TilePos();
                nextTile = NextTileMethod();
                Vector3 nextFloor = new Vector3(transform.position.x,transform.position.y,InsPos);   
                pathFloors.Add(Instantiate(nextTile,nextFloor,Quaternion.identity,pathParent));
                LevelReturner();
                normalTileCount++; 
            }
        } 
    }

    public int LevelReturner()
    {
        if(normalTileCount % checkPointInsVal == 0 && normalTileCount != 0)
        {
            Level++; 
        }
        return Level;
    }
}
