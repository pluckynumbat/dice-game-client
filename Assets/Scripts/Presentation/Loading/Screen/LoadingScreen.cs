using System.Threading.Tasks;
using Presentation.Loading.Views;
using UnityEngine;

namespace Presentation.Loading.Screen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private LoadingBarPresenter loadingBarPresenter;

        private void OnEnable()
        {
            loadingBarPresenter.SetProgress(0);
        }

        public async Task ShowProgress(float finalValue, float totalTime)
        {
            await loadingBarPresenter.FillUpto(finalValue, totalTime);
        }
    }
}
