using UnityEngine;

public class ObstacleDeath : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
