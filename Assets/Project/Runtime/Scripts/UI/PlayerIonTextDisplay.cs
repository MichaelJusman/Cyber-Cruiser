using TMPro;
using UnityEngine;

namespace CyberCruiser
{
    [RequireComponent(typeof(TMP_Text))]
    public class PlayerIonTextDisplay : GameBehaviour
    {
        private TMP_Text _ionTMPText;

        private void Awake()
        {
            _ionTMPText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            PlayerStatsManager.OnIonChange += UpdateIonText;
            UpdateIonText(PlayerStatsManagerInstance.PlayerIon);
        }

        private void OnDisable()
        {
            PlayerStatsManager.OnIonChange -= UpdateIonText;
        }

        private void UpdateIonText(int ion)
        {
            _ionTMPText.text = ion.ToString();
        }
    }
}