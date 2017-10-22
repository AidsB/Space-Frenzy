using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public virtual void HoverEnter(Hand hand) { }
    public virtual void HoverExit(Hand hand) { }

    public virtual void InteractClick(Hand hand, OVRInput.Button button) { }
    public virtual void InteractRelease(Hand hand, OVRInput.Button button) { }

    public virtual void OnFootEnter(Foot foot) { }
    public virtual void OnFootExit(Foot foot) { }
}
