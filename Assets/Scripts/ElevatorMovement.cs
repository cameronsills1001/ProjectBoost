using UnityEngine;
using System.Collections;


public class ElevatorMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] float elevatorTop; //setting constraints for the highest level of the elevator
    [SerializeField] float elevatorBottom; //setting contraints for the lowest level of the elevator
    [SerializeField] float moveSpeed; //speed elevator should move
    [SerializeField] float lpActiveTime = 5.0f; //total time the landing pad is available for landing per cycle
    [SerializeField] float lpWarningPeriod = 2.0f; //warning period before the landing pad become unavailable
    [SerializeField] AudioClip warningClip; //audio clip for landing pad warning


    AudioSource audioSource;
     GameObject lp;
     Renderer lpRenderer;

     float lpWarningTime; //landing pad active time - warning period
    int movementStage = 2; //setting initial stage for elevator movement
    bool elevatorLocked = false;
    public int landingStatus {get; private set;} = 0; // 
    void Start()
    {
        lp = GameObject.Find("LandingPad");   
        lpRenderer  = lp.GetComponent<Renderer>();
        lpWarningTime = lpActiveTime - lpWarningPeriod;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessElevatorMovement(movementStage);
    
    }

    void ProcessElevatorMovement(int movementStage)
    {
        //uses a switch statement to indicate what stage of the elevator cycle is to be called
        switch(movementStage)
        {
            case 0:
                MoveDown();
                break;
            case 1:
                MoveUp();
                break;
            case 2:
                ActivateLandingPad();
                break;
            default:
                Debug.Log("Something went wrong with the elevator movement");
                break;
        }
    }
    void MoveUp()
    {
        //elevator will move towards its top contstraint
        if(transform.position.y < elevatorTop)
        {
            transform.position += new Vector3(0f, moveSpeed, 0f) * Time.deltaTime;
        }
        else
        {
            //set next stage in elevator movement
            movementStage = 2;
        }
        
    }

    void MoveDown()
    {
        //move elevator towards its bottom constraint
        if(transform.position.y > elevatorBottom)
        {
            transform.position -= new Vector3(0f, moveSpeed, 0f) * Time.deltaTime;
        }
        else
        {
            //set next stage in elevator movement
            movementStage = 1;
        }
    }

    void ActivateLandingPad()
    {
        //make landing pad available for landing
        if(!elevatorLocked)
        {   
            elevatorLocked = true;
            landingStatus = 0;
            lp.tag = "Finish";

            StartCoroutine(KeepLandingPadActive());
        }
        
        
    }

    IEnumerator KeepLandingPadActive()
    {
        //before the pause
        lpRenderer.material.SetColor("_EmissionColor",Color.green);
        StartCoroutine(LandingPadWarning());
        yield return new WaitForSecondsRealtime(lpActiveTime);
        //after the pause
        lp.tag = "Untagged";
        elevatorLocked = false;
        landingStatus = 2;
        lpRenderer.material.SetColor("_EmissionColor",Color.red);
        movementStage = 0;
        
    }

    IEnumerator LandingPadWarning()
    {
        //elevatorLocked = false;
        yield return new WaitForSecondsRealtime(lpWarningTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(warningClip);
        }
        landingStatus = 1;
        lpRenderer.material.SetColor("_EmissionColor",Color.yellow);
    }

}
