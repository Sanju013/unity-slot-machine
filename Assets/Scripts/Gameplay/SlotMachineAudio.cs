using UnityEngine;

namespace UnitySlotMachine.Gameplay
{
    public class SlotMachineAudio : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip spinSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip jackpotSound;
        [SerializeField] private AudioClip lossSound;
        [SerializeField] private AudioClip resetSound;

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (audioSource == null)
            {
                Debug.LogError(
                    "SlotMachineAudio requires an AudioSource.",
                    this
                );
            }
        }

        public void PlaySpin()
        {
            PlayClip(spinSound);
        }

        public void PlayWin()
        {
            PlayClip(winSound);
        }

        public void PlayJackpot()
        {
            PlayClip(jackpotSound);
        }

        public void PlayLoss()
        {
            PlayClip(lossSound);
        }

        public void PlayReset()
        {
            PlayClip(resetSound);
        }

        private void PlayClip(AudioClip clip)
        {
            if (audioSource == null || clip == null)
            {
                return;
            }

            audioSource.PlayOneShot(clip);
        }
    }
}