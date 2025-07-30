using TMPro;
using UnityEngine;

namespace Presentation.Error.Presenters
{
    /// <summary>
    /// show the required error message based on the error
    /// </summary>
    public class ErrorPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI buttonText;

        public void Init(string title, string description, string actionText)
        {
            titleText.text = title;
            descriptionText.text = description;
            buttonText.text = actionText;
        }
    }
}
