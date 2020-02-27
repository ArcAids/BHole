using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaped : MonoBehaviour
{
    [SerializeField]
    GameObject gameWonUI;
    [SerializeField]
    GameObject inceptionUI;
   public void Win()
    {
        if(gameWonUI!=null)
            gameWonUI?.SetActive(true);
        GameManager.Instance.Escaped();
    }

    public void FinalWin()
    {
        if (gameWonUI != null)
            gameWonUI?.SetActive(true);
        GameManager.Instance.Win();
    }

    public void Inception()
    {
        if(inceptionUI!=null)
            inceptionUI.SetActive(true);
        GameManager.Instance.GameOverFromWormHole();
    }
}
