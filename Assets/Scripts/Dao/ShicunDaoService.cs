﻿using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     数据服务接口
/// </summary>

namespace scdesktop
{
    public class ShicunDaoService : MonoBehaviour, IDaoService
    {
        List<BallData> _items = null;

        public BallData GetItem()
        {

            return _items[Random.Range(0, _items.Count)];

        }

        public void prepareData()
        {            
            _items = new List<BallData>();
            

            // 准备数据
            string[,] str = { 
                { "千岁庙1", "千岁庙" },
                {"晒盐2","晒盐1" },
                { "村墅公园/1","村墅公园/村墅公园1"},
                { "村墅公园/2","村墅公园/村墅公园2"},
                { "村墅公园/3","村墅公园/村墅公园3"},
                { "奉贤滚灯/1","奉贤滚灯/奉贤滚灯1"},
                { "奉贤滚灯/2","奉贤滚灯/奉贤滚灯2"},
                { "奉贤黄桃/1","奉贤黄桃/奉贤黄桃1"},
                { "奉贤黄桃/2","奉贤黄桃/奉贤黄桃2"},
                { "开泰橡胶厂原址/1","开泰橡胶厂原址/开泰橡胶厂原址1"},
                { "开泰橡胶厂原址/2","开泰橡胶厂原址/开泰橡胶厂原址2"},
                { "开心农场/1","开心农场/开心农场1"},
                { "开心农场/2","开心农场/开心农场2"},       
                { "民宿组团/1-1","民宿组团/民宿组团1"},
                { "民宿组团/2-1","民宿组团/民宿组团2"},
                { "拾村里项目规划/1","拾村里项目规划/拾村里项目规划1"},
                { "拾村里项目规划/2","拾村里项目规划/拾村里项目规划2"},
                { "拾村里项目规划/3","拾村里项目规划/拾村里项目规划3"},
                { "拾村新米/1","拾村新米/拾村新米1"},
                { "拾村新米/2","拾村新米/拾村新米2"},
                { "手工打船/1","手工打船/手工打船1"},
                { "手工打船/2","手工打船/手工打船2"},
                { "土布文化/1","土布文化/土布文化1"},
                { "土布文化/2","土布文化/土布文化2"},
                { "土布文化/3","土布文化/土布文化3"},
                { "袖珍农庄/1","袖珍农庄/袖珍农庄1"},
                { "袖珍农庄/2","袖珍农庄/袖珍农庄2"},
                { "有机蔬菜/1","有机蔬菜/有机蔬菜1"},
                { "有机蔬菜/2","有机蔬菜/有机蔬菜2"},
                { "最美庭院/1","最美庭院/最美庭院1"},
                { "最美庭院/2","最美庭院/最美庭院2"}
            };


            for (int i = 0; i < str.GetLength(0); i++) {
                BallData data = new BallData();

                data.cover = str[i, 0];
                data.detailCover = str[i, 1];


                //Debug.Log(data.ToString());
                _items.Add(data);
            }            
        }
    }
}