/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
// [RequireComponent(typeof(SpriteRenderer))]
public class Brochure : MonoBehaviour {

    /* --- Components --- */
    public GameObject revenueComponentObject;
    public Transform contentTransform;

    /* --- Properties --- */

    /* --- Unity --- */
    private void Start() {
        FindRevenues();
    }

    private void Update() {

    }

    /* --- Methods --- */
    private void FindRevenues() {
        Revenue[] revenues = (Revenue[])GameObject.FindObjectsOfType(typeof(Revenue));
        for (int i = 0; i < revenues.Length; i++) {
            // Destroy(shuttles[i].gameObject);
            float yOffset = revenueComponentObject.GetComponent<RectTransform>().sizeDelta.y;
            GameObject newObject = Instantiate(revenueComponentObject.gameObject, Vector3.zero, Quaternion.identity, contentTransform);
            newObject.GetComponent<RectTransform>().localPosition = revenueComponentObject.GetComponent<RectTransform>().localPosition + new Vector3(0f, -i * yOffset, 0f);

            // newTextbox.transform.SetParent(contentTransform);
            Text newTextbox = newObject.transform.GetChild(0).GetComponent<Text>();
            newTextbox.text = revenues[i].locationName + "\n (" + revenues[i].type.ToString() + ")\n Value: " + revenues[i].value.ToString();
            newObject.gameObject.SetActive(true);
        }
    }

}
