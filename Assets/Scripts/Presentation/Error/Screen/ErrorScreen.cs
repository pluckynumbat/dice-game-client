using Model;
using Presentation.Error.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Error.Screen
{
    /// <summary>
    /// The ErrorScreen will display an error message on a popup along with a continue button
    /// </summary>
    public class ErrorScreen : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private ErrorPresenter errorPresenter;
    }
}
