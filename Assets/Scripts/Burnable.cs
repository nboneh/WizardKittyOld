using UnityEngine;
using System.Collections;

public class Burnable : MonoBehaviour {

    public GameObject Fire;
    bool burning = false;
    Material[] materials;
    Vector3 initialScale;
    Vector3 fireInitialScale;
    GameObject fireS;
    float scalefactor = 0f;
    float colorDec = 0;
    float destroyTimer = 0f;
    private object burnlock = new object();
    // Use this for initialization
    void Start () {
        materials = GetComponent<MeshRenderer>().materials;
        initialScale = transform.parent.gameObject.transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime;
        if (burning) {
            transform.parent.gameObject.transform.localScale = new Vector3((1- (scalefactor)/2)* initialScale.x, (1-scalefactor) * initialScale.y, (1 - (scalefactor) / 2) * initialScale.z);
            fireS.transform.localScale = new Vector3((1 - (scalefactor) / 2) * fireInitialScale.x, (1 - (scalefactor) / 2) * fireInitialScale.y, (1 - scalefactor) * fireInitialScale.z);
            foreach (Material material in materials)
            {
                material.color = new Color(material.color.r - colorDec, material.color.g - colorDec, material.color.b - colorDec);
            }
            if (scalefactor < 1f)
                scalefactor += t / 3;
            else
                destroyTimer += t;

            if (destroyTimer > 1.0f)
                Destroy(transform.parent.gameObject);

            if (colorDec < 1f)
                colorDec += t /100;
        }
    }

    void Burn()
    {
        lock (burnlock)
        {
            if (burning)
                return;
            burning = true;
            fireS = (GameObject)Instantiate(Fire, transform.parent.gameObject.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            Renderer renderer = GetComponent<Renderer>();
            Vector3 parentScale = transform.parent.localScale;
            fireS.transform.localScale = new Vector3(parentScale.x * renderer.bounds.size.x * 1.5f, parentScale.y * renderer.bounds.size.y * 1.5f, parentScale.z * renderer.bounds.size.z * 1.5f);
            fireS.transform.parent = transform.parent;
            fireInitialScale = fireS.transform.localScale;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Fire")
            Burn();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fire")
           Burn();
    }
}
