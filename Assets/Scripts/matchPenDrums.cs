using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchPenDrums : MonoBehaviour
{
    public LibPdInstance patch;
    [SerializeField]
    public float ramp = 500;
    float t;
    int[] mode;
    public int count = 0;

    [SerializeField]
    public float rateOfInstrument = 1;
    

    [SerializeField]
    public List<bool> kick;
    [SerializeField]
    List<bool> snare;
    [SerializeField]
    List<bool> sticks;
    public List<AudioClip> sounds;
    string[] drum_type = new string[] { "Kick", "Snare", "Sticks" };
    public List<float> envelopes = new List<float>();
    public List<bool>[] gates = new List<bool>[3];
    List<Vector4> adsr_params = new List<Vector4>();
    GameObject kickObj; 
    GameObject snareObj;
    GameObject stickObj;
    
    bool delay;
    int delayTime;
    float delayRamp = 0;

    [SerializeField]
    MovePendulum getROS;

    void Start()
    {
        //start up delay for the drums
        delay = true;
        delayTime = 250;
        ramp = ramp*getROS.frequency;
        for(int i = 0; i < sounds.Count; i++)
        {
            //send sound files names to patch
            //add .wav
            //drum type is both the name of receive obj 
            //and of Drums folder subdirectory for sound
            string name = sounds[i].name + ".wav";
            patch.SendSymbol(drum_type[i], name);
            //build list of envelopes
            envelopes.Add(1);
        }
        gates[0] = kick;
        gates[1] = snare;
        gates[2] = sticks;
        adsr_params.Add(new Vector4(100, 100, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);

        //delay setup
        if(delay){
            ramp = 0;
            //setting up delay for the first note to come in 
            bool delayTrig = delayRamp > (delayRamp + dMs) % delayTime;
            delayRamp = (delayRamp + dMs) % delayTime;
            if(delayTrig){
                delay = false;
            }
        }

        bool trig = ramp > ((ramp + dMs) % (1000/(getROS.frequency*rateOfInstrument)));
        ramp = (ramp + dMs) % (1000/(getROS.frequency*rateOfInstrument));

        if (trig)
        {
            if (kick[count])
            {
                patch.SendBang("kick_bang");
            } 
            if (snare[count])
            {
                patch.SendBang("snare_bang");
            }
            if (sticks[count])
            {
                patch.SendBang("sticks_bang");
            }
            count = (count + 1) % kick.Count;
        }
        
        
        for (int i = 0; i < sounds.Count; i++){
            if(count==0){
                envelopes[i] = ControlFunctions.ADSR(ramp/1000, gates[i][kick.Count-1], adsr_params[0]);
            }
            else{
                envelopes[i] = ControlFunctions.ADSR(ramp/1000, gates[i][count-1], adsr_params[0]);
            }
            
        }

    }
}
