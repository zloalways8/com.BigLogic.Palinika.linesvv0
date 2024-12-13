using UnityEngine;
using UnityEngine.UI;

public class BackgroundSelectionMenu : MonoBehaviour
{
    public GameObject[] backgroundImages; // Массив для фонов (Images)
    public Button[] backgroundButtons;    // Массив для кнопок выбора фонов

    private int selectedBackgroundIndex = 0; // Индекс текущего выбранного фона

    void Start()
    {
        // Загружаем сохраненный фон
        LoadSelectedBackground();

        // Настроим кнопки для выбора фона
        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            int index = i; // Локальная копия индекса для кнопки
            backgroundButtons[i].onClick.AddListener(() => OnBackgroundSelected(index));
        }

        // Отобразим фоны в соответствии с выбранным фоном
        SetBackground(selectedBackgroundIndex);
    }

    // Метод для установки выбранного фона
    void SetBackground(int index)
    {
        if (index < 0 || index >= backgroundImages.Length)
            return;

        // Скрываем все фоны
        foreach (var bg in backgroundImages)
        {
            bg.SetActive(false);
        }

        // Активируем только выбранный фон
        backgroundImages[index].SetActive(true);

        // Сохраняем выбор фона
        SaveSelectedBackground(index);
    }

    // Метод для сохранения выбранного фона в PlayerPrefs
    void SaveSelectedBackground(int index)
    {
        PlayerPrefs.SetInt("SelectedBackground", index);
        PlayerPrefs.Save();
    }

    // Метод для загрузки выбранного фона из PlayerPrefs
    void LoadSelectedBackground()
    {
        // Получаем индекс выбранного фона (по умолчанию 0)
        selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
    }

    // Метод для обработки выбора фона кнопкой
    void OnBackgroundSelected(int index)
    {
        SetBackground(index); // Устанавливаем выбранный фон
    }
}