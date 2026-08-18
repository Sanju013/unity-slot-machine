using UnityEngine;

namespace UnitySlotMachine.Gameplay
{
    public class PayoutManager : MonoBehaviour
    {
        
        [SerializeField] private Wallet wallet;
        [SerializeField] private BetManager betManager;

        [SerializeField] private float smallBetMultiplier = 3f;
        [SerializeField] private float mediumBetMultiplier = 5f;
        [SerializeField] private float largeBetMultiplier = 15f;

        [SerializeField] private int jackpotPayout = 5000;

        public int CalculatePayout()
        {
            if (betManager == null)
            {
                return 0;
            }

            int currentBet = betManager.CurrentBet;

            switch (currentBet)
            {
                case 10:
                    return Mathf.RoundToInt(
                        currentBet * smallBetMultiplier
                    );

                case 50:
                    return Mathf.RoundToInt(
                        currentBet * mediumBetMultiplier
                    );

                case 100:
                    return Mathf.RoundToInt(
                        currentBet * largeBetMultiplier
                    );

                default:
                    return 0;
            }
        }

        public int AwardPayout()
        {
            int payout = CalculatePayout();

            if (payout <= 0 || wallet == null)
            {
                return 0;
            }

            wallet.Add(payout);

            return payout;
        }

        public int AwardJackpot()
        {
            if (wallet == null)
            {
                return 0;
            }

            wallet.Add(jackpotPayout);

            return jackpotPayout;
        }
    }
}