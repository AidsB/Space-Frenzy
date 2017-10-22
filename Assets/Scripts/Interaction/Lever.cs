using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable {
    public string eventLabel = "Lever";
    public bool enableEvent = true;

    public HapticsController.VibrationStrength stepVibration = HapticsController.VibrationStrength.None;
    public float minAngle = -25;
	public float maxAngle = 25;
	[System.NonSerialized]
	public int output; // between 0 and 1
	public int steps = 2;

    int lastStep = 0;

    Hand hand;

    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryIndexTrigger)
            this.hand = hand;
    }
    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryIndexTrigger && this.hand == hand)
            this.hand = null;
    }

    void Update () {
        if (hand) {
            Vector2 localhand = transform.parent.InverseTransformPoint(hand.transform.position);
            float theta = -Mathf.Atan2(localhand.x, localhand.y) * Mathf.Rad2Deg + 90f;
            theta = (Mathf.Clamp(theta, minAngle, maxAngle) - minAngle) / (maxAngle - minAngle);

            output = (int)(theta * (steps - 1) + .5f);
            if (output != lastStep) hand.Vibrate(stepVibration);
            lastStep = output;

            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minAngle, maxAngle, output / (float)(steps - 1)));
        }
	}
}
