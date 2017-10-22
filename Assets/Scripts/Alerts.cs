using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Alerts : MonoBehaviour {
    public List<Light> lights = new List<Light>();
    public Light mainLight;
    public Slider slider;
    public float rotateSpeed = 90f;

    AudioSource audioSrc;

    void Start() {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update () {
        transform.rotation *= Quaternion.Euler(0, Time.deltaTime * rotateSpeed, 0);
        lights.ToList().ForEach(l =>{
            l.color = slider.output == 1 ? Color.yellow : slider.output == 2 ? Color.red : Color.white;
            l.enabled = slider.output != 0;
        });
        mainLight.intensity = slider.output != 0 ? .25f : .5f;

        if (slider.output == 0)
            audioSrc.Stop();
        else if (!audioSrc.isPlaying)
            audioSrc.Play();
	}
}
