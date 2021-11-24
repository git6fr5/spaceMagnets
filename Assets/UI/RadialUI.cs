/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialUI : MonoBehaviour {

    /* --- Properties --- */
    public float rotationRate = 5f;
    public bool counterClockWise;

    /* --- Unity --- */
    // Runs once per frame.
    private void Update() {
        int direction = counterClockWise ? 1 : -1;
        transform.eulerAngles += direction * Time.deltaTime * new Vector3(0f, 0f, rotationRate);
    }

}
