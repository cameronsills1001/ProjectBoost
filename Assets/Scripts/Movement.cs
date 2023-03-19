using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 1000f; //force to be applied from the main rocket 
    [SerializeField] float rotationSpeed = 200f; // force to be applied from the thruster for rotation/steering
    //audio clips
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip thruster;

    //particles
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem boosterParticles;
    //light from main rocket
    [SerializeField] Light engineLight;

    Rigidbody rb;
    AudioSource audioSource; //audio source for main rocket
    AudioSource thrusterAudio; //audio source for the thrusters

    public bool firstThrustApplied {get; set;} = false; //using for leg suspension to stop raising/lowering as game is starting


    // Start is called before the first frame update
    void Start()
    {
        engineLight.enabled = false;
        rb = GetComponent<Rigidbody>();
        var aSources = GetComponents<AudioSource>(); //making a list of available audio sources
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
        //applying the main rocket
        if (Input.GetKey(KeyCode.Space))
        {
            StartMainRocket();
        }
        else
        {
            StopMainRocket();
        }
    }

    private void StopMainRocket()
    {
        engineLight.enabled = false;
        audioSource.Stop();
        boosterParticles.Stop();
    }

    void StartMainRocket()
    {   
        // applying force and effects for main rocket

        //keep legs from moving before the thrust is applied
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
        //rotate the craft for steering
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
        //stop effects applied during rotation
        thrusterAudio.Stop();
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void RotateRight()
    {
        //roation to the right
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
        //roation to the left
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
        // apply the transform rotation to the craft
        rb.freezeRotation = true; //freeze rotation so that manual rotation can take place
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //once again allow physics to rotate 
    }
    
}
