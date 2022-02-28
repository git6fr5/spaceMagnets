using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class BrochureIncrement : MonoBehaviour {

    public int index;
    public Brochure brochure;

    /* --- Unity --- */
    private void Start() {

        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnMouseDown() {
        print("hello");
        int newIndex = ((brochure.currIndex + index) % brochure.revenues.Length);
        brochure.SetBrochure(newIndex);
    }
}
