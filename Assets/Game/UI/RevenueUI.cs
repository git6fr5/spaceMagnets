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

    public bool getDataFromBrochure;
    public Brochure brochure;

    void Start() {

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.pixelPerfect = true;

        // Assuming there is one on the parent.
        if (!getDataFromBrochure) {
            revenue = transform.parent.GetComponent<Revenue>();
        }
        else {
            revenue = brochure.revenues[brochure.currIndex];
        }
        // Assuming there is only one.
        shuttlePath = (ShuttlePath)GameObject.FindObjectOfType(typeof(ShuttlePath));

        if (revenue != null && shuttlePath != null) {

            nameTextbox.text = revenue.locationName;
            typeTextbox.text = revenue.type.ToString();

        }
    }

    void Update() {

        if (getDataFromBrochure) {
            revenue = brochure.revenues[brochure.currIndex];
            shuttlePath = (ShuttlePath)GameObject.FindObjectOfType(typeof(ShuttlePath));
        }

        if (revenue != null && shuttlePath != null) {
            nameTextbox.text = revenue.locationName;
            typeTextbox.text = revenue.type.ToString();
            
            int currRevenue = shuttlePath.revenueDict.ContainsKey(revenue) ? shuttlePath.revenueDict[revenue] : 0;
            valueText = currRevenue.ToString() + " / " + revenue.value.ToString();
            valueTextbox.text = valueText;

        }

    }

}
