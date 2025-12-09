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
    int AppleCount;
    int k,i =0,j=0;

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
        buttonText.text = "Collect 20 Apples to Activate";
    }

    void Update()
    {
        CoolDownTime += Time.deltaTime;
        if(AppleCount > 20)
        {
            buttonText.text = "Activated";
        }
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
                AppleCount++;
                buttonText.text = "Apples : " + AppleCount.ToString();
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
        if(healthInc)
        {
            j++;
            if(j>2 && i > 0)
            {
                HealthIndicator[k].SetActive(true);
                k--;
                i--;
                j = 0;
                if(i == 0)
                {
                    healthInc = false;
                }
            }
        }
    }

    void PlayerCollisionDitection()
    {
        audioSource.PlayOneShot(obstacleAudioClip);

        if(CoolDownTime < CollisionCoolTime) return;
        if(i <= 2)
        {
            HealthIndicator[i].SetActive(false);
            healthInc = true;
            k = i;

            if(i == 2)
            {
                float speedRef = path.speedReturn();
                path.SpeedModifier(-speedRef);
                button.SetActive(true);
                Exitbutton.SetActive(true);
                obstacleGB.SetActive(false);
                Destroy(gameObject);
            }
            i++;
        }
        animator.SetTrigger(AnimTrigger);
        CoolDownTime = 0;
    }

    public void ButtonPressedRef()
    {
        if(AppleCount > 20 )
        {
            AppleCount = 0;
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
            buttonText.text = "Time : " + loopIncrementor.ToString();
            yield return null;
        }
        loopIncrementor = 0f;
        collisionAvoidPower = false;
        buttonText.text = "Deactivated";
    }
}
