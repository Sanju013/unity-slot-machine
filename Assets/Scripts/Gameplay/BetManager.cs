using UnityEngine;

namespace UnitySlotMachine.Gameplay
{
    public class BetManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Wallet wallet;

        [Header("Bet Options")]
        [SerializeField] private int[] betOptions = { 10, 50, 100 };

        private int currentBet;

        public int CurrentBet => currentBet;
        public bool HasBet => currentBet > 0;

        public bool SelectBet(int amount)
        {
            if (!IsValidBet(amount))
            {
                return false;
            }

            if (!wallet.CanAfford(amount))
            {
                return false;
            }

            currentBet = amount;

            return true;
        }

        public bool PlaceBet()
        {
            if (!HasBet)
            {
                return false;
            }

            if (!wallet.Spend(currentBet))
            {
                return false;
            }

            return true;
        }

        public void ClearBet()
        {
            currentBet = 0;
        }

        public int GetCurrentBet()
        {
            return currentBet;
        }

        public bool IsValidBet(int amount)
        {
            for (int i = 0; i < betOptions.Length; i++)
            {
                if (betOptions[i] == amount)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanAffordBet(int amount)
        {
            return IsValidBet(amount)
                && wallet.CanAfford(amount);
        }
    }
}