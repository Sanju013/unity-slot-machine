using UnityEngine;
using UnitySlotMachine.Core;

namespace UnitySlotMachine.Gameplay
{
    public class WinEvaluator : MonoBehaviour
    {
        public bool IsWinningResult(SpinResult result)
        {
            if (result == null || result.ReelCount < 3)
            {
                return false;
            }

            return result.GetSymbol(0) == result.GetSymbol(1)
                && result.GetSymbol(1) == result.GetSymbol(2);
        }
    }
}