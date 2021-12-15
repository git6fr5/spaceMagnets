using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarUI : MonoBehaviour {


    public GameObject toggleStateObject;
    public GameObject[] selectionObjects;

    public Dropdown dropdown;


    public void Start() {
        List<GameObject> selectionList = new List<GameObject>();
        foreach (Transform child in toggleStateObject.transform) {
            selectionList.Add(child.gameObject);
        }
        selectionObjects = selectionList.ToArray();
    }

    public void Toggle() {

        toggleStateObject.SetActive(!toggleStateObject.activeSelf);

    }

    public void Select() {

        for (int i = 0; i < selectionObjects.Length; i++) {
            selectionObjects[i].SetActive(false);
        }

        selectionObjects[dropdown.value].SetActive(true);

    }
}
