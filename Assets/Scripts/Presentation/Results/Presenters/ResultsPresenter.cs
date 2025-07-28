using TMPro;
using UnityEngine;

namespace Presentation.Results.Presenters
{
    /// <summary>
    /// This shows the message based on level win / loss
    /// </summary>
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        public void Init(string value)
        {
            labelText.text = value;
        }
    }
}
