using System;
using System.Collections;
using System.Collections.Generic;
using Function1._02.Scripts;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;


// Item的游戏信息
public class ItemInfomation
{
    public RewardType rewardType;
    public int type;
    public int num;
    public int isPurchased;
    public int productId;
    public int subType;
    public int costGold;
}

[Serializable]
// Item的UI信息
public class ItemUI
{
    public Image cardBackgroundImage; // 卡片背景图
    public Text name; // 卡片名字
    public Image iconBackgroundImage; // 展示卡片内容的背景
    public Image iconFront; // 展示的卡片内容
    public GameObject buttonFree; // 免费领取按钮
    public GameObject buttonCost; // 购买按钮
    public Text costText; // 购买花费金币的信息
    public GameObject purchasedPanel; // 购买完毕的信息文本
}

[Serializable]
// 卡片的UI配置
public class ItemUIConfig
{
    public int fontSize; // 文本名称的大小
    public CoinsUIConfig coinsUIConfig; // 金币图片
    public CharacterUIConfig characterUIConfig; // 人物卡片、人物子类型
}

[Serializable]
// 钻石和金币的UI 配置信息
public class CoinsUIConfig
{
    public Sprite iconCoins; // 金币图片
    public Sprite iconDiamonds; // 钻石图片
    public Color coinsNameFontColor; // 金币和钻石的名称的字体颜色
    public Sprite iconBackgroundImage; // 展示卡片内容的背景
    public Sprite cardBackgroundImage; // 卡片背景图
}

[Serializable]
// 人物卡片的UI 配置信息
public class CharacterUIConfig
{
    public Color nameFontColor; // 人物卡片的名称的字体颜色
    public CharacterUISprite[] characterUISprite;
}

[Serializable]
public class CharacterUISprite
{
    public Sprite characterSprite; //人物卡片
    public int characterSubType; // 人物子类型
    public String name; // 人物名称
}

public class Item : MonoBehaviour
{
    // 储存类型、是否购买等信息
    private ItemInfomation itemInfomation = new ItemInfomation();

    // 游戏中的UI控件
    public ItemUI itemUI;

    // 游戏中UI的配置信息
    public ItemUIConfig itemUIConfig;


    // 根据json文件设置信息，例如：是否购买等；并更新UI的购买状态
    public void Initialize(ItemInfomation itemInfomation)
    {
        this.itemInfomation = itemInfomation;
        // 调整字号
        itemUI.name.fontSize = itemUIConfig.fontSize;
        // 金币和钻石都需要调整：名称、字体颜色、整个卡片背景、图标背景
        if (itemInfomation.rewardType == RewardType.Coins || itemInfomation.rewardType == RewardType.Diamonds)
        {
            itemUI.name.text = itemInfomation.rewardType.ToString();
            itemUI.name.color = itemUIConfig.coinsUIConfig.coinsNameFontColor;
            itemUI.cardBackgroundImage.sprite = itemUIConfig.coinsUIConfig.cardBackgroundImage;
            itemUI.iconBackgroundImage.sprite = itemUIConfig.coinsUIConfig.iconBackgroundImage;
        }
        // 进一步调整UI配置
        switch (itemInfomation.rewardType)
        {
            case RewardType.Coins:
                // 更换金币的图标
                itemUI.iconFront.sprite = itemUIConfig.coinsUIConfig.iconCoins;
                break;
            case RewardType.Diamonds:
                // 更换钻石的图标
                itemUI.iconFront.sprite = itemUIConfig.coinsUIConfig.iconDiamonds;
                break;
            case RewardType.Cards:
                // 设定金币消耗量
                itemUI.costText.text = itemInfomation.costGold.ToString();
                // 更新名称，以及名称的颜色
                itemUI.name.text = (string) GetInformationBySubType<string>(itemInfomation.subType);
                itemUI.name.color = itemUIConfig.characterUIConfig.nameFontColor;
                // 更新人物的图片
                itemUI.iconFront.sprite = (Sprite) GetInformationBySubType<Sprite>(itemInfomation.subType);
                break;
        }

        // 已购买时： isPurchased != -1； 此时激活购买画面
        ActivePurchasedPanel(itemInfomation.isPurchased != -1);
        gameObject.SetActive(true);
    }

    // 根据人物卡片Id,返回对应的名称 或者 图片
    object GetInformationBySubType<T>(int subType)
    {
        foreach (var cardsInfo in itemUIConfig.characterUIConfig.characterUISprite)
        {
            if (cardsInfo.characterSubType == subType)
            {
                if (typeof(T) == typeof(Sprite))
                {
                    return cardsInfo.characterSprite;
                }

                if (typeof(T) == typeof(String))
                {
                    return cardsInfo.name;
                }
            }
        }

        Debug.Log("Unknown Cards Type: " + subType);
        return null;
    }


    //响应鼠标点击事件
    public void Purchase()
    {
        // 更新按钮
        ActivePurchasedPanel(true);
    }

    // 控制是否显示：购买完毕的 Panel
    public void ActivePurchasedPanel(bool isShow)
    {
        itemUI.purchasedPanel.SetActive(isShow);

        if (itemInfomation.rewardType == RewardType.Cards)
        {
            itemUI.buttonCost.SetActive(!isShow);
            itemUI.buttonFree.SetActive(false);
        }
        else
        {
            itemUI.buttonFree.SetActive(!isShow);
            itemUI.buttonCost.SetActive(false);
        }
    }
}