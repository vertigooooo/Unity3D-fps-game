using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//提供所有按键触发的函数
public class ButtonAction : MonoBehaviour{
    public void EnterGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ReturnHomeUI()
    {
        SceneManager.LoadScene("HomeUI");
    }
}
