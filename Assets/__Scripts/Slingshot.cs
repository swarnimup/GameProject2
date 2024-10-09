using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    [Header("Inscribed")]
    public GameObject projectilePrefab;  // The prefab for the projectile
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public AudioSource snapSound;  // Reference to the AudioSource for snapping sound

    [Header("Dynamic")]
    public GameObject launchPoint;       // The launch point for the projectile
    public Vector3 launchPos;            // The position of the launch point
    public GameObject projectile;        // The instantiated projectile
    public bool aimingMode;              // Whether or not the slingshot is aiming

    void Awake() {
        // Find the launch point within the Slingshot object
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        // Get the AudioSource for the snap sound
        snapSound = GetComponent<AudioSource>();
    }

    void OnMouseDown() {
        aimingMode = true;

        // Instantiate a projectile
        projectile = Instantiate(projectilePrefab) as GameObject;

        // Start it at the launch point
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update() {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        projectile.transform.position = launchPos + mouseDelta;

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.velocity = -mouseDelta * velocityMult;

            // Play the snapping sound when the projectile is fired
            if (snapSound != null) {
                snapSound.Play();
            }

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }
}
