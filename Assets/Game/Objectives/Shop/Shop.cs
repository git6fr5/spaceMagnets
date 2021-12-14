using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public int currGold; // Move this somewhere else eventually right?
    public Text goldTextbox;

    public Button buybutton;
    public Purchaseable[] purchaseables;

    void Start() {


        for (int i = 0; i < purchaseables.Length; i++) {
            // Destroy(shuttles[i].gameObject);
            float yOffset = buybutton.GetComponent<RectTransform>().sizeDelta.y;
            Button newButton = Instantiate(buybutton.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Button>();
            newButton.GetComponent<RectTransform>().localPosition = buybutton.GetComponent<RectTransform>().localPosition + new Vector3(0f, -i * yOffset, 0f);

            Purchaseable purchaseable = purchaseables[i];
            newButton.onClick.AddListener(delegate { Purchase(purchaseable); });

            newButton.transform.GetChild(0).GetComponent<Text>().text = purchaseables[i].name + ": " + purchaseables[i].price.ToString();
            newButton.gameObject.SetActive(true);
        }


    }

    public void Purchase(Purchaseable purchaseable) {
        print("bought whatever");
        GameObject newGameObject = Instantiate(purchaseable.gameObject);
        newGameObject.transform.position = Vector3.zero;
        newGameObject.SetActive(true);
        currGold -= purchaseable.price;
    }

    void Update() {


        goldTextbox.text = "Gold: " + currGold.ToString();

    }

}
