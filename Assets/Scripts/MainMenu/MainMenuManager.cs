using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using levelManagement;
namespace GameSettingsUI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        GameSettingsMenu gameSettings;
        [SerializeField]
        SceneController loader;
        [SerializeField]
        bool showPromptBeforeExit=true;
        [SerializeField]
        GameObject exitPrompt;

        private void Start()
        {
            if(GameManager.Instance!=null)
                Destroy(GameManager.Instance.gameObject);
            gameSettings.LoadSettings();
            Cursor.lockState = CursorLockMode.None;
        }

        public void LoadGame()
        {
            loader.Load(1);
        }

        public void AttemptExitGame()
        {
            if (showPromptBeforeExit)
                exitPrompt.SetActive(true);
            else
                QuitGame();
        }

        public void QuitGame()
        {
                Application.Quit();
        }
    }
}
