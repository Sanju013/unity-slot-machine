using UnityEngine;
using UnitySlotMachine.Core;
using UnitySlotMachine.Data;
using UnitySlotMachine.Reels;

namespace UnitySlotMachine.Gameplay
{
    public class SlotMachineController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ReelController[] reels;
        [SerializeField] private ReelConfiguration reelConfiguration;

        [Header("Spin Settings")]
        [SerializeField] private float reelStartDelay = 0.15f;

        private SpinResultGenerator resultGenerator;

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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spin();
            }
        }

        public void Spin()
        {
            if (reels == null || reels.Length == 0)
            {
                Debug.LogError(
                    "SlotMachineController requires at least one Reel Controller.",
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

            SpinResult result =
                resultGenerator.GenerateResult(reels.Length);

            for (int i = 0; i < result.ReelCount; i++)
            {
                Debug.Log(
                    $"Spin Result - Reel {i + 1}: {result.GetSymbol(i).name}"
                );
            }

            StartCoroutine(
                StartReelsSequentially(result)
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
        }
    }
}