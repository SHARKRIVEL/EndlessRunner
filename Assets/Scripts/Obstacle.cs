using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;
    bool obstacleInst = true;

    void Start()
    {
        StartCoroutine(Obstacles());
    }

    IEnumerator Obstacles()
    {
        while(obstacleInst)
        {
            Vector3 rangePos = new Vector3(Random.Range(-5,5),transform.position.y,transform.position.z);
            int obstacle = Random.Range(0,obstacles.Length);
            Instantiate(obstacles[obstacle],rangePos,Quaternion.identity,this.transform);
            yield return new WaitForSeconds(3f);
        }
    }
}
