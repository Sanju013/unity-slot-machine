using UnityEngine;
using UnitySlotMachine.Gameplay;

namespace UnitySlotMachine.UI
{
    public class ResetGameController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject resetGamePanel;
        [SerializeField] private Wallet wallet;
        [SerializeField] private BetManager betManager;
        [SerializeField] private BetPopupController betPopupController;
        [SerializeField] private ResultUIController resultUIController;
        [SerializeField] private SlotMachineAudio slotMachineAudio;

        private void Awake()
        {
            Hide();
        }

        public void Show()
        {
            if (resetGamePanel != null)
            {
                resetGamePanel.SetActive(true);
            }
        }

        public void Hide()
        {
            if (resetGamePanel != null)
            {
                resetGamePanel.SetActive(false);
            }
        }

        public void ResetGame()
        {
            if (slotMachineAudio != null)
            {
                slotMachineAudio.PlayReset();
            }

            if (wallet != null)
            {
                wallet.ResetBalance();
            }

            if (betManager != null)
            {
                betManager.ClearBet();
            }

            if (resultUIController != null)
            {
                resultUIController.Hide();
            }

            Hide();

            if (betPopupController != null)
            {
                betPopupController.Show();
            }
        }
    }
}