using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelKick : Interactable {
    public string eventLabel = "Kick";
    public bool enableEvent = true;
    public float velocityThreshold = 5;

    Vector3 velocityDir = Vector3.down;
    public bool isOpen = false;

    Quaternion defaultRotation;

    public void Activate() {
        isOpen = true;
        defaultRotation = transform.rotation;
        transform.rotation *= Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.Lerp(transform.right, transform.forward, Random.value));
    }
    
    public override void OnFootEnter(Foot foot) {
        Vector3 vel = Vector3.Project(foot.velocity, velocityDir);
        if(vel.magnitude >= velocityThreshold) {
            transform.rotation = defaultRotation;
            isOpen = false;
        }
    }
}
