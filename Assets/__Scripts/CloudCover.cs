using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCover : MonoBehaviour {
    [Header("Inscribed")]
    public Sprite[] cloudSprites;  // Array of sprites for clouds
    public int numClouds = 40;     // Number of clouds to generate
    public Vector3 minPos = new Vector3(-20, -5, -5);  // Minimum position for cloud placement
    public Vector3 maxPos = new Vector3(300, 40, 5);   // Maximum position for cloud placement

    [Tooltip("For scaleRange, x is the min value, and y is the max value.")]
    public Vector2 scaleRange = new Vector2(1, 4);  // Range for random scale of clouds

    void Start() {
        Transform parentTrans = this.transform;
        GameObject cloudGO;
        Transform cloudTrans;
        SpriteRenderer sRend;
        float scaleMult;

        for (int i = 0; i < numClouds; i++) {
            // Create a new GameObject (from scratch!) and get its Transform
            cloudGO = new GameObject();
            cloudTrans = cloudGO.transform;
            sRend = cloudGO.AddComponent<SpriteRenderer>();  // Add a SpriteRenderer component

            // Select a random sprite from the array
            int spriteNum = Random.Range(0, cloudSprites.Length);
            sRend.sprite = cloudSprites[spriteNum];

            // Set the cloud's position
            cloudTrans.position = RandomPos();
            cloudTrans.SetParent(parentTrans, true);

            // Set the cloud's scale
            scaleMult = Random.Range(scaleRange.x, scaleRange.y);
            cloudTrans.localScale = Vector3.one * scaleMult;
        }
    }

    // Function to get a random position within the defined range
    Vector3 RandomPos() {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(minPos.x, maxPos.x);
        pos.y = Random.Range(minPos.y, maxPos.y);
        pos.z = Random.Range(minPos.z, maxPos.z);
        return pos;
    }
}
