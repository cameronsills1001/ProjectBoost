using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Oscilate();

    }

    void Oscilate()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cylces = Time.time / period; //continually growning over time
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cylces * tau); //going from -1 to 1
        Debug.Log(rawSinWave);

        movementFactor = (rawSinWave + 1f) / 2f; //recalculated to go from 0 to 1 so it's cleaner

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
