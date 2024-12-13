using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем TextMeshPro

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImage; // Компонент Image для фона
    public Sprite[] backgroundSprites; // Массив фоновых изображений
    public Button[] backgroundButtons; // Кнопки для выбора фона
    public int[] backgroundCosts; // Стоимость каждого фона (первый фон бесплатный)
    public TMP_Text[] buttonTexts; // TextMeshPro для отображения цены или статуса на кнопках

    private const string BackgroundKey = "SelectedBackground"; // Ключ для сохранения выбранного фона
    private const string PurchasedBackgroundsKey = "PurchasedBackgrounds"; // Ключ для хранения купленных фонов
    private int balance = 0; // Текущий баланс игрока

    void Start()
    {
        LoadBalance();
        LoadPurchasedBackgrounds();

        int savedIndex = PlayerPrefs.GetInt(BackgroundKey, 0); // Загрузка сохранённого фона (по умолчанию 0)
        ApplyBackground(savedIndex);
        UpdateButtons();
    }

    public void ChangeBackground(int index)
{
    if (index >= 0 && index < backgroundSprites.Length)
    {
        bool isPurchased = IsBackgroundPurchased(index);

        if (isPurchased)
        {
            ApplyBackground(index);
            PlayerPrefs.SetInt(BackgroundKey, index);
            PlayerPrefs.Save();
            Debug.Log($"Фон {index} выбран.");
            UpdateButtons(); // Обновляем кнопки после выбора
        }
        else if (balance >= backgroundCosts[index])
        {
            // Покупка фона
            balance -= backgroundCosts[index];
            SaveBalance();
            PurchaseBackground(index);
            ApplyBackground(index);
            PlayerPrefs.SetInt(BackgroundKey, index);
            PlayerPrefs.Save();
            Debug.Log($"Баланс после покупки: {balance}");
            UpdateButtons(); // Обновляем кнопки после покупки
        }
        else
        {
            Debug.Log("Недостаточно очков для покупки этого фона!");
        }
    }
}


    private void ApplyBackground(int index)
    {
        backgroundImage.sprite = backgroundSprites[index];
        Debug.Log($"Фон {index} применён.");
    }

    private void UpdateButtons()
    {
        int selectedIndex = PlayerPrefs.GetInt(BackgroundKey, 0); // Получаем индекс выбранного фона

        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            if (i == selectedIndex)
            {
                // Если фон выбран, текст "Selected"
                buttonTexts[i].text = "Selected";
            }
            else if (IsBackgroundPurchased(i))
            {
                // Если фон куплен, текст "Выбрать"
                buttonTexts[i].text = "Select";
            }
            else
            {
                // Если фон не куплен, отображаем его стоимость
                buttonTexts[i].text = $"{backgroundCosts[i]}";
            }
        }
    }

    private void PurchaseBackground(int index)
    {
        string purchased = PlayerPrefs.GetString(PurchasedBackgroundsKey, "");
        purchased += index + ",";
        PlayerPrefs.SetString(PurchasedBackgroundsKey, purchased);
        PlayerPrefs.Save();
        Debug.Log($"Фон {index} куплен.");
    }

    private bool IsBackgroundPurchased(int index)
    {
        if (index == 0) return true; // Первый фон всегда доступен
        string purchased = PlayerPrefs.GetString(PurchasedBackgroundsKey, "");
        return purchased.Contains(index.ToString() + ",");
    }

    private void LoadBalance()
    {
        balance = PlayerPrefs.GetInt("Balance", 0);
        Debug.Log($"Загруженный баланс: {balance}");
    }

    private void SaveBalance()
    {
        Debug.Log($"Сохранение баланса: {balance}");
        PlayerPrefs.SetInt("Balance", balance);
        PlayerPrefs.Save();
    }

    private void LoadPurchasedBackgrounds()
    {
        if (!PlayerPrefs.HasKey(PurchasedBackgroundsKey))
        {
            PlayerPrefs.SetString(PurchasedBackgroundsKey, "0,"); // Первый фон доступен по умолчанию
            PlayerPrefs.Save();
        }
    }

    public void AddPoints(int points)
    {
        balance += points;
        Debug.Log($"Баланс после добавления очков: {balance}");
        SaveBalance();
    }
}
