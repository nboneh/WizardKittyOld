using UnityEngine;
using System.Collections;

public class MainLogo : MonoBehaviour {

    float floatCounter = 0;
    float floatTime = 1;
    bool goUp = true; 
	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	    float t = Time.deltaTime;
        if (transform.localScale.x < 1)
        {
            transform.localScale += new Vector3(.3f * t, .3f * t, .3f * t);
            transform.Translate(Vector3.up * 1.2f * Time.deltaTime);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            if(goUp)
                transform.Translate(Vector3.up * Time.deltaTime);
            else
                transform.Translate(Vector3.up * -1 * Time.deltaTime);

            floatCounter += t;
            if(floatCounter >= floatTime)
            {
                goUp = !goUp;
                floatCounter = 0;
            }
        }


    }
}
