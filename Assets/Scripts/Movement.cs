using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip thruster;

    //particles
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem boosterParticles;
    [SerializeField] Light engineLight;

    Rigidbody rb;
    AudioSource audioSource;
    AudioSource thrusterAudio;

    public bool firstThrustApplied {get; set;} = false; //using for leg suspension to stop raising/lowering as game is starting


    // Start is called before the first frame update
    void Start()
    {
        engineLight.enabled = false;
        rb = GetComponent<Rigidbody>();
        var aSources = GetComponents<AudioSource>();
        audioSource = aSources[0];
        thrusterAudio = aSources[1];
        
        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust() 
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrustingEffects();
        }
    }

    private void StopThrustingEffects()
    {
        engineLight.enabled = false;
        audioSource.Stop();
        boosterParticles.Stop();
    }

    void StartThrusting()
    {   
        if (!firstThrustApplied)
        {
            firstThrustApplied = true;
        }
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        engineLight.enabled = true;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!boosterParticles.isPlaying)
        {
            boosterParticles.Play();
        }
    }

    void ProcessRotation() {
        
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            RotateRight();
        }
        else
        {
            StopRotationEffects();
        }

    }

    private void StopRotationEffects()
    {
        thrusterAudio.Stop();
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
        if(!thrusterAudio.isPlaying)
            {
                thrusterAudio.PlayOneShot(thruster);
            }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
        if(!thrusterAudio.isPlaying)
            {
                thrusterAudio.PlayOneShot(thruster);
            }
    }

    void ApplyRotation(float rotationThisFrame)
    {   
        rb.freezeRotation = true; //freeze rotation so that manual rotation can take place
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //once again allow physics to rotate 
    }

    void lowerVolume() 
    {
        
        while(audioSource.volume > 0)
        {
            audioSource.volume -= 0.1f;
        }    
        
    } 

    void raiseVolume() 
    {
        
        while (audioSource.volume < 0.9f)
        {
            audioSource.volume += 0.1f;
        }     
        
    } 


    
}
