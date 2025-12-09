using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speedRef = speed*Time.deltaTime;
        transform.Rotate(0,1*speedRef,0);
    }
}
