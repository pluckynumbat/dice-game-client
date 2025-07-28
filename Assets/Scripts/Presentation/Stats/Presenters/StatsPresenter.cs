using System.Collections.Generic;
using Network;
using Presentation.Stats.Views;
using UnityEngine;

namespace Presentation.Stats.Presenters
{
    /// <summary>
    /// This will display the current stat entries in the scroll view
    /// It prevents unnecessary instantiations by caching and re-using the existing stat entry views
    /// </summary>
    public class StatsPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject statsEntryViewPrefab;
        [SerializeField] private Transform statsEntryContainerTransform;

        private List<StatEntryView> cachedEntries;

        public void Init(PlayerLevelStats[] statsArray)
        {
        }
    }
}