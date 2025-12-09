using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CiCamMovement : MonoBehaviour
{
    CinemachineCamera cinemachineCamera;

    float minDist = 35f;
    float maxDist = 85f;

    float zoomDuration = 1f;
    
    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void Cinemachine(float speedOfPlayer)
    {
        StopAllCoroutines();
        StartCoroutine(CamerasFieldOfView(speedOfPlayer));
    }

    IEnumerator CamerasFieldOfView(float speedOfPlayer)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedOfPlayer,minDist,maxDist);
        float lerpSpeed = 0f;

        while(lerpSpeed < zoomDuration)
        {
            lerpSpeed += Time.deltaTime;
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV,targetFOV,lerpSpeed);
            yield return null;
        }
        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}
