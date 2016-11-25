using UnityEngine;
using System.Collections;

public class StaffScript : MonoBehaviour {

    enum Spell { Ice, Fire, Nature };
    enum State { Dimming, Turnup, Normal};

    float changeRate = 15f;
    public Light halo;
    Color color;
    float intensity = 1.0f;
    State state;
    Spell spell;

    public ParticleSystem Fire;
    public ParticleSystem Ice;
    public ParticleSystem Leaf;
    public GameObject treeAim;
    public TreeSpawn treeSpawn;
    TreeSpawn currentTreeSpawn;
    GameObject currentTreeAim;

    ParticleSystem nextParticleSystem;
    ParticleSystem currentParticleSystem;

    public Camera followCamera;

    public GameObject fireBall;
    AudioSource source;

    public AudioClip fireInitSound;
    public AudioClip iceInitSound;
    public AudioClip natureInitSound;

    public AudioClip flameAttack;

    // Use this for initialization
    void Start () {
        SetSpell(Spell.Ice);
        state = State.Turnup;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        float t = Time.deltaTime;
        if (Input.GetKeyDown("1"))
        {
            SetSpell(Spell.Ice);
        }
        else if (Input.GetKeyDown("2"))
        {
            SetSpell(Spell.Fire);
        }
        else if (Input.GetKeyDown("3"))
        {
            SetSpell(Spell.Nature);
        }

        switch (state)
        {
            case State.Dimming:
                intensity -= t * changeRate;
                if (intensity <= 0.0f)
                {
                    intensity = 0.0f;
                    state = State.Turnup;
                    GetComponent<Light>().color = color;
                    halo.color = color;
                    switch (spell)
                    {
                        case Spell.Ice:
                            source.PlayOneShot(iceInitSound);
                            break;
                        case Spell.Fire:
                            source.PlayOneShot(fireInitSound);
                            break;
                        case Spell.Nature:
                            source.PlayOneShot(natureInitSound);
                            break;

                    }
                }
                GetComponent<Light>().intensity = intensity;
                halo.intensity = intensity/5.0f;
                break;
            case State.Turnup:
                intensity += t * changeRate;
                if (intensity >= 5.0f)
                {
                    if (currentParticleSystem != null)
                        DestroyImmediate(currentParticleSystem.gameObject);
                    intensity = 5.0f;
                    state = State.Normal;
                    currentParticleSystem = (ParticleSystem)Instantiate(nextParticleSystem, new Vector3(0, 0, 0), Quaternion.identity);
                    currentParticleSystem.transform.parent = transform;
                    if (spell == Spell.Nature)
                        currentParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                    else
                        currentParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)); currentParticleSystem.transform.localPosition = new Vector3(0, 0, 0);
                    currentParticleSystem.transform.localScale = new Vector3(1, 1, 1);
               

                }
                GetComponent<Light>().intensity = intensity;
                halo.intensity = intensity/5.0f;
                break;
        }

        if (Input.GetMouseButtonDown(0))
            CastSpell();

        if (Input.GetMouseButton(0))
        {
            if(spell == Spell.Nature)
            {
                RaycastHit hit;
                Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.tag == "Floor")
                        currentTreeAim.transform.position = hit.point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (spell == Spell.Nature)
            {
                if (currentTreeSpawn != null)
                    currentTreeSpawn.Despawn();
                currentTreeSpawn = (TreeSpawn)Instantiate(treeSpawn, currentTreeAim.transform.position, Quaternion.identity);
                if (currentTreeAim != null)
                    Destroy(currentTreeAim.gameObject);
            }
        }
    }

    void CastSpell()
    {
        switch (spell)
        {
            case Spell.Ice:
            
                break;
            case Spell.Fire:
                GameObject fBall = (GameObject)Instantiate(fireBall, transform.position, Quaternion.identity);
                Vector3 direction = Quaternion.AngleAxis(-10, followCamera.transform.right) * Quaternion.AngleAxis(0, followCamera.transform.up) * followCamera.transform.forward;
                fBall.GetComponent<Rigidbody>().velocity = direction *12;
                fBall.transform.rotation = Quaternion.LookRotation(direction);
                fBall.gameObject.tag = "Fire";
                fBall.transform.rotation = Quaternion.Euler(new Vector3(fBall.transform.rotation.eulerAngles.x - 90, fBall.transform.rotation.eulerAngles.y, fBall.transform.rotation.eulerAngles.z));
                source.PlayOneShot(flameAttack);
                break;
            case Spell.Nature:
                currentTreeAim = (GameObject)Instantiate(treeAim, new Vector3(0, 0, 0), Quaternion.identity);
                break;
        }
    }

    void SetSpell(Spell spell)
    {
        Light lt = GetComponent<Light>();
        if(currentTreeAim != null)
             Destroy(currentTreeAim.gameObject);
        if (currentParticleSystem != null)
        {
           currentParticleSystem.loop = false;
        }
        switch (spell)
        {
            case Spell.Ice:
                color = new Color(33/255f, 249/255f, 255/255f);
                nextParticleSystem = Ice;
                break;
            case Spell.Fire:
                color = new Color(255/255f,20/255f,49/255f);
                nextParticleSystem = Fire;
                break;
            case Spell.Nature:
                color = new Color(100/255f, 232/255f, 37/255f);
                nextParticleSystem = Leaf;
                break;

        }
        this.spell = spell;
        state = State.Dimming;

    }
}
