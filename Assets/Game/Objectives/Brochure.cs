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
    public Text textbox;

    /* --- Properties --- */

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        //spriteRenderer = GetComponent<SpriteRenderer>();

        //// Set up the components
        //spriteRenderer.sortingLayerName = GameRules.Foreground;
        FindPlanets();
    }

    private void Update() {

    }

    /* --- Methods --- */
    private void FindPlanets() {
        Planet[] planets = (Planet[])GameObject.FindObjectsOfType(typeof(Planet));
        for (int i = 0; i < planets.Length; i++) {
            // Destroy(shuttles[i].gameObject);
            float yOffset = textbox.GetComponent<RectTransform>().sizeDelta.y;
            Text newTextbox = Instantiate(textbox.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Text>();
            newTextbox.GetComponent<RectTransform>().localPosition = textbox.GetComponent<RectTransform>().localPosition + new Vector3(0f, -i * yOffset, 0f);
            newTextbox.text = planets[i].planetName + ": " + planets[i].scoreValue.ToString();
            newTextbox.gameObject.SetActive(true);
        }
    }

}
