using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreUI : MonoBehaviour {

    private Text textbox;

    public int currScore;

    private void Start() {
        textbox = GetComponent<Text>();
    }

    private void Update() {

        ShuttlePath shuttlePath = (ShuttlePath)GameObject.FindObjectOfType(typeof(ShuttlePath));
        string profitString = "Did not reach station.";
        if (shuttlePath.reachedStation) {
            profitString = shuttlePath.profit.ToString();
        }
        textbox.text = "Revenue: " + shuttlePath.revenue.ToString() + ", Costs: " + shuttlePath.cost.ToString() + ", Profit: " + profitString;

    }

}
