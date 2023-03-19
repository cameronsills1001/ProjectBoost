using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLMovement : MonoBehaviour
{
    //This script is for making an object move in a repeating L shaped pattern along the x and y axes. 
    [SerializeField] float moveSpeed = 2.0f;

    Vector3 startingPosition;
    float objLeftConstraint;
    float objTopConstraint;
    [SerializeField] float objRightConstraint = -5;
    [SerializeField] float objBottomConstraint = 0;

    int stageVariable = 0; //variable to choose which leg of the l shaped journey the object will take

    //pick which method to run based on leg variable


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        objTopConstraint = startingPosition.y;
        objLeftConstraint = startingPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        MoveElevator(stageVariable);
    }

    void MoveElevator(int stageVariable)
    {
        //using a switch to select what stage of movement 
        switch(stageVariable)
        {
            case 0:
                MoveDown();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                MoveLeft();
                break;
            case 3:
                MoveUp();
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
    }
    void MoveDown()
    {
        //move down stage
        if (transform.position.y > objBottomConstraint) 
        {
            transform.position -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
        else
        {
            stageVariable = 1;
        }
        
    }

    void MoveUp()
    {
        //move up stage
        if(transform.position.y < objTopConstraint)
        {
            transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
        else
        {
            stageVariable = 0;
        }
    }

    void MoveRight()
    {   
        //move right stage
        if (transform.position.x < objRightConstraint)
        {
            transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            stageVariable = 2;
        }
    }

    void MoveLeft()
    {
        // move left stage
        if(transform.position.x > objLeftConstraint)
        {
            transform.position -= new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            stageVariable = 3;
        }
    }
   
}
