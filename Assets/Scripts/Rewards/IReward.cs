using System;

namespace Rewards
{
    public abstract class Reward
    {
        public string Name { get; }
        public string Description { get; }
        public int Amount { get; }

        public Reward(string name, string description, int amount)
        {
            Name = name;
            Description = description;
            Amount = amount;
        }

        public virtual void GetReward()
        {
            throw new NotImplementedException();
        }
    }
}
