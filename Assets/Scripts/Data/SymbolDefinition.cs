using UnityEngine;

namespace UnitySlotMachine.Data
{
    [CreateAssetMenu(
        fileName = "SymbolDefinition",
        menuName = "Slot Machine/Symbol Definition"
    )]
    public class SymbolDefinition : ScriptableObject
    {
        [SerializeField]
        private SymbolType symbolType;

        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        [Min(0)]
        private int payoutMultiplier;

        public SymbolType SymbolType => symbolType;
        public Sprite Sprite => sprite;
        public int PayoutMultiplier => payoutMultiplier;
    }
}