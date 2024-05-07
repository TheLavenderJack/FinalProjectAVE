using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using EZCameraShake;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public LibPdInstance patch;
    [SerializeField] CameraShaker cameraShaker;
    [SerializeField] public float ramp = 250;
    float t;
    int[] mode;
    [SerializeField] public List<bool> seq;
    [SerializeField] public int[] notes; //each int should have a range of 0 to the length of mode+1
    public int CNote = 48;

    [SerializeField] bool fastHighNS = false;
    [SerializeField] bool baseTempo = true;
    [SerializeField] public bool startingChords=false;
    int octave = 12;

    [SerializeField] ConductorScript conductor;
    public int sequenceCount;
    public int tempoMs;
    //delay variables
    bool delay;
    int delayTime;
    float delayRamp = 0;
    [SerializeField] int chordID = 0;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject hanger;
    [SerializeField] GameObject mount;
    bool opacityBool = false;
    float opacityTime;
    float ballStartVal = 0;
    float hangerStartVal = 0;

    [SerializeField] GameObject pend;
    [SerializeField] GameObject anchor;
    [SerializeField] public GameObject emitSphere;


    public int[] fastHighNoteSeq1 = {0, -60, 0, -60, 0, 2, 3, 7, -60, 5, 3, 5, -60, 3, 2, 0};
    public int[] fastHighNoteSeq2 = {-2, 0, -60, -2, 0, -60, -2, 3, -60, 2, -60, -2, -60, -5, -5, -2};


    // Start is called before the first frame update
    void Start()
    {   
        //setting sequences of notes
        
        
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
                notes[i] = CNote+fastHighNoteSeq1[i];
            }
            delay=true;
            delayTime = 0;
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
            delayTime = 0;
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
        // if(delay){
        //     ramp = 0;
        //     //setting up delay for the first note to come in 
        //     bool delayTrig = delayRamp > (delayRamp + deltaMs) % delayTime;
        //     delayRamp = (delayRamp + deltaMs) % delayTime;
        //     if(delayTrig){
        //         delay = false;
        //     }
        // }

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
            sequenceCount++;
        }

        
        if(trig&&startingChords&&ballStartVal<1){
            cameraShaker.ShakeOnce(4f,5f,2.5f,1f);
            mount.GetComponent<Renderer>().material.SetFloat("_trans", 1);
            StartCoroutine(fadeIn25P());
        }

        if(trig&&fastHighNS){
            if(seq[sequenceCount]){
                StartCoroutine(transformSphere());
            }
        }
        
        
    }

    IEnumerator fadeIn25P(){
        float temp1 = ballStartVal;
        float temp2 = hangerStartVal;
        for(float i=ballStartVal;i<ballStartVal+.25f;i+=0.01f){
            ball.GetComponent<Renderer>().material.SetFloat("_trans", i);
            hanger.GetComponent<Renderer>().material.SetFloat("_trans", i);
            yield return new WaitForSeconds(.03f);
        }
        ballStartVal += 0.25f; 
        hangerStartVal += 0.25f;
        StopCoroutine(fadeIn25P());
    }


    IEnumerator transformSphere(){
        yield return new WaitForSeconds(.1f);
        anchor.transform.rotation = Quaternion.Euler(anchor.transform.rotation.x, anchor.transform.rotation.y, pend.transform.rotation.z*94);
        StopCoroutine(transformSphere());
    }
}

