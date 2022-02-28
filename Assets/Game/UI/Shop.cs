using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Shop : MonoBehaviour {

    // public GameObject toggleShopButton;
    public Button buybutton;
    public Cost[] costs;

    public int currIndex;
    public bool isActive = false;

    public SpriteRenderer shopBackground;
    public SpriteRenderer shopDisplay;

    public Brochure brochure;

    void Start() {
        // CreateUIElements();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Update() {
        shopBackground.gameObject.SetActive(isActive);
    }

    private void OnMouseDown() {
        OpenShop();
    }

    /* --- Methods --- */
    private void OpenShop() {
        isActive = !isActive;
        if (isActive) {
            brochure.isActive = false;
        }
        SetShop(currIndex);
    }
    
    public void SetShop(int index) {
        shopDisplay.sprite = costs[index].GetComponent<SpriteRenderer>().sprite;
        currIndex = index;
    }

    private void CreateUIElements() {
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

    public void Purchase() {
        Purchase(costs[currIndex]);
    }

    //public void ToggleShop() {
    //    gameObject.SetActive(!gameObject.activeSelf);
    //}

}
