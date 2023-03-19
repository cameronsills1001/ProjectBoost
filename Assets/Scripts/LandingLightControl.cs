using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingLightControl : MonoBehaviour
{
    // Start is called before the first frame update

    ElevatorMovement elevatorScript; //access ElevatorMovement script
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
        //using a switch to select the color of the landing lights depending on the status of the landing pad
        // case 0 = available/green     case 1 = warning/yellow    case 2 = unavailable/red
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
