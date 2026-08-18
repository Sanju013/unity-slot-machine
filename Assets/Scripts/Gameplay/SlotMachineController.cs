using UnityEngine;
using UnitySlotMachine.Core;
using UnitySlotMachine.Data;
using UnitySlotMachine.Reels;
using UnitySlotMachine.UI;

namespace UnitySlotMachine.Gameplay
{
    public class SlotMachineController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ReelController[] reels;
        [SerializeField] private ReelConfiguration reelConfiguration;
        [SerializeField] private WinEvaluator winEvaluator;
        [SerializeField] private PayoutManager payoutManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private Wallet wallet;
        [SerializeField] private BetPopupController betPopupController;
        [SerializeField] private ResultUIController resultUIController;
        [SerializeField] private ResetGameController resetGameController;
        [SerializeField] private SlotMachineAudio slotMachineAudio;

        [Header("Spin Settings")]
        [SerializeField] private float reelStartDelay = 0.15f;

        private SpinResultGenerator resultGenerator;
        private SpinResult currentSpinResult;

        private SlotMachineState currentState;

        public SlotMachineState CurrentState => currentState;

        public bool IsSpinning =>
            currentState == SlotMachineState.Spinning;

        public SpinResult CurrentSpinResult =>
            currentSpinResult;

        private void Awake()
        {
            if (reelConfiguration == null)
            {
                Debug.LogError(
                    "SlotMachineController requires a Reel Configuration.",
                    this
                );

                return;
            }

            resultGenerator =
                new SpinResultGenerator(reelConfiguration);

            currentState = SlotMachineState.Betting;
        }

        public void Spin()
        {
            if (currentState != SlotMachineState.ReadyToSpin)
            {
                return;
            }

            if (reels == null || reels.Length == 0)
            {
                Debug.LogError(
                    "one reel controller",
                    this
                );

                return;
            }

            if (winEvaluator == null)
            {
                Debug.LogError(
                    "winevaluator missing",
                    this
                );

                return;
            }

            if (payoutManager == null)
            {
                Debug.LogError(
                    "payout manager is missing",
                    this
                );

                return;
            }

            if (betManager == null)
            {
                Debug.LogError(
                    "betmanager is missing",
                    this
                );

                return;
            }

            if (!betManager.HasBet)
            {
                Debug.LogWarning(
                    "active bet is needed here",
                    this
                );

                return;
            }

            foreach (ReelController reel in reels)
            {
                if (reel == null)
                {
                    Debug.LogError(
                        "reel controller is null",
                        this
                    );

                    return;
                }

                if (reel.IsSpinning)
                {
                    return;
                }
            }

            currentState = SlotMachineState.Spinning;
            if (slotMachineAudio != null)
            {
                slotMachineAudio.PlaySpin();
            }

            currentSpinResult =
                resultGenerator.GenerateResult(reels.Length);

            StartCoroutine(
                StartReelsSequentially(currentSpinResult)
            );
        }

        private System.Collections.IEnumerator StartReelsSequentially(
            SpinResult result)
        {
            for (int i = 0; i < reels.Length; i++)
            {
                reels[i].StartSpin(
                    result.GetSymbol(i)
                );

                if (i < reels.Length - 1)
                {
                    yield return new WaitForSeconds(
                        reelStartDelay
                    );
                }
            }

            yield return new WaitUntil(
                () => !AreReelsSpinning()
            );

            ProcessSpinResult();
        }

        private bool AreReelsSpinning()
        {
            foreach (ReelController reel in reels)
            {
                if (reel != null && reel.IsSpinning)
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessSpinResult()
        {
            currentState =
                SlotMachineState.ProcessingResult;

            bool isJackpot =
                winEvaluator.IsJackpotResult(
                    currentSpinResult
                );

            bool won =
                winEvaluator.IsWinningResult(
                    currentSpinResult
                );

            if (isJackpot)
            {
                if (slotMachineAudio != null)
                {
                    slotMachineAudio.PlayJackpot();
                }
                int normalPayout =
                    payoutManager.CalculatePayout();

                int jackpotBonus =
                    payoutManager.AwardJackpot();

                int totalPayout =
                    normalPayout + jackpotBonus;

                if (normalPayout > 0)
                {
                    wallet.Add(normalPayout);
                }

                if (resultUIController != null)
                {
                    resultUIController.ShowJackpot(totalPayout);
                }
            }
            else if (won)
            {
                if (slotMachineAudio != null)
                {
                    slotMachineAudio.PlayWin();
                }
                int payout =
                    payoutManager.AwardPayout();

                if (resultUIController != null)
                {
                    resultUIController.ShowWin(payout);
                }
            }
            else
            {
                if (slotMachineAudio != null)
                {
                    slotMachineAudio.PlayLoss();
                }
                if (resultUIController != null)
                {
                    resultUIController.ShowLoss();
                }
            }

            betManager.ClearBet();

            currentState =
                SlotMachineState.Betting;

            if (betPopupController != null)
            {
                betPopupController.Show();
            }

            if (!won &&
                !isJackpot &&
                wallet != null &&
                wallet.Balance <= 0 &&
                resetGameController != null)
            {
                resetGameController.Show();
            }
        }

        public void SetReadyToSpin()
        {
            if (currentState != SlotMachineState.Betting)
            {
                return;
            }

            if (betManager == null || !betManager.HasBet)
            {
                return;
            }

            currentState =
                SlotMachineState.ReadyToSpin;
        }
    }
}