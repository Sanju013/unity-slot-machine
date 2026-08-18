using System;
using UnitySlotMachine.Data;

namespace UnitySlotMachine.Core
{
    public sealed class SpinResult
    {
        public SymbolDefinition[] Symbols { get; }

        public int ReelCount => Symbols.Length;

        public SpinResult(SymbolDefinition[] symbols)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            if (symbols.Length == 0)
            {
                throw new ArgumentException(
                    "one symbol needed by spinresult",
                    nameof(symbols)
                );
            }

            Symbols = symbols;
        }

        public SymbolDefinition GetSymbol(int reelIndex)
        {
            if (reelIndex < 0 || reelIndex >= Symbols.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(reelIndex));
            }

            return Symbols[reelIndex];
        }
    }
}