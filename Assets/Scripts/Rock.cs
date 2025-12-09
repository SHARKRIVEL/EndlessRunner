using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class Rock : MonoBehaviour
{
    CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] ParticleSystem collisionParticleSystem;
    AudioSource audioSource;
    float Shakemodifier = 10f;
    float impulseMinValue;
    float collisionTime = 1f;
    float nextCollisionTime = 1f;

    void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        StartCoroutine(CollisionWaitTime());
        
        if(collisionTime >= nextCollisionTime)
        {
            CollisionPoint(other);
            CinemachineImpulse();
            collisionTime = 0f;
        }
    }

    IEnumerator CollisionWaitTime()
    {
        while(collisionTime < nextCollisionTime)
        {
            collisionTime += Time.deltaTime;
            yield return null;
        }
    }

    void CollisionPoint(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        audioSource.Play(); 
    }

    void CinemachineImpulse()
    {
        float impulseValue = Vector3.Distance(transform.position,Camera.main.transform.position);
        float impulseDivValue =(1f/impulseValue) * Shakemodifier;
        impulseMinValue = Mathf.Min(impulseDivValue,1f);
        cinemachineImpulseSource.GenerateImpulse(impulseMinValue);
    }
}
