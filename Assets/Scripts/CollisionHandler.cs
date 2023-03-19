using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    ///<summary>  <summary>
    [SerializeField] float loadLevelDelay = 1.0f; // Delay from when a player reaches their objective and a new level is loaded
    //audio clips
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    //particle effects
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    //components
    AudioSource audioSource;
    Rigidbody rb;


   
    
    public bool inCollision {get; private set;} = false;  //public property for signaling landing leg extension/retraction

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

        //set true if in collision
        inCollision = true;
        
        //switch to determine action based on tag of object hit
        switch (other.gameObject.tag) 
        {
            case "Friendly":
                //Friendly contacts should not cause any outcome
                break;
            case "Finish":
                // For touching the landing pad indicating a 'win' for the level
                StartSuccessSequence();
                break;
            default:
                //touching anything not tagged above is a loss
                StartCrashSequence();
                break;

        }
    }

    void OnCollisionExit(Collision other) 
    {   
        //signaling the lander legs that the lander is no longer in a collision
        inCollision = false;
    }

    
    void StartSuccessSequence()
    {
        //actions to take for a successful landing
        isTransitioning = true;
        successParticles.Play();
        audioSource.Stop(); //stop any current audio
        audioSource.PlayOneShot(success);
        GetComponent<Movement>().enabled = false; //diable the players ability to continue to control the craft
        autoRotationEnabled = true; //signals AutoCorrect to control the ship post win
        Invoke("LoadNextLevel", loadLevelDelay);
            
    }

    void StartCrashSequence()
    {
        //actions to be taken when the ship crashes
        isTransitioning = true;
        crashParticles.Play();
        audioSource.Stop(); //stop any current audio
        audioSource.PlayOneShot(crash);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadLevelDelay);
    }

    

    void AutoCorrect(float currentRotation)
    {
        //attempt to stop ship from moving and then apply rotation to keep it level after 
        //a successful landing. This should stop the ship from tumbling or rolling after it has landed. 
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
        //reload the same level in case of a crash
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        //load the next level when player is successful at landing. Will start at 
        // initial level if last level is complete
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
