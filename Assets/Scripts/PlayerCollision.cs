using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    float CollisionCoolTime = 1f;
    float CoolDownTime;
    float speedIncForEveryChecPoint = 1f;
    float loopIncrementor = 0;
    int Score = 0,ScoreInc = 100;
    int AppleCountForPower = 0;
    int activeHealthIndex = 0,appleCountForHealth = 0;

    bool healthInc = false;
    bool collisionAvoidPower = false;
    const string AnimTrigger = "Hit";
    string Coin = "coin";
    string apple = "Apple";
    string checkPointTag = "CheckPoint";
    string obstacle = "Obstacle";

    [Header("Required Components")]
    Path path;
    [SerializeField] GameObject button;
    [SerializeField] GameObject obstacleGB;
    [SerializeField] GameObject Exitbutton;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] GameObject[] HealthIndicator = new GameObject[3];
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip coinAudioClip;
    [SerializeField] AudioClip appleAudioClip;
    [SerializeField] AudioClip checkPointAudioClip;
    [SerializeField] AudioClip obstacleAudioClip;

    void Start()
    {
        button.SetActive(false);
        Exitbutton.SetActive(false);
        path = FindFirstObjectByType<Path>();
        ScoreText.text = "SCORE :" + Score;
        audioSource = GetComponent<AudioSource>();
        buttonText.text = "20 Apples to Activate";
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(Coin))
        {
            Score += ScoreInc;
            ScoreText.text = "SCORE :" + Score;
            audioSource.PlayOneShot(coinAudioClip);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag(apple))
        {   
            audioSource.PlayOneShot(appleAudioClip);
            if(!collisionAvoidPower)
            {
                AppleCountForPower++;
                buttonText.text = "Apples : " + AppleCountForPower.ToString();
                if(AppleCountForPower > 20)
                {
                    buttonText.text = "Activated";
                }
            }
            Destroy(other.gameObject);
            HealthCheckMethod();
        }
        else if((other.gameObject.CompareTag(checkPointTag)))
        { 
            audioSource.PlayOneShot(checkPointAudioClip);
            path.SpeedModifier(speedIncForEveryChecPoint);       
        }
        else if((other.gameObject.CompareTag(obstacle)))
        {
            if(!collisionAvoidPower)
            {
                PlayerCollisionDitection();
            }  
        }
    }

    void HealthCheckMethod()
    {
        if(HealthIndicator[0].activeSelf) return;

        appleCountForHealth++;
        if(appleCountForHealth == HealthIndicator.Length)
        {
            activeHealthIndex--;
            HealthIndicator[activeHealthIndex].SetActive(true);
            appleCountForHealth = 0;
        }
    }

    void PlayerCollisionDitection()
    {
        if(!gameObject.activeSelf) return;
        
        audioSource.PlayOneShot(obstacleAudioClip);

        if(activeHealthIndex < HealthIndicator.Length)
        {
            HealthIndicator[activeHealthIndex].SetActive(false);

            if(activeHealthIndex == HealthIndicator.Length-1)
            {
                float speedRef = path.speedReturn();
                path.SpeedModifier(-speedRef);
                button.SetActive(true);
                Exitbutton.SetActive(true);
                obstacleGB.SetActive(false);
                Destroy(gameObject);
            }
            activeHealthIndex++;
        }
        if(animator.GetBool("slide")) return;
        animator.SetBool("gotHit",true);
    }

    public void OnPowerPressed()
    {
        if(AppleCountForPower > 20 )
        {
            AppleCountForPower = 0;
            collisionAvoidPower = true;
            StartCoroutine(CollisionActivation());
        }
    }
    
    IEnumerator CollisionActivation()
    {
        float loopTimeOut = 10f;
        while(loopIncrementor < loopTimeOut)
        {
            loopIncrementor += Time.deltaTime;
            buttonText.text = "Time : " + loopIncrementor.ToString("F1");
            yield return null;
        }
        loopIncrementor = 0f;
        collisionAvoidPower = false;
        buttonText.text = "Deactivated";
    }
}
