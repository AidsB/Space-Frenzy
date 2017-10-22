using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Hand : MonoBehaviour {
    private static OVRInput.Button[] buttons;
    static Hand() {
        buttons = System.Enum.GetValues(typeof(OVRInput.Button))
            .Cast<OVRInput.Button>()
            .Where(x=> { return x != OVRInput.Button.Any && x != OVRInput.Button.None; })
            .ToArray();
    }

    public OVRInput.Controller controllerMask = OVRInput.Controller.RTouch;
    public Rigidbody rbody { get; private set; }

    OVRInput.Button pressed = OVRInput.Button.None;
    OVRInput.Button pressed_lastframe = OVRInput.Button.None;

    List<Interactable> hover = new List<Interactable>();

    Dictionary<OVRInput.Button, List<Interactable>> click = new Dictionary<OVRInput.Button, List<Interactable>>();

    private void Start() {
        rbody = GetComponent<Rigidbody>();
    }

    void Update () {
        pressed_lastframe = pressed;
        pressed = OVRInput.Button.None;
        
        for (int i = 0; i < buttons.Length; i++) {
            if (OVRInput.Get(buttons[i], controllerMask))
                pressed |= buttons[i];

            if ((pressed & buttons[i]) > 0 && (pressed_lastframe & buttons[i]) == 0) {
                if (!click.ContainsKey(buttons[i])) {
                    click.Add(buttons[i], new List<Interactable>());
                    foreach (Interactable interact in hover) {
                        interact.InteractClick(this, buttons[i]);
                        click[buttons[i]].Add(interact);
                    }
                }
            }

            if ((pressed & buttons[i]) == 0 && (pressed_lastframe & buttons[i]) > 0 && click.ContainsKey(buttons[i])) {
                foreach (Interactable interact in click[buttons[i]]) // Release() all buttons that were Click()ed on by this button
                    interact.InteractRelease(this, buttons[i]);
                click.Remove(buttons[i]);
            }
        }
    }

    public void Vibrate(HapticsController.VibrationStrength strength) {
        if (strength == HapticsController.VibrationStrength.Hard)
            HapticsController.VibrateHard(controllerMask);
        if (strength == HapticsController.VibrationStrength.Medium)
            HapticsController.VibrateMedium(controllerMask);
        if (strength == HapticsController.VibrationStrength.Light)
            HapticsController.VibrateLight(controllerMask);
    }

	void OnTriggerEnter(Collider other){
        Interactable interact = other.gameObject.GetComponent<Interactable>();
        if (interact) {
            hover.Add(interact);
            interact.HoverEnter(this);
        }
    }
    void OnTriggerExit(Collider other) {
        Interactable interact = other.gameObject.GetComponent<Interactable>();
        if (interact) {
            interact.HoverExit(this);
            hover.Remove(interact);
        }
    }
}
