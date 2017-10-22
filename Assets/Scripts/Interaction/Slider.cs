using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Interactable {
    public string eventLabel = "Slider";
    public bool enableEvent = true;

    public HapticsController.VibrationStrength stepVibration = HapticsController.VibrationStrength.None;
    public float min = 0;
    public float max = 1;
    public int steps = 5;
    
    [System.NonSerialized]
    public int output;

    float offset = 0;

    int laststep;
    Hand hand;

    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            this.hand = hand;
            offset = (transform.parent.InverseTransformPoint(hand.transform.position) - transform.localPosition).z;
        }
    }
    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryIndexTrigger && this.hand == hand)
            this.hand = null;
    }

    private void Update() {
        if (hand) {
            Vector3 handPos = transform.parent.InverseTransformPoint(hand.transform.position);
            handPos.z -= offset;
            float v = max < min ? Mathf.Clamp(handPos.z, max, min) : Mathf.Clamp(handPos.z, min, max);
            v = (v - min) / (max - min); // map to 0-1
            
            laststep = output;
            output = (int)(v * (steps - 1) + .5f);
            if (output != laststep) hand.Vibrate(stepVibration);
            
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(min, max, (float)output / (steps - 1)));
        }
    }
}
