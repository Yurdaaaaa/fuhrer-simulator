using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.btnPlaySound);

        SceneManager.LoadScene("Game");
    }
}
