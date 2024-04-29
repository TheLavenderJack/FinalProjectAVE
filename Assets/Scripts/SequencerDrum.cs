using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerDrum : MonoBehaviour
{
    public LibPdInstance patch;
    float ramp;
    float t;
    int[] mode;
    int count = 0;
    

    [SerializeField]
    List<bool> kick;
    [SerializeField]
    List<bool> snare;
    [SerializeField]
    List<bool> sticks;
    [SerializeField]
    int beatMs;
    public List<AudioClip> sounds;
    string[] drum_type = new string[] { "Kick", "Snare", "Sticks" };
    List<float> envelopes = new List<float>();
    List<bool>[] gates = new List<bool>[3];
    List<Vector4> adsr_params = new List<Vector4>();
    GameObject kickObj; 
    GameObject snareObj;
    [SerializeField]
    GameObject stickObj;

    void Start()
    {
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
        adsr_params.Add(new Vector4(10, 100, 0, 0));
        adsr_params.Add(new Vector4(10, 200, 0, 0));
        adsr_params.Add(new Vector4(5, 50, 0, 0));
        kickObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        snareObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stickObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);


        bool trig = ramp > ((ramp + dMs) % beatMs);
        ramp = (ramp + dMs) % beatMs;

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
                envelopes[i] = ControlFunctions.ADSR(ramp/1000, gates[i][kick.Count-1], adsr_params[i]);
            }
            else{
                envelopes[i] = ControlFunctions.ADSR(ramp/1000, gates[i][count-1], adsr_params[i]);
            }
           
        }
        kickObj.transform.localPosition = new Vector3(0, envelopes[0]*2, 0);
        snareObj.transform.localPosition = new Vector3(5, envelopes[1]*2, 0);
        stickObj.transform.localPosition = new Vector3(-5, envelopes[2]*2, 0);
    }
}
