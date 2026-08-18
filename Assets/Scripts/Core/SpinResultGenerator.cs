using System;
using UnityEngine;
using UnitySlotMachine.Data;

namespace UnitySlotMachine.Core
{
    public sealed class SpinResultGenerator
    {
        private readonly ReelConfiguration reelConfiguration;

        public SpinResultGenerator(ReelConfiguration configuration)
        {
            reelConfiguration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            if (reelConfiguration.Symbols.Count == 0)
            {
                throw new ArgumentException(
                    "one symbol need by reel config.",
                    nameof(configuration)
                );
            }
        }

        public SpinResult GenerateResult(int reelCount)
        {
            if (reelCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reelCount));
            }

            SymbolDefinition[] results = new SymbolDefinition[reelCount];

            for (int i = 0; i < reelCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(
                    0,
                    reelConfiguration.Symbols.Count
                );

                results[i] = reelConfiguration.Symbols[randomIndex];
            }

            return new SpinResult(results);
        }
    }
}