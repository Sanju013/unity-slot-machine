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
        [SerializeField] private BetPopupController betPopupController;

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
                    "SlotMachineController requires at least one Reel Controller.",
                    this
                );

                return;
            }

            if (winEvaluator == null)
            {
                Debug.LogError(
                    "SlotMachineController requires a WinEvaluator reference.",
                    this
                );

                return;
            }

            if (payoutManager == null)
            {
                Debug.LogError(
                    "SlotMachineController requires a PayoutManager reference.",
                    this
                );

                return;
            }

            if (betManager == null)
            {
                Debug.LogError(
                    "SlotMachineController requires a BetManager reference.",
                    this
                );

                return;
            }

            if (!betManager.HasBet)
            {
                Debug.LogWarning(
                    "Cannot spin without an active bet.",
                    this
                );

                return;
            }

            foreach (ReelController reel in reels)
            {
                if (reel == null)
                {
                    Debug.LogError(
                        "SlotMachineController contains a null Reel Controller.",
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

            currentSpinResult =
                resultGenerator.GenerateResult(reels.Length);

            for (int i = 0; i < currentSpinResult.ReelCount; i++)
            {
                Debug.Log(
                    $"Spin Result - Reel {i + 1}: " +
                    $"{currentSpinResult.GetSymbol(i).name}"
                );
            }

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

            bool won =
                winEvaluator.IsWinningResult(
                    currentSpinResult
                );

            if (won)
            {
                int payout =
                    payoutManager.AwardPayout();

                Debug.Log(
                    $"WIN! Payout: {payout}G"
                );
            }
            else
            {
                Debug.Log("LOSS.");
            }

            betManager.ClearBet();

            currentState =
                SlotMachineState.Betting;

            if (betPopupController != null)
            {
                betPopupController.Show();
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