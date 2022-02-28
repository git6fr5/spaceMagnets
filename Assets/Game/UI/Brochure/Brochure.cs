/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Brochure : MonoBehaviour {

    /* --- Components --- */
    public GameObject revenueComponentObject;
    public Transform contentTransform;

    /* --- Properties --- */
    //
    public Revenue[] revenues;
    public int currIndex;
    //
    public bool isActive = false;

    public SpriteRenderer brochureBackground;
    public SpriteRenderer brochureDisplay;

    public Shop shop;

    /* --- Unity --- */
    private void Start() {

        GetComponent<BoxCollider2D>().isTrigger = true;
        FindRevenues();
    }

    private void Update() {

        brochureBackground.gameObject.SetActive(isActive);


    }

    private void OnMouseDown() {

        OpenBrochure();

    }

    /* --- Methods --- */
    private void FindRevenues() {
        revenues = (Revenue[])GameObject.FindObjectsOfType(typeof(Revenue));


        // CreateUIElements();
    }

    private void OpenBrochure() {
        isActive = !isActive;
        if (isActive) {
            shop.isActive = false;
        }
        SetBrochure(currIndex);

    }

    public void SetBrochure(int index) {
        brochureDisplay.sprite = revenues[index].GetComponent<SpriteRenderer>().sprite;
        currIndex = index;
    }

    private void CreateUIElements() {
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
