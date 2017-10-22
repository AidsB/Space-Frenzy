using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrator : Interactable {
    public Transform hmd;
    public Material rayMaterial;
    public Transform kinectTransform;
    public UnityEngine.UI.Text helpText;

    bool rotationMode = false;

    List<Transform> rays = new List<Transform>();
    
    Dictionary<Hand, Transform> addingHands = new Dictionary<Hand, Transform>();

    void Start() {
        kinectTransform.position = KinectInput.kinect2world * new Vector4(0f, 0f, 0f, 1f);
        kinectTransform.rotation = KinectInput.kinect2world.rotation;
        kinectTransform.localScale = KinectInput.kinect2world.lossyScale;
    }
    void Update() {
        foreach (KeyValuePair<Hand, Transform> kp in addingHands) {
            kp.Value.position = kp.Key.transform.position;
            kp.Value.rotation = kp.Key.transform.rotation;
        }

        kinectTransform.rotation = Quaternion.LookRotation((hmd.transform.position - kinectTransform.position).normalized);
    }
    
    public override void InteractClick(Hand hand, OVRInput.Button button) {
        if (rotationMode) return;

        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            hand.Vibrate(HapticsController.VibrationStrength.Light);

            GameObject obj = new GameObject("Calibration Ray");
            LineRenderer lr = obj.AddComponent<LineRenderer>();
            obj.transform.position = hand.transform.position;
            obj.transform.rotation = hand.transform.rotation;

            lr.positionCount = 2;
            lr.startWidth = lr.endWidth = .02f;
            lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.forward * 100f });
            lr.startColor = lr.endColor = Color.red;
            lr.useWorldSpace = false;
            lr.sharedMaterial = rayMaterial;

            addingHands.Add(hand, obj.transform);
        } else if (button == OVRInput.Button.PrimaryHandTrigger) {
            if (rotationMode) return;
            if (rays.Count > 0) {
                Destroy(rays[rays.Count - 1].gameObject);
                rays.RemoveAt(rays.Count - 1);
            }
            hand.Vibrate(HapticsController.VibrationStrength.Hard);
        }
    }
    public override void InteractRelease(Hand hand, OVRInput.Button button) {
        if (button == OVRInput.Button.PrimaryIndexTrigger) {
            if (rotationMode) return;

            if (addingHands.ContainsKey(hand)) {
                rays.Add(addingHands[hand]);
                addingHands.Remove(hand);
            }

            hand.Vibrate(HapticsController.VibrationStrength.Light);

            if (rays.Count > 1) EstimatePosition();
        } else if (button == OVRInput.Button.One) {
            if (rotationMode) {
                hand.Vibrate(HapticsController.VibrationStrength.Light);
                rotationMode = false;
                KinectInput.SetCalibration(kinectTransform.localToWorldMatrix);
                helpText.text = "Calibration complete.";
                enabled = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("ship", UnityEngine.SceneManagement.LoadSceneMode.Single);
            } else {
                rotationMode = true;
                helpText.text = "Please look directly at the front of your Kinect and press a button.";
            }
        }
    }

    Vector3 midpoint(Ray r1, Ray r2) {
        float a = Vector3.Dot(r1.direction, r1.direction);
        float b = Vector3.Dot(r1.direction, r2.direction);
        float e = Vector3.Dot(r2.direction, r2.direction);

        float d = a * e - b * b;
        if (d != 0) {
            Vector3 r = r1.origin - r2.origin;
            float c = Vector3.Dot(r1.direction, r);
            float f = Vector3.Dot(r2.direction, r);

            float s = (b * f - c * e) / d;
            float t = (a * f - b * c) / d;
            return (r1.GetPoint(s) + r2.GetPoint(t)) * .5f;
        }

        return (r1.origin + r2.origin) * .5f; // parallel lines
    }

    void EstimatePosition() {
        Vector3 pos = Vector3.zero;

        foreach (Transform r in rays) {
            Vector3 pt = Vector3.zero;
            foreach (Transform r2 in rays) {
                if (r != r2) pt += midpoint(new Ray(r.position, r.forward), new Ray(r2.position, r2.forward));
            }
            pt /= rays.Count - 1;
            pos += pt;
        }

        pos /= rays.Count;
        kinectTransform.position = pos;
    }
}
