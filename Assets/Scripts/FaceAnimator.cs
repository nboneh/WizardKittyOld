using UnityEngine;
using System.Collections;

public class FaceAnimator : MonoBehaviour {

    public GameObject eyeTarget;
    public GameObject eyeBase;

    public GameObject mouthTarget;
    public GameObject mouthBase;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //eyes
        float xshift = Mathf.Abs(eyeTarget.transform.localPosition.x - eyeBase.transform.localPosition.x);
        if (xshift < 0.025f)
            xshift = 0.25f;
        else if (xshift > 0.025f && xshift < 0.075f)
            xshift = 0.5f;
        else if (xshift > 0.075f && xshift < 0.125f)
            xshift = 0.75f;
        else
            xshift = 0f;

        float yshift = Mathf.Abs(eyeTarget.transform.localPosition.z - eyeBase.transform.localPosition.z);

        GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex", new Vector2(xshift, 0f));

        //mouth
        xshift = Mathf.Abs(mouthTarget.transform.localPosition.x - mouthBase.transform.localPosition.x);
        if (xshift < 0.025f)
            xshift = 0.25f;
        else if (xshift > 0.025f && xshift < 0.075f)
            xshift = 0.5f;
        else if (xshift > 0.075f && xshift < 0.125f)
            xshift = 0.75f;
        else
            xshift = 0f;

        yshift = Mathf.Abs(eyeTarget.transform.localPosition.z - eyeBase.transform.localPosition.z);

        GetComponent<Renderer>().materials[2].SetTextureOffset("_MainTex", new Vector2(xshift, 0f));
    }
}
