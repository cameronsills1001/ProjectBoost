using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegSuspension : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initalPosition; //starting position of the legs
    //selecting leg parts
    Transform lowerLeg;
    Transform upperLeg;
    Transform strut;
    Transform foot;

    Movement landerMovement; //access Movement script
    CollisionHandler collisionScript; // access CollisionHandler

    //adjustments for leg movements
    float timeVariable = 12.0f;
    float rotateVariable;
    float movementVectorX = 0.05f;
    float movementVectorY = 0.1f;
    float movementVectorZ = 0;

    

    void Start()
    {
        lowerLeg = transform.Find("Lower Leg");
        upperLeg = transform.Find("Upper Leg");
        strut = transform.Find("Strut");
        foot = transform.Find("Foot");
        initalPosition = lowerLeg.localPosition;
        float timeVariable = 12.0f;
        rotateVariable = 0.5f  * timeVariable;
        landerMovement = GetComponentInParent<Movement>(); //access movement script to see if any thrust has been applied
        collisionScript = GetComponentInParent<CollisionHandler>(); //access the collision handler script
        LegUp(); //manually set legs to collision mode.
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLegMovement();
    }


    void ProcessLegMovement()
    {   
        // determing leg movement
        if (collisionScript.inCollision && landerMovement.firstThrustApplied)
        {  
            LegUp();
        }
        else 
        {
           
            LegDown();
        }
    }

    void LegDown()
    {   
        // move the leg parts down
        if(lowerLeg.transform.localPosition.y > -0.1 && landerMovement.firstThrustApplied)
        {
            // The orginal dialed in vector3 that seemded to work is (0.05f, 0.1f, 0)
            lowerLeg.transform.localPosition -= new Vector3(movementVectorX, movementVectorY, movementVectorZ) * Time.deltaTime  * timeVariable;
            upperLeg.transform.Rotate(Vector3.back * 20 * Time.deltaTime * rotateVariable);
            strut.transform.Rotate(-Vector3.forward * 10 * Time.deltaTime *  rotateVariable);
            foot.transform.localPosition -= new Vector3(movementVectorX, movementVectorY, movementVectorZ) * Time.deltaTime  * timeVariable;     
        }
           
    }

    void LegUp()
    {
        //move leg parts up
        if(lowerLeg.transform.localPosition.y <=  initalPosition.y)
        {   
            lowerLeg.transform.localPosition += new Vector3(movementVectorX, movementVectorY, movementVectorZ) * Time.deltaTime * timeVariable;
            upperLeg.transform.Rotate(Vector3.forward * 20 * Time.deltaTime * rotateVariable);
            strut.transform.Rotate(-Vector3.back * 10 * Time.deltaTime *  rotateVariable);
            foot.transform.localPosition += new Vector3(movementVectorX, movementVectorY, movementVectorZ) * Time.deltaTime * timeVariable; 
        }
        
    }
}
