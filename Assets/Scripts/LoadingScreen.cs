using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingObject; // Объект загрузки
    [SerializeField] private float displayTime = 5f;    // Время отображения в секундах

    private void Start()
    {
        if (loadingObject != null)
        {
            loadingObject.SetActive(true); // Показываем объект
            Invoke("HideLoadingObject", displayTime); // Скрываем через заданное время
        }
        else
        {
            Debug.LogWarning("Loading object не назначен в инспекторе.");
        }
    }

    private void HideLoadingObject()
    {
        if (loadingObject != null)
        {
            loadingObject.SetActive(false); // Скрываем объект
        }
    }
}