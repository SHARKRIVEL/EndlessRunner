using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject[] instanObstacles;
    bool obstacleInst = true;

    void Start()
    {
        instanObstacles = new GameObject[obstacles.Length];
        for(int i=0;i<obstacles.Length;i++)
        {
            instanObstacles[i] = Instantiate(obstacles[i],transform.position,Quaternion.identity,this.transform);
            instanObstacles[i].SetActive(false);
        }
        StartCoroutine(Obstacles());
    }

    IEnumerator Obstacles()
    {
        while(obstacleInst)
        {
            Vector3 rangePos = new Vector3(Random.Range(-5,5),transform.position.y,transform.position.z);
            int obstacle = Random.Range(0,instanObstacles.Length);
            if(!instanObstacles[obstacle].activeSelf)
            {   
                instanObstacles[obstacle].transform.position = rangePos;
                instanObstacles[obstacle].SetActive(true);
                yield return new WaitForSeconds(3f);
            }
            yield return null;
        }
    }
}
