using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class RevenueUI : MonoBehaviour {

    private Canvas canvas;

    private Revenue revenue;
    private ShuttlePath shuttlePath;

    public Text nameTextbox;
    public Text typeTextbox;

    public Text valueTextbox;
    public string valueText;

    void Start() {

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.pixelPerfect = true;

        // Assuming there is one on the parent.
        revenue = transform.parent.GetComponent<Revenue>();
        // Assuming there is only one.
        shuttlePath = (ShuttlePath)GameObject.FindObjectOfType(typeof(ShuttlePath));

        if (revenue != null && shuttlePath != null) {

            nameTextbox.text = revenue.locationName;
            typeTextbox.text = revenue.type.ToString();

        }
    }

    void Update() {

        if (revenue != null && shuttlePath != null) {

            int currRevenue = shuttlePath.revenueDict.ContainsKey(revenue) ? shuttlePath.revenueDict[revenue] : 0;
            valueText = currRevenue.ToString() + " / " + revenue.value.ToString();
            valueTextbox.text = valueText;

        }

    }

}
