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
    [SerializeField] Sequencer a1;
    
    [SerializeField] Sequencer b;
    [SerializeField] List<Sequencer> startchords = new List<Sequencer>();
    [SerializeField] matchPenDrums drums = new matchPenDrums();

    
    void Start()
    {
        for(int i=0;i<a.seq.Count;i++){
            a.seq[i]=false;
            a1.seq[i]=false;
        }
        for(int i=0;i<b.seq.Count;i++){
            b.seq[i]=false;
        }
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
                    s.startingChords = false;
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



        //end of intro/start of main sequence
        if(globalCount == 14 && eventTrig){
            drums.rateOfInstrument = 4;
            drums.count = 0;
            drums.ramp = (1000/(drums.getROS.frequency*drums.rateOfInstrument))-1;
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

            a.sequenceCount = 0;
            a.ramp = a.tempoMs-5;
            a1.sequenceCount = 0;
            a1.ramp = a1.tempoMs-5;
            for(int i=0;i<a.seq.Count;i++){
                a.seq[i] = true;
                a1.notes[i]-=5;
                a1.seq[i] = true;
            }

            b.emitSphere.GetComponent<Renderer>().material.SetFloat("_emitTrans", 1);

            b.sequenceCount = 0;
            b.ramp = b.tempoMs-5;
            for(int i=0;i<b.seq.Count;i++){
                b.notes[i] = b.CNote+b.fastHighNoteSeq1[i];
                if(b.notes[i]==-12){
                    b.seq[i] = false;
                }
                else{
                    b.seq[i] = true;
                }
            }

            eventTrig = false;
        }

        if(globalCount == 16 && eventTrig){
            for(int i=0;i<a1.seq.Count;i++){
                a1.notes[i]+=1;
            }
            eventTrig = false;
        }

        if(globalCount == 18 && eventTrig){
            for(int i=0;i<a1.seq.Count;i++){
                a1.notes[i]+=1;
            }
            eventTrig = false;
        }

        if(globalCount == 20 && eventTrig){
            for(int i=0;i<a1.seq.Count;i++){
                a1.notes[i]+=1;
            }
            eventTrig = false;
        }

        if(globalCount == 22 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=3;
                a1.notes[i]-=2;
            }

            for(int i=0;i<b.seq.Count;i++){
                b.notes[i] = b.CNote+b.fastHighNoteSeq2[i];
                if(b.notes[i]==-12){
                    b.seq[i] = false;
                }
                else{
                    b.seq[i] = true;
                }
            }
            eventTrig = false;
        }
        
        if(globalCount == 26 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]-=4;
                a1.notes[i]-=1;
            }
            eventTrig = false;
        }

        //Both on C
        if(globalCount == 30 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]+=1;
                a1.notes[i]+=5;
            }

            for(int i=0;i<b.seq.Count;i++){
                b.notes[i] = b.CNote+b.fastHighNoteSeq1[i];
                if(b.notes[i]==-12){
                    b.seq[i] = false;
                }
                else{
                    b.seq[i] = true;
                }
            }

            eventTrig = false;
        }

        if(globalCount == 33 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a1.notes[i]-=2;
            }
            eventTrig = false;
        }

        if(globalCount == 34 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a1.notes[i]-=1;
            }
            eventTrig = false;
        }

        if(globalCount == 36 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a1.notes[i]-=1;
            }
            eventTrig = false;
        }
        
        if(globalCount == 38 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.notes[i]-=1;
                a1.notes[i]-=1;
            }
            eventTrig = false;
        }

        if(globalCount == 42 && eventTrig){
            for(int i=0;i<a.seq.Count;i++){
                a.seq[i] = false;
                a1.seq[i] = false;
            }

            for(int i=0;i<b.seq.Count;i++){
                if(i==0||i==1||i==15){
                    b.seq[i] = true;
                }
                else{
                    b.seq[i] = false;
                }
            }
            
            b.notes[0] = b.CNote;
            b.notes[1] = b.CNote;
            b.notes[15] = b.CNote-2;
            
            eventTrig = false;
        }

        if(globalCount == 47 && eventTrig){
            for(int i=0;i<b.seq.Count;i++){
                b.seq[i] = false;
            }
            drums.gates[0][0] = false;
            drums.gates[0][15] = false;
            eventTrig = false;
        }

        if(globalCount == 50 && eventTrig){
            for(int i=0;i<b.seq.Count;i++){
                b.seq[i] = false;
            }
            for(int i=0;i<16;i++){
                drums.gates[0][i] = false;
                drums.gates[1][i] = false;
            }
            eventTrig = false;
        }




    }
}
