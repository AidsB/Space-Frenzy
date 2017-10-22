using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : Interactable {
    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.One)
            SceneManager.LoadScene("ship", LoadSceneMode.Single);
        else if (button == OVRInput.Button.Two)
            SceneManager.LoadScene("calibration", LoadSceneMode.Single);
    }
}
