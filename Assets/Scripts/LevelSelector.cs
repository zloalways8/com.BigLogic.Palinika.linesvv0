using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public Button button;  // Кнопка уровня
        public Image lockImage; // Изображение замка
    }

    public LevelButton[] levelButtons; // Массив кнопок с уровнями

    private void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Получаем последний разблокированный уровень (по умолчанию 1)

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 <= unlockedLevel)
            {
                // Уровень разблокирован
                levelButtons[i].button.interactable = true;
                levelButtons[i].lockImage.gameObject.SetActive(false); // Скрываем замок

                int levelIndex = i + 1; // Локальная копия переменной для лямбда-выражения
                levelButtons[i].button.onClick.AddListener(() => LoadLevel(levelIndex));
            }
            else
            {
                // Уровень заблокирован
                levelButtons[i].button.interactable = false;
                levelButtons[i].lockImage.gameObject.SetActive(true); // Показываем замок
            }
        }
    }

    public void UnlockNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1); // Разблокируем следующий уровень
        }
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }
}
