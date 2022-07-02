using PSG.BattlefieldAndGuns.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSG.BattlefieldAndGuns.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene(Constants.SCENE_MANAGEMENT_MARS_INDEX);
        }

        public void Settings()
        {

        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
