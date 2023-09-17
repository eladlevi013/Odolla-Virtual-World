using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public class Item
{
    [FirestoreProperty]
    public string item_name { get; set; }
    [FirestoreProperty]
    public string item_price { get; set; }
    [FirestoreProperty]
    public string hebrew_name { get; set; }
    [FirestoreProperty]
    public string store_name { get; set; }
    [FirestoreProperty]
    public string category_name { get; set; }
    [FirestoreProperty]
    public string id { get; set; }

    public Item()
    {
        this.item_name = "";
        this.item_price = "";
        this.hebrew_name = "";
        this.category_name = "";
    }

    public Item(string item_name, string item_price, string store, string hebrew_name, string category_name, string id)
    {
        this.item_name = item_name;
        this.item_price = item_price;
        this.store_name = store;
        this.hebrew_name = hebrew_name;
        this.category_name = category_name;
        this.id = id;
    }

    public string getItem_name()
    {
        return item_name;
    }


    public string getItem_price()
    {
        return item_price;
    }

    public string getHebrew_name()
    {
        return hebrew_name;
    }

    public string getCategory_name()
    {
        return category_name;
    }

    public string getStore_name()
    {
        return store_name;
    }

    public string getId()
    {
        return id;
    }
    
    public void setItem_name(string item_name)
    {
        this.item_name = item_name;
    }

    public void setItem_price(string item_price)
    {
        this.item_price = item_price;
    }

    public void setItem_hebrew(string hebrew)
    {
        this.hebrew_name = hebrew;
    }

    public void setCategory(string category)
    {
        this.category_name = category;
    }

    public void setStore(string store)
    {
        this.store_name = store;
    }

    public void setId(string id)
    {
        this.id = id;
    }
}
