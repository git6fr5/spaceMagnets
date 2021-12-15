using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CostUI : MonoBehaviour {

    private Canvas canvas;

    private Cost cost;

    public Text nameTextbox;
    public Text typeTextbox;

    public Text valueTextbox;
    public string valueText;

    void Start() {

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.pixelPerfect = true;

        // Assuming there is one on the parent.
        cost = transform.parent.GetComponent<Cost>();
        if (cost != null) {

            nameTextbox.text = cost.toolName;
            typeTextbox.text = cost.type.ToString();
            valueTextbox.text = "-" + cost.value.ToString();
        }
    }

}
