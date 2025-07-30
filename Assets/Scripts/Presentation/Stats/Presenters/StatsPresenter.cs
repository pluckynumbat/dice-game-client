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
            // TODO: display a "no stats yet" message?
            
            if (cachedEntries == null)
            {
                cachedEntries = new List<StatEntryView>();
            }

            for (int index = 0; index < statsArray.Length; ++index)
            {
                PlayerLevelStats source = statsArray[index];
                StatEntryView destination;
                if (index < cachedEntries.Count) // re-use cached elements
                {
                    destination = cachedEntries[index];
                }
                else // not enough cached elements, create more
                {
                    destination = Instantiate(statsEntryViewPrefab, statsEntryContainerTransform)
                        .GetComponent<StatEntryView>();
                    cachedEntries.Add(destination);
                }

                if (destination != null)
                {
                    destination.gameObject.SetActive(true);
                    destination.Init(source.level, source.winCount, source.lossCount, source.bestScore);
                }
            }
            
            // disable extra cached entries
            if (cachedEntries.Count > statsArray.Length)
            {
                for (int index = statsArray.Length; index < cachedEntries.Count; ++index)
                {
                    cachedEntries[index]?.gameObject.SetActive(false);
                }
            }
        }
    }
}