using UnityEngine;
using System.Collections;

public class TreeSpawn : MonoBehaviour {
    float scaleY = 0;
    bool spawning = true;
    AudioSource source;
    public AudioClip spawnSound;
    public AudioClip despawnSound;
    // Use this for initialization
    void Start () {
        this.transform.localScale = new Vector3(this.transform.localScale.x, scaleY, this.transform.localScale.z);
        source = GetComponent<AudioSource>();
        source.PlayOneShot(spawnSound);
    }
	
	// Update is called once per frame
	void Update () {
	    if(spawning && scaleY < 3)
        {
            scaleY += Time.deltaTime*2;
            if (scaleY > 3)
            {
                scaleY = 3;
            }

        }
    
        if (!spawning && scaleY > 0)
        {
            scaleY -= Time.deltaTime * 2;
            if (scaleY < 0)
            {
                Destroy(gameObject);
            }
        }

        this.transform.localScale = new Vector3(this.transform.localScale.x, scaleY, this.transform.localScale.z);
    }

    public void Despawn()
    {
        spawning = false;
        source.PlayOneShot(despawnSound);
    }
}
