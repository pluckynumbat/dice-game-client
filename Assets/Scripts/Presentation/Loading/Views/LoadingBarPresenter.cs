using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Loading.Views
{
    public class LoadingBarPresenter : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void SetProgress(float value)
        {
            slider.value = value;
        }
        
        public async Task FillUpto(float finalValue, float totalTime)
        {
            float initialValue = slider.value;
            float currentTime = 0f;
            while (currentTime < totalTime)
            {
                slider.value = Mathf.Lerp(initialValue, finalValue, currentTime);
                currentTime += Time.deltaTime;
                await Task.Yield();
            }
            slider.value = finalValue;
        }
    }
}