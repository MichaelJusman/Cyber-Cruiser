using TMPro;
using UnityEngine;

namespace CyberCruiser
{
    public class CurrentMissionDisplay : GameBehaviour
    {
        [SerializeField] private TMP_Text _missionDescription, _missionProgress;

        private void OnEnable()
        {
            if (MissionManagerInstance.CurrentMission == null)
            {
                _missionDescription.text = "";
                _missionProgress.text = "";
            }
            else
            {
                _missionDescription.text = MissionManagerInstance.CurrentMission.missionDescription;
                _missionProgress.text = "Current progress: " + MissionManagerInstance.CurrentMissionProgress.ToString();
            }
        }
    }
}