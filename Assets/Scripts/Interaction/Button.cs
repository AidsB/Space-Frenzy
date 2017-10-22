using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable {
    public string eventLabel = "Button";
    public bool enableEvent = true;

    public HapticsController.VibrationStrength clickVibration = HapticsController.VibrationStrength.None;
    public float displacement = .3f;
    public bool state = false;
    
    public override void InteractClick(Hand hand, OVRInput.Button button){
        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            hand.Vibrate(clickVibration);
            state = !state;
            transform.localPosition -= displacement * Vector3.up;
        }
    }

    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if(button == OVRInput.Button.PrimaryIndexTrigger) {
            transform.localPosition += displacement * Vector3.up;
        }
    }
}
