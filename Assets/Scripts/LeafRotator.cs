using UnityEngine;
using System.Collections;

public class LeafRotator : MonoBehaviour {
    Vector3 center;
    float radius = 0f;
    bool setupRot = false;
    // Use this for initialization
    void Start () { 
    }
	
	// Update is called once per frame
	void Update () {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem.particleCount == 0)
            return;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];

        particleSystem.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle p = particles[i];
            if (!setupRot)
            {
                particles[i].rotation = (i / (float)particles.Length) * 360;
            }
            particles[i].position = new Vector3( radius * Mathf.Cos(Mathf.Deg2Rad * p.rotation),0,  radius * Mathf.Sin(Mathf.Deg2Rad * p.rotation));
            particles[i].startSize = radius * .35f;
        }
        setupRot = true;
        particleSystem.SetParticles(particles, particleSystem.particleCount);
        float t = Time.deltaTime;
        if (particleSystem.loop)
        {
            if (radius < .5f)
            {
                radius += t;
                if (radius >= .5f)
                    radius = .5f;
            }
        }

        if (!particleSystem.loop)
        {
            radius -= t;
        }

    }


}
