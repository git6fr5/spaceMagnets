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

        Shuttle[] shuttles = (Shuttle[])GameObject.FindObjectsOfType(typeof(Shuttle));
        for (int i = 0; i < shuttles.Length; i++) {
            if (shuttles[i].GetComponent<ScoreCollector>().value > currScore) {
                currScore = shuttles[i].GetComponent<ScoreCollector>().value;
            }
        }
        textbox.text = "Score: " + currScore.ToString();

    }

}
