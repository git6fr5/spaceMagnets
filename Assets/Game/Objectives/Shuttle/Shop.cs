using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    // public GameObject toggleShopButton;
    public Button buybutton;
    public Cost[] costs;

    void Start() {


        for (int i = 0; i < costs.Length; i++) {
            // Destroy(shuttles[i].gameObject);
            float yOffset = buybutton.GetComponent<RectTransform>().sizeDelta.y;
            Button newButton = Instantiate(buybutton.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Button>();
            newButton.GetComponent<RectTransform>().localPosition = buybutton.GetComponent<RectTransform>().localPosition + new Vector3(0f, -i * yOffset, 0f);

            Cost newCost = costs[i];
            newButton.onClick.AddListener(delegate { Purchase(newCost); });

            newButton.transform.GetChild(0).GetComponent<Text>().text = newCost.name + ": " + newCost.value.ToString();
            newButton.gameObject.SetActive(true);
        }


    }

    public void Purchase(Cost cost) {
        print("Purchasing");
        GameObject newGameObject = Instantiate(cost.gameObject);
        newGameObject.transform.position = Vector3.zero;
        newGameObject.SetActive(true);
    }

    //public void ToggleShop() {
    //    gameObject.SetActive(!gameObject.activeSelf);
    //}

}
