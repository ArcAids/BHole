using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSettingsUI
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        Button headerPrefab;
        [SerializeField]
        List<Tab> tabs;

        Tab selectedTab;

        private void Awake()
        {
            foreach (var tab in tabs)
            {
                Button tabHeader = Instantiate(headerPrefab, transform);
                tab.tabHeader = tabHeader;
                tab.tabHeader.onClick.AddListener(delegate { tab.SelectTab(ref selectedTab); });
                tab.DeSelectTab();
                tab.tabHeader.GetComponentInChildren<TMPro.TMP_Text>().text = tab.title;
            }

        }

        private void OnEnable()
        {
            if (tabs.Count > 0)
                tabs[0].SelectTab(ref selectedTab);
        }

        [System.Serializable]
        class Tab
        {
            [HideInInspector]
            public Button tabHeader;
            public string title;
            public GameObject body;

            public void SelectTab(ref Tab selectedTab)
            {
                selectedTab?.DeSelectTab();
                body.SetActive(true);
                tabHeader.interactable = false;
                selectedTab = this;
            }

            public void DeSelectTab()
            {
                body.SetActive(false);
                tabHeader.interactable = true;
            }
        }
    }
}
