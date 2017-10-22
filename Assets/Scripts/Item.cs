using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item  {

    private int id;
    private string name;
    private int buyPrice;
    private int sellPrice;
    private string type;

    public Item(int newId, string newName, int newBuyPrice, int newSellPrice, string newType)
    {
        id = newId;
        name = newName;
        buyPrice = newBuyPrice;
        sellPrice = newSellPrice;
        type = newType;
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int BuyPrice
    {
        get
        {
            return buyPrice;
        }

        set
        {
            buyPrice = value;
        }
    }

    public int SellPrice
    {
        get
        {
            return sellPrice;
        }

        set
        {
            sellPrice = value;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }
}
