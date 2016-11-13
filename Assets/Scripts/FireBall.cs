using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public GameObject tempFire;
    float lifeTime = 4;
    float counter = 0; 
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () { 
	    counter += Time.deltaTime;
        if(counter> lifeTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "kitty")
        {
            
            Instantiate(tempFire, transform.position + transform.up * .25f , Quaternion.Euler(new Vector3(-90, 0, 0)));
            Destroy(gameObject);
        }
    }
}
