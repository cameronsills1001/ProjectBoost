using UnityEngine;
using System.Collections;


public class ElevatorMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] float elevatorTop;
    [SerializeField] float elevatorBottom;
    [SerializeField] float moveSpeed;
    [SerializeField] float lpActiveTime = 5.0f;
    [SerializeField] float lpWarningPeriod = 2.0f;
    [SerializeField] AudioClip warningClip;


    AudioSource audioSource;
     GameObject lp;
     Renderer lpRenderer;

     float lpWarningTime;
    int movementStage = 2;
    public bool elevatorLocked {get; private set;} = false;
    public int landingStatus {get; private set;} = 0;
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
        
        if(transform.position.y < elevatorTop)
        {
            transform.position += new Vector3(0f, moveSpeed, 0f) * Time.deltaTime;
        }
        else
        {
            movementStage = 2;
        }
        
    }

    void MoveDown()
    {

        if(transform.position.y > elevatorBottom)
        {
            transform.position -= new Vector3(0f, moveSpeed, 0f) * Time.deltaTime;
        }
        else
        {
            movementStage = 1;
        }
    }

    void ActivateLandingPad()
    {
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
