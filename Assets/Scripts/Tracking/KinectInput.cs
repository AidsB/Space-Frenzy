using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectInput : MonoBehaviour {
    public BodySourceManager bodySource;
    public Transform hmd, rightController, leftController;

    public GameObject leftFoot, rightFoot, leftHand, rightHand, leftKnee, rightKnee, leftElbow, rightElbow;

    Vector3 offset;
    
    public float unitScale = 1f;
    public static Matrix4x4 kinect2world { get; private set; }

    Body trackedBody;
    
    void Update() {
        Body[] data = bodySource.GetData();
        if (data == null) return;

        List<ulong> trackedIds = new List<ulong>();
        if (trackedBody != null && !trackedIds.Contains(trackedBody.TrackingId))
            trackedBody = null;

        foreach (Body body in data)
            if (body != null && body.IsTracked) {
                if (trackedBody == null)
                    trackedBody = body;
                if (trackedBody.TrackingId == body.TrackingId)
                    UpdatePositions();
            }
    }

    Vector3 k2u(CameraSpacePoint p) {
        return new Vector3(p.X, p.Y, p.Z);
    }
    Quaternion k2u(Windows.Kinect.Vector4 p) {
        return new Quaternion(p.X, p.Y, p.Z, p.W);
    }

    Vector3 cameraToWorld(Vector3 point) {
        return kinect2world * new UnityEngine.Vector4(-point.x, point.y, point.z, 1.0f);
    }
    
    void UpdatePositions() {
        Vector3 rfoot = cameraToWorld(k2u(trackedBody.Joints[JointType.AnkleRight].Position));
        Vector3 lfoot = cameraToWorld(k2u(trackedBody.Joints[JointType.AnkleLeft].Position));
        Vector3 rhand = cameraToWorld(k2u(trackedBody.Joints[JointType.HandRight].Position));
        Vector3 lhand = cameraToWorld(k2u(trackedBody.Joints[JointType.HandLeft].Position));
        Vector3 lknee = cameraToWorld(k2u(trackedBody.Joints[JointType.KneeLeft].Position));
        Vector3 rknee = cameraToWorld(k2u(trackedBody.Joints[JointType.KneeRight].Position));
        Vector3 lelbow = cameraToWorld(k2u(trackedBody.Joints[JointType.ElbowLeft].Position));
        Vector3 relbow = cameraToWorld(k2u(trackedBody.Joints[JointType.ElbowRight].Position));

        offset = (rightController.position - rhand) * .5f + (leftController.position - lhand) * .5f;

        rfoot += offset;
        lfoot += offset;
        rhand += offset;
        lhand += offset;
        lknee += offset;
        rknee += offset;
        lelbow += offset;
        relbow += offset;

        leftFoot.transform.position = lfoot;
        rightFoot.transform.position = rfoot;
        leftHand.transform.position = lhand;
        rightHand.transform.position = rhand;
        leftKnee.transform.position = lknee;
        rightKnee.transform.position = rknee;
        leftElbow.transform.position = lelbow;
        rightElbow.transform.position = relbow;
    }

    void Awake() {
        TryLoadCalibration();
    }

    static void TryLoadCalibration() {
        kinect2world = Matrix4x4.identity;
        if (!PlayerPrefs.HasKey("CalibrationData")) return;
        Matrix4x4 m = Matrix4x4.identity;
        for (int i = 0; i < 16; i++)
            m[i] = PlayerPrefs.GetFloat("Calibration" + i);
        kinect2world = m;
    }
    public static void SetCalibration(Matrix4x4 kinectMatrix) {
        kinect2world = kinectMatrix;

        PlayerPrefs.SetInt("CalibrationData", 1);
        for (int i = 0; i < 16; i++)
            PlayerPrefs.SetFloat("Calibration" + i, kinectMatrix[i]);
        PlayerPrefs.Save();
    }
}
