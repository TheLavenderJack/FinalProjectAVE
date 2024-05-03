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

    public int delayUntilMain;

    [SerializeField] Sequencer a;
    [SerializeField] Sequencer b;
    [SerializeField] List<Sequencer> startchords = new List<Sequencer>();
    [SerializeField] matchPenDrums drums = new matchPenDrums();

    
    void Start()
    {
        //time before main start
        delayUntilMain = 15750;

        
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
            Debug.Log(globalCount);
        }
        
        //stop starting chords
        if(globalCount == 9 && eventTrig){
            foreach(Sequencer s in startchords){
                for(int i=0;i<4;i++){
                    s.seq[i] = false;
                }
            }
            eventTrig = false;
        }
        
        //bring in starting drums
        if(globalCount == 10 && eventTrig){
            drums.count = 0;
            drums.rateOfInstrument = 8;
            drums.gates[0][0] = true;
            drums.gates[0][4] = true;
            drums.gates[0][8] = true;
            drums.gates[0][12] = true;
            eventTrig = false;
        }
        if(globalCount == 12 && eventTrig){
            drums.gates[0][2] = true;
            drums.gates[0][6] = true;
            drums.gates[1][9] = true;
            drums.gates[0][10] = true;
            drums.gates[1][11] = true;
            drums.gates[0][12] = true;
            drums.gates[1][12] = true;
            drums.gates[1][13] = true;
            drums.gates[0][14] = true;
            drums.gates[1][14] = true;
            drums.gates[1][15] = true;
            eventTrig = false;
        }

        if(globalCount == 14 && eventTrig){
            drums.rateOfInstrument = 4;
            for(int i=0;i<16;i++){
                drums.gates[0][i] = false;
                drums.gates[1][i] = false;
            }
            drums.gates[0][0] = true;
            drums.gates[0][1] = true;
            drums.gates[1][2] = true;
            drums.gates[0][3] = true;
            drums.gates[0][4] = true;
            drums.gates[0][5] = true;
            drums.gates[1][7] = true;
            drums.gates[0][9] = true;
            drums.gates[1][10] = true;
            drums.gates[0][11] = true;
            drums.gates[0][12] = true;
            drums.gates[1][12] = true;
            drums.gates[0][14] = true;
            drums.gates[1][14] = true;
            drums.gates[0][15] = true;

            eventTrig = false;
        }


        //note changes for main melody
        if(globalCount == 16 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=4;
            }
            eventTrig = false;
        }

        if(globalCount == 20 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=1;
            }
            eventTrig = false;
        }
        if(globalCount == 24 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]-=5;
            }
            eventTrig = false;
        }


    }
}
