using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySlotMachine.Gameplay;

namespace UnitySlotMachine.UI
{
    public class BetPopupController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BetManager betManager;
        [SerializeField] private Wallet wallet;
        [SerializeField] private SlotMachineController slotMachineController;

        [Header("UI References")]
        [SerializeField] private Button bet10Button;
        [SerializeField] private Button bet50Button;
        [SerializeField] private Button bet100Button;
        [SerializeField] private TMP_Text balanceText;
        [SerializeField] private ResultUIController resultUIController;

        private void OnEnable()
        {
            RefreshUI();
        }

        private void Start()
        {
            bet10Button.onClick.AddListener(OnBet10Clicked);
            bet50Button.onClick.AddListener(OnBet50Clicked);
            bet100Button.onClick.AddListener(OnBet100Clicked);

            RefreshUI();
        }

        private void OnDestroy()
        {
            if (bet10Button != null)
            {
                bet10Button.onClick.RemoveListener(OnBet10Clicked);
            }

            if (bet50Button != null)
            {
                bet50Button.onClick.RemoveListener(OnBet50Clicked);
            }

            if (bet100Button != null)
            {
                bet100Button.onClick.RemoveListener(OnBet100Clicked);
            }
        }

        private void OnBet10Clicked()
        {
            SelectBet(10);
        }

        private void OnBet50Clicked()
        {
            SelectBet(50);
        }

        private void OnBet100Clicked()
        {
            SelectBet(100);
        }

        private void SelectBet(int amount)
        {
            if (betManager == null)
            {
                return;
            }

            if (!betManager.SelectBet(amount))
            {
                return;
            }

            if (!betManager.PlaceBet())
            {
                return;
            }

            if (resultUIController != null)
            {
                resultUIController.Hide();
            }

            if (slotMachineController != null)
            {
                slotMachineController.SetReadyToSpin();
            }

            gameObject.SetActive(false);
        }

        public void RefreshUI()
        {
            if (wallet == null || betManager == null)
            {
                return;
            }

            balanceText.text =
                $"Balance: {wallet.Balance}G";

            bet10Button.interactable =
                betManager.CanAffordBet(10);

            bet50Button.interactable =
                betManager.CanAffordBet(50);

            bet100Button.interactable =
                betManager.CanAffordBet(100);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            RefreshUI();
        }
    }
}