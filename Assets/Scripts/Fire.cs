using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public string eventLabel = "Fire";
    public bool enableEvent = true;

    public int health = 200;
    public ParticleSystem[] particles;
    public Light fireLight;
    public float minSize = .5f;
    public float maxSize = 1f;
    public float minRate = .5f;
    public float maxRate = 1f;
    public float minIntensity = .5f;
    public float maxIntensity = 1f;
    public float flickerSpeed = 5f;

    float[] defaultRates;
    float[] defaultSizes;

    int maxHealth;
    float flickerTime = 0;
    public bool isRaging { get; private set; }
    BoxCollider box;
    AudioSource audioLoop;

    void Start() {
        maxHealth = health;
        box = GetComponent<BoxCollider>();
        audioLoop = GetComponent<AudioSource>();
        defaultRates = new float[particles.Length];
        defaultSizes = new float[particles.Length];
        int i = 0;
        foreach (ParticleSystem particle in particles) {
            ParticleSystem.EmissionModule emit = particle.emission;
            defaultRates[i] = emit.rateOverTime.constant;
            ParticleSystem.MainModule main = particle.main;
            defaultSizes[i] = main.startSize.constant;
            i++;
        }
        EventDeactivate();
    }

    public void EventActivate() {
        box.enabled = true;
        fireLight.enabled = true;
        health = maxHealth;
        isRaging = true;
        audioLoop.Play();
        foreach (ParticleSystem particle in particles) {
            ParticleSystem.EmissionModule emit = particle.emission;
            emit.enabled = true;
        }
        AnimateFlames();
    }
    void EventDeactivate() {
        box.enabled = false;
        fireLight.enabled = false;
        isRaging = false;
        audioLoop.Stop();
        foreach (ParticleSystem particle in particles) {
            ParticleSystem.EmissionModule emit = particle.emission;
            emit.enabled = false;
        }
    }

    void Update() {
        float healthpercent = health / (float)maxHealth;
        flickerTime += Time.deltaTime * healthpercent * flickerSpeed;
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, healthpercent) + Mathf.PerlinNoise(flickerTime, 0f);
    }

    void AnimateFlames() {
        float healthpercent = health / (float)maxHealth;
        int i = 0;
        foreach (ParticleSystem particle in particles) {
            ParticleSystem.EmissionModule emit = particle.emission;
            emit.rateOverTime = new ParticleSystem.MinMaxCurve(defaultRates[i] * Mathf.Lerp(minRate, maxRate, healthpercent));
            ParticleSystem.MainModule main = particle.main;
            main.startSize = new ParticleSystem.MinMaxCurve(defaultSizes[i] * Mathf.Lerp(minSize, maxSize, healthpercent));
            i++;
        }
    }

    void OnParticleCollision(GameObject other) {
        if (other.GetComponent<Extinguisher>()) {
            health--;
            AnimateFlames();
          
            if (health <= 0)
                EventDeactivate(); 
        }
    }
}
