using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace Function1._02.Scripts
{
    public class ShopPage : MonoBehaviour
    {
        // 新生成子页面的父物体
        [SerializeField]
        private  GameObject panelCards;
        // 展示倒计时信息的文本
        [SerializeField]
        private  Text refreshMessage;
        // 需要实例化的对象：item
        [SerializeField]
        private  Item item;
        // 需要实例化的对象：被锁卡片
        [SerializeField]
        private  GameObject cardLocked;
        [SerializeField]
        private int CardsToShow = 6; // 展示的卡片数量
        
        private  int rewardDurationTime = 10; // 两次领取免费礼品的间隔时间，单位：秒
        private DateTime nextRewardTime; // 下一次领取奖励的时间

        void OnEnable()
        {
            // 读取配置文件，创建预制件
            var itemInfomationList = JsonController.LoadItemInfomation("data");
            Initialize(itemInfomationList);
        }

        // 商店界面刷新
        void refresh()
        {
            foreach (Transform child in panelCards.transform)
            {
                Destroy(child.gameObject);
            }

            // 读取配置文件，创建预制件
            var itemInfomationList = JsonController.LoadItemInfomation("data");
            CreatPrefeb(itemInfomationList);

            // 刷新倒计时信息
            StartCoroutine(WaitForReward());
        }

        void Initialize(List<ItemInfomation> itemInfomationList)
        {
            // 配置领取时间间隔
            rewardDurationTime = JsonController.LoadDurationTime("data");
            // 刷新倒计时信息
            StartCoroutine(WaitForReward());
            
            CreatPrefeb(itemInfomationList);
        }
        

        // 根据解析的JSON，数据生成对应的卡片
        void CreatPrefeb(List<ItemInfomation> itemInfomationList)
        {
            
            // 生成预制件
            GameObject cardPrefeb;
            foreach (var itemInfomation in itemInfomationList)
            {
                // 实例化预制件，放到特定位置；
                cardPrefeb = Instantiate(item.gameObject, panelCards.transform);
                cardPrefeb.GetComponent<Item>().Initialize(itemInfomation);
                cardPrefeb.SetActive(true);
            }

            //  动态控制被锁的卡片的数量
            var cardLockNumber = CardsToShow - itemInfomationList.Count;
            for (var i = 0; i < cardLockNumber; i++)
            {
                cardPrefeb = Instantiate(cardLocked, panelCards.transform);
                cardPrefeb.SetActive(true);
            }
        }

        // 计算领取倒计时，更新文本
        IEnumerator WaitForReward()
        {
            nextRewardTime = DateTime.Now.AddSeconds(rewardDurationTime + 0.1f);
            TimeSpan duration = nextRewardTime.Subtract(DateTime.Now);
            while (duration.TotalSeconds > 1)
            {
                refreshMessage.text = $"Refresh time:{duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
                duration = nextRewardTime.Subtract(DateTime.Now);
                yield return new WaitForSeconds(0.5f);
            }
            refreshMessage.text = "Refresh time:00:00:00";
            refresh();
        }
    }
}