using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimbreGame_fountain : MonoBehaviour {
    public List<FountainValues> fountainValues;
    private ParticleSystem ps;
    private float minGravity = 15.5f;
    private float maxGravity = 0.56f;
    private float minLifeTime = 0.26f;
    private float maxLifeTime = 2.9f;


    void Start () {
        ps = GetComponent<ParticleSystem>();
        SetPower(0);

    }
	
    public void SetPower(int power)
    {
        ps.startLifetime = fountainValues[power].lifetime;
        ps.gravityModifier = fountainValues[power].gravity;;
    }

	void Update () {
		
	}

    public float GetMaxPosition()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);

        float maxY = -100.0f;
        foreach(ParticleSystem.Particle particle in particles)
        {
            if (particle.position.y > maxY)
            {
                maxY = particle.position.y;
            }
        }

        return maxY;   
    }

    [System.Serializable]
    public struct FountainValues
    {
        public float lifetime;
        public float gravity;
    }
}
