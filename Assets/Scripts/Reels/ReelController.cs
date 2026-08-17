using System.Collections.Generic;
using UnityEngine;
using UnitySlotMachine.Data;

namespace UnitySlotMachine.Reels
{
    public class ReelController : MonoBehaviour
    {
        [Header("Reel References")]
        [SerializeField] private RectTransform symbolStrip;
        [SerializeField] private ReelSymbol symbolPrefab;

        [Header("Reel Data")]
        [SerializeField] private ReelConfiguration reelConfiguration;

        [Header("Spin Settings")]
        [SerializeField] private float spinDuration = 1.5f;
        [SerializeField] private float spinSpeed = 900f;
        [SerializeField] private float symbolHeight = 96f;
        [SerializeField] private int visibleBufferSymbols = 2;

        private readonly List<ReelSymbol> reelSymbols = new();

        private bool isSpinning;

        public bool IsSpinning => isSpinning;

        private void Awake()
        {
            BuildSymbolStrip();
        }

        private void BuildSymbolStrip()
        {
            if (symbolStrip == null)
            {
                Debug.LogError("ReelController requires a Symbol Strip reference.", this);
                return;
            }

            if (symbolPrefab == null)
            {
                Debug.LogError("ReelController requires a Reel Symbol prefab.", this);
                return;
            }

            if (reelConfiguration == null)
            {
                Debug.LogError("ReelController requires a Reel Configuration.", this);
                return;
            }

            ClearSymbolStrip();

            int symbolCount = reelConfiguration.Symbols.Count + visibleBufferSymbols;

            for (int i = 0; i < symbolCount; i++)
            {
                SymbolDefinition definition =
                    reelConfiguration.Symbols[i % reelConfiguration.Symbols.Count];

                ReelSymbol symbol = Instantiate(symbolPrefab, symbolStrip);

                RectTransform symbolTransform =
                    symbol.GetComponent<RectTransform>();

                symbolTransform.anchoredPosition =
                    new Vector2(0f, -i * symbolHeight);

                symbol.SetSymbol(definition);

                reelSymbols.Add(symbol);
            }
        }

        private void ClearSymbolStrip()
        {
            reelSymbols.Clear();

            for (int i = symbolStrip.childCount - 1; i >= 0; i--)
            {
                Destroy(symbolStrip.GetChild(i).gameObject);
            }
        }

        public void StartSpin()
        {
            if (isSpinning)
            {
                return;
            }

            StartCoroutine(SpinRoutine());
        }

        private System.Collections.IEnumerator SpinRoutine()
        {
            isSpinning = true;

            float elapsedTime = 0f;

            while (elapsedTime < spinDuration)
            {
                float movement = spinSpeed * Time.deltaTime;

                symbolStrip.anchoredPosition += Vector2.down * movement;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            isSpinning = false;
        }
    }
}