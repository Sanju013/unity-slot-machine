using System.Collections.Generic;
using UnityEngine;

namespace UnitySlotMachine.Data
{
    [CreateAssetMenu(
        fileName = "ReelConfiguration",
        menuName = "Slot Machine/Reel Configuration"
    )]
    public class ReelConfiguration : ScriptableObject
    {
        [SerializeField]
        private List<SymbolDefinition> symbols = new();

        public IReadOnlyList<SymbolDefinition> Symbols => symbols;
    }
}