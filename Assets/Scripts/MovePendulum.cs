using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePendulum : MonoBehaviour
{
    public float frequency;
    [Range(0f,1f)]
    [SerializeField] public float amplitude = 0.25f;
    [SerializeField] ConductorScript conductor;
    float t;
    float prev_oscillation;
    float delta;
    float prev_delta;
    bool left_edge, right_edge, middle;

    // Update is called once per frame
    void FixedUpdate()
    {
        frequency = conductor.tempoMs/1000;
        t += Time.deltaTime;
        //triangle function 
        float oscillation = ControlFunctions.Tri(t, frequency, 0.5f) * amplitude;
        
        //fake gravity
        float sign = Mathf.Sign(oscillation);
        //need to work with absolute value to avoid sqrt error
        oscillation = Mathf.Sqrt(Mathf.Abs(oscillation));
       //reapply sign
        oscillation *= sign;
        //added some smoothing to get rid of discontinuity at middle point
        oscillation = Mathf.Lerp(oscillation, prev_oscillation, 0.85f);
        //get the derivative of the motion
        delta = oscillation - prev_oscillation;
        
        //apply rotation
        transform.localRotation = Quaternion.Euler(0, 0, oscillation*180 );

        //stages (this would probably work better with a switch, I am just not that familiar with it...)
        if (Mathf.Sign(delta) != Mathf.Sign(prev_delta))
        {
            left_edge = right_edge = middle = false;
            if (delta > prev_delta)
                left_edge = true;
            else if (delta < prev_delta)
                right_edge = true;
            
        }
        else if (Mathf.Sign(oscillation) != Mathf.Sign(prev_oscillation))
        {
            left_edge = right_edge = middle = false;
            middle = true;
        }
            

        //update delta for derivative
        prev_delta = delta;
        //upate previous oscillaltion
        prev_oscillation = oscillation;
    }
}