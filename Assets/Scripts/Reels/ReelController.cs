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
        [SerializeField] private float symbolHeight = 110f;
        [SerializeField] private int visibleBufferSymbols = 2;
        [SerializeField] private float settleDuration = 0.25f;

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
                Debug.LogError(
                    "ReelController requires a Symbol Strip reference.",
                    this
                );

                return;
            }

            if (symbolPrefab == null)
            {
                Debug.LogError(
                    "ReelController requires a Reel Symbol prefab.",
                    this
                );

                return;
            }

            if (reelConfiguration == null)
            {
                Debug.LogError(
                    "ReelController requires a Reel Configuration.",
                    this
                );

                return;
            }

            ClearSymbolStrip();

            int symbolCount =
                reelConfiguration.Symbols.Count + visibleBufferSymbols;

            for (int i = 0; i < symbolCount; i++)
            {
                SymbolDefinition definition =
                    reelConfiguration.Symbols[
                        i % reelConfiguration.Symbols.Count
                    ];

                ReelSymbol symbol =
                    Instantiate(symbolPrefab, symbolStrip);

                RectTransform symbolTransform =
                    symbol.GetComponent<RectTransform>();

                symbolTransform.anchoredPosition =
                    new Vector2(
                        0f,
                        -i * symbolHeight
                    );

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

        public void StartSpin(SymbolDefinition targetSymbol)
        {
            if (isSpinning)
            {
                return;
            }

            if (targetSymbol == null)
            {
                Debug.LogError(
                    "ReelController received a null target symbol.",
                    this
                );

                return;
            }

            StartCoroutine(
                SpinRoutine(targetSymbol)
            );
        }

        private System.Collections.IEnumerator SpinRoutine(
            SymbolDefinition targetSymbol)
        {
            isSpinning = true;

            float elapsedTime = 0f;

           

            while (elapsedTime < spinDuration)
            {
                float movement =
                    spinSpeed * Time.deltaTime;

                symbolStrip.anchoredPosition +=
                    Vector2.down * movement;

                Transform firstSymbol =
                    symbolStrip.GetChild(0);

                RectTransform firstRect =
                    firstSymbol.GetComponent<RectTransform>();

                float firstWorldY =
                    symbolStrip.anchoredPosition.y +
                    firstRect.anchoredPosition.y;

                
                if (firstWorldY < -symbolHeight * 2f)
                {
                    Transform lastSymbol =
                        symbolStrip.GetChild(
                            symbolStrip.childCount - 1
                        );

                    RectTransform lastRect =
                        lastSymbol.GetComponent<RectTransform>();

                    firstRect.anchoredPosition =
                        new Vector2(
                            firstRect.anchoredPosition.x,
                            lastRect.anchoredPosition.y + symbolHeight
                        );

                    firstSymbol.SetAsLastSibling();
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            

            ReelSymbol centerSymbol = null;

            float closestDistance = float.MaxValue;

            for (int i = 0; i < symbolStrip.childCount; i++)
            {
                Transform child =
                    symbolStrip.GetChild(i);

                RectTransform childRect =
                    child.GetComponent<RectTransform>();

                float childY =
                    symbolStrip.anchoredPosition.y +
                    childRect.anchoredPosition.y;

                float distanceFromCenter =
                    Mathf.Abs(childY);

                if (distanceFromCenter < closestDistance)
                {
                    closestDistance = distanceFromCenter;

                    centerSymbol =
                        child.GetComponent<ReelSymbol>();
                }
            }

           
            if (centerSymbol != null)
            {
                
                centerSymbol.SetSymbol(targetSymbol);

                RectTransform centerTransform =
                    centerSymbol.GetComponent<RectTransform>();

                float startY =
                    symbolStrip.anchoredPosition.y;

                float currentSymbolY =
                    startY +
                    centerTransform.anchoredPosition.y;

                float targetStripY =
                    startY - currentSymbolY;

                float settleElapsed = 0f;

                while (settleElapsed < settleDuration)
                {
                    float t =
                        settleElapsed / settleDuration;

                    
                    t = t * t * (3f - 2f * t);

                    float newY =
                        Mathf.Lerp(
                            startY,
                            targetStripY,
                            t
                        );

                    symbolStrip.anchoredPosition =
                        new Vector2(
                            symbolStrip.anchoredPosition.x,
                            newY
                        );

                    settleElapsed += Time.deltaTime;

                    yield return null;
                }

             
                symbolStrip.anchoredPosition =
                    new Vector2(
                        symbolStrip.anchoredPosition.x,
                        targetStripY
                    );
            }
            else
            {
                Debug.LogError(
                    "ReelController could not find a center symbol.",
                    this
                );
            }

            isSpinning = false;
        }
    }
}