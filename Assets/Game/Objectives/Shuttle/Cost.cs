using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost : MonoBehaviour {

    public enum Type {
        Blackhole,
        Wormhole
    };

    /* --- Properties --- */
    public string toolName = "Unknown";
    public Type type;
    public int value = 0;

    public CostUI uiComponent;

    void Start() {

        if (type == Type.Blackhole) {
            SetBlackholeName();
        }
        if (type == Type.Wormhole) {
            DockInCorner();
        }

        GameObject uiObject = Instantiate(uiComponent.gameObject, transform.position, Quaternion.identity, transform);
        uiObject.SetActive(true);
        uiObject.transform.position += new Vector3(0, -1.15f, 0f);

    }

    void DockInCorner() {
        int count = 0;
        Cost[] costs = (Cost[])GameObject.FindObjectsOfType(typeof(Cost));
        for (int i = 0; i < costs.Length; i++) {
            if (costs[i].type == Type.Wormhole) {
                count++;
            }
        }
        transform.position = new Vector3(-7.25f + (count - 1) * 1.15f, 5f, 0f);
    }

    void SetBlackholeName() {

        string[] prefixes = new string[] { "MX-", "C-", "H-", "RX-", "A-" };
        int index = Random.Range(0, prefixes.Length);

        int minID = 100; int maxID = 999;
        int id = Random.Range(minID, maxID);
        string suffix = id.ToString();

        toolName = prefixes[index] + suffix;
    }

}
