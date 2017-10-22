using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {
    public Vector3 velocity { get; private set; }

    Vector3 lastPos;

    void Update() {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    void OnTriggerEnter(Collider other) {
        Interactable interact = other.gameObject.GetComponent<Interactable>();
        if (interact) {
            interact.OnFootEnter(this);
        }
    }
    void OnTriggerExit(Collider other) {
        Interactable interact = other.gameObject.GetComponent<Interactable>();
        if (interact) {
            interact.OnFootExit(this);
        }
    }
}
