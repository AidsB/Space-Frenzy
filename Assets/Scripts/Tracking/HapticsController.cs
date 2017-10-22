using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HapticsController {
    public enum VibrationStrength {
        None, Light, Medium, Hard
    };

    private static OVRHapticsClip clipLight;
    private static OVRHapticsClip clipMedium;
    private static OVRHapticsClip clipHard;
    
    static HapticsController() {
        int cnt = 10;
        clipLight = new OVRHapticsClip(cnt);
        clipMedium = new OVRHapticsClip(cnt);
        clipHard = new OVRHapticsClip(cnt);
        for (int i = 0; i < cnt; i++) {
            clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)75;
            clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)150;
            clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)255;
        }

        clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
        clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
        clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
    }

    public static void VibrateLight(OVRInput.Controller controllerMask) {
        if ((controllerMask & OVRInput.Controller.LTouch) > 0)
            OVRHaptics.LeftChannel.Preempt(clipLight);
        if ((controllerMask & OVRInput.Controller.RTouch) > 0)
            OVRHaptics.RightChannel.Preempt(clipLight);
    }

    public static void VibrateMedium(OVRInput.Controller controllerMask) {
        if ((controllerMask & OVRInput.Controller.LTouch) > 0)
            OVRHaptics.LeftChannel.Preempt(clipMedium);
        if ((controllerMask & OVRInput.Controller.RTouch) > 0)
            OVRHaptics.RightChannel.Preempt(clipMedium);
    }

    public static void VibrateHard(OVRInput.Controller controllerMask) {
        if ((controllerMask & OVRInput.Controller.LTouch) > 0)
            OVRHaptics.LeftChannel.Preempt(clipHard);
        if ((controllerMask & OVRInput.Controller.RTouch) > 0)
            OVRHaptics.RightChannel.Preempt(clipHard);
    }
}
