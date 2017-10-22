using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : Interactable {
    public string eventLabel = "Dial";
    public bool enableEvent = true;

    public HapticsController.VibrationStrength stepVibration = HapticsController.VibrationStrength.None;

    public int steps = 6;
    

    [System.NonSerialized]
    public int output;

    int laststep;
    int min = 0;
    int max = 1;
    Hand hand;
    
    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryHandTrigger)
            this.hand = hand;
    }
    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryHandTrigger)
            this.hand = null;
    }

    private void Update() {
        if (hand) {
            Vector3 handPos = transform.parent.InverseTransformPoint(hand.transform.position);
            float v = Mathf.Clamp(handPos.z, min, max);

            laststep = output;
            output = (int)(v * steps + .5f);
            if (output != laststep) hand.Vibrate(stepVibration);

        }
    }
}
