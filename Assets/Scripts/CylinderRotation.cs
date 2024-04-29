using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderRotation : MonoBehaviour
{
    public LibPdInstance pdPatch;
    public Transform obj;
    // Start is called before the first frame update
    void Start()
    {
        pdPatch.Bind("AmpRead");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FloatReceive(string sender, float value)
	{
		//This function will get called for *every* Float event sent by our
		//patch, so we need to make sure we're only acting on the
		//*AmplitudeEnvelope* event that we're actually interested in.
		if (sender == "AmpRead")
		{
            Debug.Log(value);
			obj.transform.Rotate(obj.rotation.x, obj.rotation.y, value);
		}
	}
}
