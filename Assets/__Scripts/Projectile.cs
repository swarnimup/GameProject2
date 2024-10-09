using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
    public GameObject impactParticlePrefab;  // Reference to the particle effect prefab
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;
    private bool _awake = true;  // Add the 'awake' boolean property back

    public bool awake {
        get { return _awake; }
        private set { _awake = value; }
    }

    void Start() {
        rigid = GetComponent<Rigidbody>();
        prevPos = new Vector3(1000, 1000, 0);
        deltas.Add(1000);
        PROJECTILES.Add(this);
    }

    void FixedUpdate() {
        if (rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT) {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach (float f in deltas) {
            if (f > maxDelta) maxDelta = f;
        }

        if (maxDelta <= Physics.sleepThreshold) {
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (impactParticlePrefab != null) {
            Instantiate(impactParticlePrefab, collision.contacts[0].point, Quaternion.identity);
        }

        Destroy(gameObject, 2f );
    }

    private void OnDestroy() {
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES() {
        foreach (Projectile p in PROJECTILES) {
            if (p != null) {
                Destroy(p.gameObject);
            }
        }
        PROJECTILES.Clear();
    }
}
