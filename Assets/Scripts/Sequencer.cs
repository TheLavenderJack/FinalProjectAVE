using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public LibPdInstance patch;
    [SerializeField] float ramp = 250;
    float t;
    int[] mode;
    [SerializeField] public List<bool> seq;
    [SerializeField] public int[] notes; //each int should have a range of 0 to the length of mode+1
    int CNote = 48;

    [SerializeField] bool fastHighNS = false;
    [SerializeField] bool baseTempo = true;
    [SerializeField] bool startingChords=false;
    int octave = 12;

    [SerializeField] ConductorScript conductor;
    int sequenceCount;
    int tempoMs;
    //delay variables
    bool delay;
    int delayTime;
    float delayRamp = 0;
    [SerializeField] int chordID = 0;


    // Start is called before the first frame update
    void Start()
    {   
        //setting sequences of notes
        int[] fastHighNoteSeq = {0, 7, 7, 4, 0, 0, 0, 0, 2, 4, 2, 0, 0, -5, -3, -5, 0};
        int[] sSeq1 = {0,-2,-4,-5};
        int[] sSeq2 = {3,3,0,-1};
        int[] sSeq3 = {7,7,3,2};
        int[] sSeq4 = {10,10,7,5};

        notes = new int[seq.Count];
        //base set of notes (all middle C) 
        for(int i=0;i<seq.Count;i++){
          notes[i] = CNote;
        }
        //fastHighNS notes
        if(fastHighNS){
            for(int i=0;i<seq.Count;i++){
                notes[i] = CNote+octave+fastHighNoteSeq[i];
            }
            delay=true;
            delayTime = conductor.delayUntilMain+750;
        }
        else if(startingChords){
            if(chordID==1){
                for(int i=0;i<seq.Count;i++){
                    notes[i] = CNote+sSeq1[i];
                }
            }
            else if(chordID==2){
                for(int i=0;i<seq.Count;i++){
                    notes[i] = CNote+sSeq2[i];
                }
            }
            else if(chordID==3){
                for(int i=0;i<seq.Count;i++){
                    notes[i] = CNote+sSeq3[i];
                }
            }
            else if(chordID==4){
                for(int i=0;i<seq.Count;i++){
                    notes[i] = CNote+sSeq4[i];
                }
            }
        }
        else if(baseTempo){
            delay=true;
            delayTime = conductor.delayUntilMain;
        }



    }

    // Update is called once per frame
    void Update()
    {
        //tempo Control (based on the conductor's tempo)
        if(baseTempo){
            tempoMs = conductor.tempoMs;
        }
        else if(fastHighNS){
            tempoMs = conductor.tempoMs/4;
        }
        else if(startingChords){
            tempoMs = (int)(conductor.tempoMs*2.5f);
        }
        else{
            tempoMs = conductor.tempoMs;
        }
        
        //time tracker
        t += Time.deltaTime;
        int deltaMs = Mathf.RoundToInt(Time.deltaTime * 1000);

        //delay control
        if(delay){
            ramp = 0;
            //setting up delay for the first note to come in 
            bool delayTrig = delayRamp > (delayRamp + deltaMs) % delayTime;
            delayRamp = (delayRamp + deltaMs) % delayTime;
            if(delayTrig){
                delay = false;
            }
        }

        //controls ramp for the specific sequencer
        bool trig = ramp > (ramp + deltaMs) % tempoMs;
        ramp = (ramp + deltaMs) % tempoMs;

        
        if(trig){
            if(sequenceCount == seq.Count){
                sequenceCount = 0;
            }
            if(seq[sequenceCount]){
                patch.SendMidiNoteOn(0, notes[sequenceCount], 80);
            }
            Debug.Log(sequenceCount);
            sequenceCount++;
        } 
    }
}