using UnityEngine;
using System.Collections.Generic;

public class Collisions : MonoBehaviour
{
    float[] gatesPosVal = {-6.5f,0f,6.5f};
    List<int> LanesIndex = new List<int> {0,1,2};

    [SerializeField] GameObject gatePrefab;
    [SerializeField] GameObject ApplePrefab;
    [SerializeField] GameObject CoinPrefab;
    [SerializeField] GameObject WallPrefab;

    float AppleSpawnPer = 0.2f;
    float CoinSpawnPer = 0.8f;
    float gapBetweenTheCoins = 3f;
    float WallProb = 0.1f;

    void Start()
    {
        GatesMethod();
        AppleMethod();
        CoinsMethod();
        WallsMethod();
    }

    void WallsMethod()
    {
        float wallProbRef = Random.Range(0f,1f);
        if(wallProbRef > WallProb) return;       
        Vector3 wallPos = new Vector3(transform.position.x,WallPrefab.transform.position.y,transform.position.z + 5f);
        Instantiate(WallPrefab,wallPos,Quaternion.identity,transform); 
    }

    void GatesMethod()
    {
        int LoopingVal = Random.Range(0,LanesIndex.Count);
        for(int i = 0;i < LoopingVal;i++)
        {
            int RandomLaneIndex = Random.Range(0,LanesIndex.Count);
            int LaneIndexRef = LanesRefVal(RandomLaneIndex);
            Vector3 gatesPos = new Vector3(gatesPosVal[LaneIndexRef],gatePrefab.transform.position.y,transform.position.z);
            Instantiate(gatePrefab,gatesPos,Quaternion.identity,transform);
        }       
    }

    int LanesRefVal(int RandomLaneIndexRef)
    {
        int LanesLineVal = LanesIndex[RandomLaneIndexRef];
        LanesIndex.RemoveAt(RandomLaneIndexRef);
        return LanesLineVal;
    }

    void AppleMethod()
    {
        if(LanesIndex.Count <= 0) return;

        float AppleSpawnPerRef = Random.Range(0f,1f);
        if(AppleSpawnPer <= AppleSpawnPerRef) return;

        int RandomLaneIndexRef1 = Random.Range(0,LanesIndex.Count);
        int LaneIndexRef = LanesRefVal(RandomLaneIndexRef1);
        
        Vector3 ApplePos = new Vector3(gatesPosVal[LaneIndexRef],ApplePrefab.transform.position.y,transform.position.z);
        Instantiate(ApplePrefab,ApplePos,Quaternion.identity,transform);
    }

    void CoinsMethod()
    {
        if(LanesIndex.Count <= 0) return;
        float AppleSpawnPerRef = Random.Range(0f,1f);
        if(CoinSpawnPer <= AppleSpawnPerRef) return;

        int RandomLaneIndexRef2 = Random.Range(0,LanesIndex.Count);
        int LaneIndexRef = LanesRefVal(RandomLaneIndexRef2);

        int noOfCoins = 8;
        int CoinsPerFloor = Random.Range(0,noOfCoins);
        float CoinSpawnPosInZref = transform.position.z + (gapBetweenTheCoins*3f);

        for(int j =0;j<CoinsPerFloor;j++)
        {
            float CoinSpawnPosInZ = CoinSpawnPosInZref - (j*gapBetweenTheCoins);
            
            Vector3 CoinPos = new Vector3(gatesPosVal[LaneIndexRef],CoinPrefab.transform.position.y,CoinSpawnPosInZ);
            Instantiate(CoinPrefab,CoinPos,Quaternion.identity,transform);
        }
        
    }
}
