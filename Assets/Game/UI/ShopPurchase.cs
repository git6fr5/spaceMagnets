using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPurchase : MonoBehaviour
{
    public Shop shop;
    void OnMouseDown() {
        shop.Purchase();
    }
}
