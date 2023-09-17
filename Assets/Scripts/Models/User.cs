using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string hat;
    public string hair;
    public string shirt;
    public string pants;
    public string hands;
    public string background;

    public User()
    {
        this.hat = "";
        this.hair = "";
        this.shirt = "";
        this.pants = "";
        this.hands = "";
        this.background = "";
    }

    public User(string hat, string hair, string shirt, string pants, string hand, string background)
    {
        this.hat = hat;
        this.hair = hair;
        this.shirt = shirt;
        this.pants = pants;
        this.hands = hand;
        this.background = background;
    }

    public void SetBackground(string background)
    {
        this.background = background;
    }

    public void SetHat(string hat)
    {
        this.hat = hat;
    }

    public void SetHand(string hand)
    {
        this.hands = hand;
    }

    public void SetHair(string hair)
    {
        this.hair = hair;
    }

    public void SetShirt(string shirt)
    {
        this.shirt = shirt;
    }

    public void SetPants(string pants)
    {
        this.pants = pants;
    }

    public string GetHat()
    {
        return this.hat;
    }

    public string GetHand()
    {
        return this.hands;
    }

    public string GetHair()
    {
        return this.hair;
    }

    public string GetShirt()
    {
        return this.shirt;
    }

    public string GetPants()
    {
        return this.pants;
    }

    public string GetBackgronud()
    {
        return this.background;
    }
}
