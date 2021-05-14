using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Function1._02.Scripts
{
    public class JsonController
    {
        public static List<ItemInfomation> LoadItemInfomation(string path)
        {
            TextAsset textAsset = (TextAsset) Resources.Load("data");

            List<ItemInfomation> itemInfomationList = new List<ItemInfomation>();

            // 解析jsonnode 文件
            foreach (JSONNode product in JSONNode.Parse(textAsset.text)["dailyProduct"])
            {
                ItemInfomation itemInfomation = new ItemInfomation();
                itemInfomation.rewardType = (RewardType) (int) product["type"];
                itemInfomation.num = product["num"];
                itemInfomation.type = product["type"];
                itemInfomation.isPurchased = product["isPurchased"];
                if ((RewardType) (int) product["type"] == RewardType.Cards)
                {
                    // 存储专属与人物卡片的信息
                    itemInfomation.productId = product["productId"];
                    itemInfomation.subType = product["subType"];
                    itemInfomation.costGold = product["costGold"];
                }

                itemInfomationList.Add(itemInfomation);
            }
            return itemInfomationList;
        }

        public static int LoadDurationTime(string path)
        {
            TextAsset textAsset = (TextAsset) Resources.Load("data");
            return JSONNode.Parse(textAsset.text)["dailyProductCountDown"];
        }
    }
}