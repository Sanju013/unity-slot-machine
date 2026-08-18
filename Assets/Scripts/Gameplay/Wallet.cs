using UnityEngine;

namespace UnitySlotMachine.Gameplay
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private int startingBalance = 500;

        private int balance;

        public int Balance => balance;

        private void Awake()
        {
            balance = startingBalance;
        }

        public bool CanAfford(int amount)
        {
            return amount > 0 && balance >= amount;
        }

        public bool Spend(int amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            balance -= amount;
            return true;
        }

        public void Add(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            balance += amount;
        }
    }
}