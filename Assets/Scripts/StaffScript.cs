using UnityEngine;
using System.Collections;

public class StaffScript : MonoBehaviour {

    public enum Spell { Ice=0, Fire=1, Nature=2, Lightning=3 };
    public Color[] spellColors = new Color[4];
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
    public ParticleSystem Lightning;
    public GameObject treeAim;
    public TreeSpawn treeSpawn;
    public Spell initialSpell;

    TreeSpawn currentTreeSpawn;
    GameObject currentTreeAim;

    ParticleSystem nextParticleSystem;
    ParticleSystem currentParticleSystem;

    public Camera followCamera;

    public GameObject fireBall;
    public ParticleSystem iceAttack;
    AudioSource source;

    public AudioClip fireInitSound;
    public AudioClip iceInitSound;
    public AudioClip natureInitSound;
    public AudioClip lightningInitSound;

    public AudioClip flameAttack;

    public bool keyboardInputEnable;
    public bool soundEnabled;

    // Use this for initialization
    void Start () {
        spellColors[(int)Spell.Ice] = new Color(33 / 255f, 249 / 255f, 255 / 255f);
        spellColors[(int)Spell.Fire] = new Color(255 / 255f, 20 / 255f, 49 / 255f);
        spellColors[(int)Spell.Nature] = new Color(100 / 255f, 232 / 255f, 37 / 255f);
        spellColors[(int)Spell.Lightning] = new Color(253 / 255f, 208 / 255f, 35 / 255f);
        SetSpell(initialSpell);
        state = State.Turnup;
        halo.color = spellColors[(int)initialSpell];
        GetComponent<Light>().color = spellColors[(int)initialSpell];

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        float t = Time.deltaTime;
        if (keyboardInputEnable)
            KeyboardInput();
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
                    if (soundEnabled)
                    {
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
                            case Spell.Lightning:
                                source.PlayOneShot(lightningInitSound);
                                break;

                        }
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
                        currentParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z -90 ));
                    else
                        currentParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)); currentParticleSystem.transform.localPosition = new Vector3(0, 0, 0);
                    currentParticleSystem.transform.localScale = new Vector3(1, 1, 1);
               

                }
                GetComponent<Light>().intensity = intensity;
                halo.intensity = intensity/5.0f;
                break;
        }
    }

    void KeyboardInput()
    {
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
        else if (Input.GetKeyDown("4"))
        {
            SetSpell(Spell.Lightning);
        }

        if (Input.GetMouseButtonDown(0))
            CastSpell();

        if (Input.GetMouseButton(0))
        {
            if (spell == Spell.Nature)
            {
                RaycastHit hit;
                Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Floor")
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
                ParticleSystem ice = (ParticleSystem)Instantiate(iceAttack, this.transform.position, this.transform.rotation);
                ice.transform.parent = this.transform;
                ice.transform.localScale = new Vector3(1, 1, 1);
                break;
            case Spell.Fire:
                Vector3 direction = Quaternion.AngleAxis(-10, followCamera.transform.right) * Quaternion.AngleAxis(0, followCamera.transform.up) * followCamera.transform.forward;
                ShootFireBall(direction);
                break; 
            case Spell.Nature:
                currentTreeAim = (GameObject)Instantiate(treeAim, new Vector3(0, 0, 0), Quaternion.identity);
                break;
        }
    }

    public void ShootFireBall(Vector3 direction)
    {
        GameObject fBall = (GameObject)Instantiate(fireBall, transform.position, Quaternion.identity);
        fBall.GetComponent<Rigidbody>().velocity = direction * 12;
        fBall.transform.rotation = Quaternion.LookRotation(direction);
        fBall.gameObject.tag = "Fire";
        fBall.transform.rotation = Quaternion.Euler(new Vector3(fBall.transform.rotation.eulerAngles.x - 90, fBall.transform.rotation.eulerAngles.y, fBall.transform.rotation.eulerAngles.z));
        if (soundEnabled)
            source.PlayOneShot(flameAttack);
    }

    public void SetSpell(Spell spell)
    {
        Light lt = GetComponent<Light>();
        if(currentTreeAim != null)
             Destroy(currentTreeAim.gameObject);
        if (currentParticleSystem != null)
        {
           currentParticleSystem.loop = false;
        }
        color = spellColors[(int)spell];
        switch (spell)
        {
            case Spell.Ice:
                nextParticleSystem = Ice;
                break;
            case Spell.Fire:
                nextParticleSystem = Fire;
                break;
            case Spell.Nature:
                nextParticleSystem = Leaf;
                break;
            case Spell.Lightning:
                nextParticleSystem = Lightning;
                break;
        }
        this.spell = spell;
        state = State.Dimming;

    }
}
