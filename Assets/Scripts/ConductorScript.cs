using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ConductorScript : MonoBehaviour
{
    [SerializeField] public int globalCount = 0;
    bool eventTrig = false;
    [SerializeField] public int tempoMs = 1000;
    [SerializeField] float ramp = 250;
    float t;

    [SerializeField] Sequencer a;
    [SerializeField] Sequencer b;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //basic time keeping
        t += Time.deltaTime;
        int deltaMs = Mathf.RoundToInt(Time.deltaTime * 1000);

        bool trig = ramp > (ramp + deltaMs) % tempoMs;
        ramp = (ramp + deltaMs) % tempoMs;

        if(trig){
            globalCount++;
            eventTrig = true;
        }


        //note changes
        if(globalCount == 8 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=4;
            }
            eventTrig = false;
        }

        if(globalCount == 12 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=1;
            }
            eventTrig = false;
        }
        if(globalCount == 16 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]-=5;
            }
            eventTrig = false;
        }


    }
}
