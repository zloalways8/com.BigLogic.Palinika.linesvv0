using UnityEngine;
using UnityEngine.UI;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject[] objects; // Массив объектов для переключения
    public Button nextButton;    // Кнопка "Вперед"
    public Button prevButton;    // Кнопка "Назад"

    private int currentIndex = 0; // Текущий индекс активного объекта

    void Start()
    {
        // Привязка кнопок к методам
        nextButton.onClick.AddListener(SwitchToNext);
        prevButton.onClick.AddListener(SwitchToPrevious);

        // Инициализация: отображение только первого объекта
        UpdateObjects();
    }

    // Переключение на следующий объект
    public void SwitchToNext()
    {
        currentIndex++;
        if (currentIndex >= objects.Length) // Если индекс превышает количество объектов
        {
            currentIndex = 0; // Вернуться к первому объекту
        }
        UpdateObjects();
    }

    // Переключение на предыдущий объект
    public void SwitchToPrevious()
    {
        currentIndex--;
        if (currentIndex < 0) // Если индекс становится меньше 0
        {
            currentIndex = objects.Length - 1; // Перейти к последнему объекту
        }
        UpdateObjects();
    }

    // Обновление отображения объектов
    private void UpdateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex); // Активировать только текущий объект
        }
    }
}