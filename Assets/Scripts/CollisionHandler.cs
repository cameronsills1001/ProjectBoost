using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float loadLevelDelay = 1.0f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    Rigidbody rb;
    public bool inCollision {get; set;} = false;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    bool autoRotationEnabled = false;

    bool brakesApplied = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        RespondToDebugKeys();
        AutoCorrect(transform.rotation.z);
    }

    void RespondToDebugKeys() 
    {
        if(Input.GetKey(KeyCode.C))
        {
            collisionDisabled = collisionDisabled == true ? false : true;
            Debug.Log($"Collision Disabled: {collisionDisabled}");
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        
    }
    void OnCollisionEnter(Collision other) 
    {   
        if(isTransitioning || collisionDisabled) {return;}

        //set if in collision or not
        inCollision = true;
        
        //switch to determine action based on tag of object hit
        switch (other.gameObject.tag) 
        {
            case "Friendly":
                //Debug.Log("Just a friendly. Nothing to worry about");
                //
                break;
            case "Finish":
                //Debug.Log("You WON!!!.....maybe");
                StartSuccessSequence();
                break;
            default:
                Debug.Log("You just crashed!! We will notify your next of kin");
                StartCrashSequence();
                break;

        }
    }

    void OnCollisionExit(Collision other) 
    {
        inCollision = false;
    }

    
    void StartSuccessSequence()
    {
        isTransitioning = true;
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        GetComponent<Movement>().enabled = false;
        autoRotationEnabled = true;
        Invoke("LoadNextLevel", loadLevelDelay);
            
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        crashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        //add particle effects
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadLevelDelay);
    }

//TODO Correct any movement after a win by making the ship sit level instead of falling over with too much lateral movement before win
    

    void AutoCorrect(float currentRotation)
    {
        if(currentRotation != Mathf.Epsilon && autoRotationEnabled)
        {
            if (!brakesApplied)
            {
                brakesApplied = true;
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }
            //rb.velocity = Vector3.zero;
            if(currentRotation < Mathf.Epsilon)
            {
                transform.Rotate(Vector3.forward * 0.2f);
            }
            else
            {
                transform.Rotate(Vector3.back * 0.1f);
            }
            rb.isKinematic = false;
        }
        
        
        
    }

   

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
