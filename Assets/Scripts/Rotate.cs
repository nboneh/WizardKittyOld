using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    Vector3 center;
    float radius = 0;
    public float currentAngle;
	// Use this for initialization
	void Start () {
        center = this.transform.localPosition;
        radius = -currentAngle / 220;
        this.transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime;
        currentAngle += t * 60;
        if(radius < 1)
        {
            radius += t*3;
            if(radius >=1)
                radius = 1;
            if (radius >= 0)
                this.transform.localScale = new Vector3(radius, radius, radius);
   
        }

        if (radius >= 0)
        {
            this.transform.localPosition = new Vector3(center.x + radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle), center.y, center.z + radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle));
        }

        

    }
}
