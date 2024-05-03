using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BottomLight : MonoBehaviour
{
    [SerializeField] bool blue;
    [SerializeField] bool green;
    [SerializeField] bool blueGreen;
    [SerializeField] matchPenDrums drumEnv;
    [SerializeField] Light bL;
    int indexTracker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //setting up the index position correctly (getting th corrct value from matchPenDrums)
        if(drumEnv.count==0){
            indexTracker = drumEnv.kick.Count-1;
        }
        else{
            indexTracker = drumEnv.count-1;
        }


        if(drumEnv.gates[0][indexTracker]&&!drumEnv.gates[1][indexTracker]&&blue){
            bL.intensity = drumEnv.envelopes[0];
        }
        else if(!drumEnv.gates[0][indexTracker]&&drumEnv.gates[1][indexTracker]&&green){ 
            bL.intensity = drumEnv.envelopes[1];
        }
        else if(drumEnv.gates[0][indexTracker]&&drumEnv.gates[1][indexTracker]){
            bL.intensity = drumEnv.envelopes[0];
        }

        

    }
}
