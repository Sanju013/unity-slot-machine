using UnityEngine;
using UnityEngine.UI;
using UnitySlotMachine.Data;

namespace UnitySlotMachine.Reels
{
    public class ReelSymbol : MonoBehaviour
    {
        [SerializeField]
        private Image symbolImage;

        private SymbolDefinition symbolDefinition;

        public SymbolDefinition SymbolDefinition => symbolDefinition;

        public void SetSymbol(SymbolDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogError("ReelSymbol received a null SymbolDefinition.", this);
                return;
            }

            symbolDefinition = definition;
            symbolImage.sprite = definition.Sprite;
        }
    }
}