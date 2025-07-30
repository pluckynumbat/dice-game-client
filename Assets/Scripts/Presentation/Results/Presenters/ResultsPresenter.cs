using TMPro;
using UnityEngine;

namespace Presentation.Results.Presenters
{
    /// <summary>
    /// This shows the message(s) based on level win / loss like which new level was unlocked etc
    /// </summary>
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject winOnlyArea;
        
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI energyRewardText;
        [SerializeField] private TextMeshProUGUI levelUnlockedText;

        public void Init(bool won, int energyReward, bool unlockedNewLevel, int levelUnlocked)
        {
            winOnlyArea?.SetActive(won);
            
            titleText.text = won ? "Level Won!" : "Level Lost";
            energyRewardText.text = won ? $"Energy Won: {energyReward}" : "";
            levelUnlockedText.text = unlockedNewLevel ? $"New Level: {levelUnlocked}" : "";
        }
    }
}
