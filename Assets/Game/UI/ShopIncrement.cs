using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ShopIncrement : MonoBehaviour {

    public int index;
    public Shop shop;

    /* --- Unity --- */
    private void Start() {

        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnMouseDown() {
        print("hello");
        int newIndex = ((shop.currIndex + index) % shop.costs.Length);
        shop.SetShop(newIndex);
    }
}
