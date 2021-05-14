using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    
    // 每日精选的整个界面
    [SerializeField]
    private GameObject ShopPage;
    // 管理是否激活商店界面
    public void ActiveShopPage()
    {
        ShopPage.SetActive(!ShopPage.activeSelf);
        this.gameObject.SetActive(false);
    }
}
