using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Extinguisher : Interactable {
    public ParticleSystem particle;
    Hand hand;

    FixedJoint handJoint;
    

    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryHandTrigger) {
            if (this.hand == null) {
                this.hand = hand;
                handJoint = gameObject.AddComponent<FixedJoint>();
                handJoint.connectedBody = hand.rbody;
            }
        }

        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            if (hand == this.hand) {
                ParticleSystem.EmissionModule e = particle.emission;
                e.enabled = true;
            }
        }
    }

    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryHandTrigger) {
            if (this.hand == hand) {
                ParticleSystem.EmissionModule e = particle.emission;
                e.enabled = false;
                Destroy(handJoint);
                this.hand = null;
            }
        }
        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            if (hand == this.hand) {
                ParticleSystem.EmissionModule e = particle.emission;
                e.enabled = false;
            }
        }
    }
}
