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

    public GameObject Fire;
    public GameObject Ice;
    public GameObject Leaf;

    GameObject nextParticleSystem;
    GameObject currentParticleSystem;


    // Use this for initialization
    void Start () {
        SetSpell(Spell.Ice);
        state = State.Turnup;
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
                }
                GetComponent<Light>().intensity = intensity;
                halo.intensity = intensity/5.0f;
                break;
            case State.Turnup:
                intensity += t * changeRate;
                if (intensity >= 5.0f)
                {
                    intensity = 5.0f;
                    state = State.Normal;
                    currentParticleSystem = (GameObject)Instantiate(nextParticleSystem, new Vector3(0, 0, 0), Quaternion.identity);
                    currentParticleSystem.transform.parent = transform;
                    currentParticleSystem.transform.localPosition = new Vector3(0, 0, 0);
                    if(spell == Spell.Nature)
                        currentParticleSystem.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x , transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                    else
                        currentParticleSystem.transform.rotation =Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x -90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                    currentParticleSystem.transform.localScale = new Vector3(1, 1, 1);
                }
                GetComponent<Light>().intensity = intensity;
                halo.intensity = intensity/5.0f;
                break;
        }
    }

    void SetSpell(Spell spell)
    {
        Light lt = GetComponent<Light>();
        if (currentParticleSystem != null)
        {
            DestroyImmediate(currentParticleSystem.gameObject);
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
