using UnityEngine;
using UnityEngine.UI;
using UnitySlotMachine.Gameplay;

namespace UnitySlotMachine.UI
{
    public class LeverController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image leverImage;
        [SerializeField] private SlotMachineController slotMachineController;

        [Header("Lever Sprites")]
        [SerializeField] private Sprite unpulledSprite;
        [SerializeField] private Sprite pulledSprite;

        private Button leverButton;
        private bool leverPulled;

        private void Awake()
        {
            leverButton = GetComponent<Button>();

            if (leverImage == null)
            {
                Debug.LogError(
                    "Lever Image reference missing here.",
                    this
                );
            }

            if (slotMachineController == null)
            {
                Debug.LogError(
                    "slotmachinecontroller missing.",
                    this
                );
            }

            if (unpulledSprite == null)
            {
                Debug.LogError(
                    "unpulled lever missing.",
                    this
                );
            }

            if (pulledSprite == null)
            {
                Debug.LogError(
                    "pulled level is missing",
                    this
                );
            }

            if (leverButton == null)
            {
                Debug.LogError(
                    "component of button missing.",
                    this
                );
            }
        }

        private void Start()
        {
            leverButton.onClick.AddListener(PullLever);

            SetUnpulled();
            UpdateLeverInteractable();
        }

        private void Update()
        {
            UpdateLeverInteractable();

            if (!leverPulled)
            {
                return;
            }

            if (slotMachineController == null)
            {
                return;
            }

            if (!slotMachineController.IsSpinning &&
                slotMachineController.CurrentState != SlotMachineState.ReadyToSpin)
            {
                SetUnpulled();
            }
        }

        private void OnDestroy()
        {
            if (leverButton != null)
            {
                leverButton.onClick.RemoveListener(PullLever);
            }
        }

        private void PullLever()
        {
            if (slotMachineController == null)
            {
                return;
            }

            if (slotMachineController.CurrentState !=
                SlotMachineState.ReadyToSpin)
            {
                return;
            }

            SetPulled();

            slotMachineController.Spin();
        }

        private void UpdateLeverInteractable()
        {
            if (leverButton == null ||
                slotMachineController == null)
            {
                return;
            }

            leverButton.interactable =
                slotMachineController.CurrentState ==
                SlotMachineState.ReadyToSpin;
        }

        private void SetUnpulled()
        {
            leverImage.sprite = unpulledSprite;
            leverPulled = false;
        }

        private void SetPulled()
        {
            leverImage.sprite = pulledSprite;
            leverPulled = true;
        }
    }
}