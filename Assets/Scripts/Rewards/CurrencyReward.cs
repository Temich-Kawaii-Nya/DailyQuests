using Rewards;
using UnityEngine;

public class CurrencyReward : Reward
{
    private string _currencyType;
    public CurrencyReward(string currencyType, string name, string description, int amount) : base(name, description, amount)
    {
        _currencyType = currencyType;
    }
    public override void GetReward()
    {
        Debug.Log(_currencyType + " amount: " + Amount);
    }
}
