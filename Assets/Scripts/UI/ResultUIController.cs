using TMPro;
using UnityEngine;

namespace UnitySlotMachine.UI
{
    public class ResultUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text payoutText;

        public void ShowWin(int payout)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultText != null)
            {
                resultText.text = "WIN!";
            }

            if (payoutText != null)
            {
                payoutText.text = $"+{payout}G";
            }
        }

        public void ShowJackpot(int payout)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultText != null)
            {
                resultText.text = "JACKPOT!";
            }

            if (payoutText != null)
            {
                payoutText.text = $"+{payout}G";
            }
        }

        public void ShowLoss()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultText != null)
            {
                resultText.text = "LOSS!";
            }

            if (payoutText != null)
            {
                payoutText.text = "0G";
            }
        }

        public void Hide()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
        }
    }
}