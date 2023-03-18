using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingLightControl : MonoBehaviour
{
    // Start is called before the first frame update

    ElevatorMovement elevatorScript;
    Light landingLight;
    void Start()
    {
        elevatorScript = GameObject.FindFirstObjectByType<ElevatorMovement>();
        landingLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
    }

    void LightControl()
    {   
        
        switch(elevatorScript.landingStatus)
        {
            case 0:
                landingLight.color = Color.green;
                break;
            case 1:
                landingLight.color = Color.yellow;
                break;
            case 2:
                landingLight.color = Color.red;
                break;
            default:
                Debug.Log($"Something is wrong with the landing lights {elevatorScript.landingStatus}");
                break;

        }
    }
}
