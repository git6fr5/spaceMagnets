using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreUI : MonoBehaviour {

    private Text textbox;

    public ScoreCollector scoreCollector;

    private void Start() {
        textbox = GetComponent<Text>();
    }

    private void Update() {

        textbox.text = "Score: " + scoreCollector.value.ToString();

    }

}
