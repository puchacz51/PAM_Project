using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public Sprite nowyObraz; // Ustaw nowy obraz w inspektorze Unity

    private Button przycisk;

    void Start()
    {
        // Pobierz komponent przycisku
        przycisk = GetComponent<Button>();

        // Dodaj naszą funkcję do obsługi zdarzenia kliknięcia
        if (przycisk != null)
        {
            przycisk.onClick.AddListener(handleButtonClick);
        }

    }

    void handleButtonClick()
    {
        // Sprawdź, czy nowy obraz został przypisany
        if (nowyObraz != null)
        {
            // Zmień obraz w komponencie Image przycisku
            Image obrazPrzycisku = przycisk.GetComponent<Image>();
            if (obrazPrzycisku != null)
            {
                obrazPrzycisku.sprite = nowyObraz;
            }
 
        }

    }
    public void initGame()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.btnPlaySound);

        SceneManager.LoadScene("Game");
    }
}
