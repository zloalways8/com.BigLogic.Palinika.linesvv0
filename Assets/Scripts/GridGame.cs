using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GridGame : MonoBehaviour
{
    public GameObject[] gridCells; // Все ячейки 3x3
    public LineRenderer lineRenderer; // LineRenderer для рисования линий
    public Canvas canvas; // Ссылка на Canvas
    public GameObject popupVictory; // Попап "Игра пройдена"
    public TMP_Text balanceText; // Ссылка на UI TextMeshPro для отображения баланса
    public TMP_Text secondaryBalanceText; // Второй объект для отображения баланса

    public TMP_Text temporaryScoreText; // Ссылка на UI TextMeshPro для временного отображения баллов

    // Попапы
    public GameObject musicSettingsPopup; // Попап с настройками музыки
    public GameObject gameInfoPopup; // Попап с информацией об игре
    public GameObject storePopup; // Попап магазина

    private List<Transform> selectedCells = new List<Transform>(); // Пройденные ячейки
    private bool isDrawing = false;
    private bool gameFinished = false; // Флаг завершения игры
    private int balance = 0; // Баланс очков (начальный баланс 0)

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer не привязан в инспекторе!");
        }

        lineRenderer.useWorldSpace = false; // Убедимся, что LineRenderer работает в 2D пространстве
        LoadBalance(); // Загружаем баланс из PlayerPrefs при старте
        UpdateBalanceUI(); // Инициализация баланса на UI
    }

    void Update()
    {
        if (gameFinished)
            return;

        // Логика рисования линии
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            foreach (var cell in gridCells)
            {
                if (IsInsideCell(cell, worldPosition))
                {
                    string cellValue = GetCellValue(cell);
                    if (cellValue == "1")
                    {
                        isDrawing = true;
                        lineRenderer.positionCount = 0;
                        selectedCells.Clear();
                        AddCellToLine(cell.transform);
                        return;
                    }
                }
            }
        }

        // Продолжение рисования линии
        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            foreach (var cell in gridCells)
            {
                if (IsInsideCell(cell, worldPosition) && !selectedCells.Contains(cell.transform))
                {
                    if (selectedCells.Count > 0)
                    {
                        Transform lastCell = selectedCells[selectedCells.Count - 1];
                        if (IsAdjacent(lastCell, cell.transform))
                        {
                            AddCellToLine(cell.transform);
                        }
                    }
                    else
                    {
                        AddCellToLine(cell.transform);
                    }
                }
            }
        }

        // Завершение рисования
        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
            if (CheckVictoryCondition())
            {
                AddBalance(50); // Добавляем 50 к балансу за правильную линию
                ShowVictoryPopup();
                gameFinished = true; // Завершаем игру
            }
            else
            {
                ResetDrawing();
            }
        }
    }

    void AddCellToLine(Transform cell)
    {
        // Проверка, чтобы не добавить повторно уже выбранную ячейку
        if (!selectedCells.Contains(cell))
        {
            selectedCells.Add(cell);
            lineRenderer.positionCount = selectedCells.Count;

            // Преобразуем мировые координаты в локальные координаты канваса
            for (int i = 0; i < selectedCells.Count; i++)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedCells[i].position);
                Vector2 localPos = canvas.worldCamera.ScreenToWorldPoint(screenPos);
                lineRenderer.SetPosition(i, localPos);
            }
        }
    }

    bool IsInsideCell(GameObject cell, Vector3 position)
    {
        Collider2D collider = cell.GetComponent<Collider2D>();
        if (collider == null)
        {
            return false;
        }
        return collider.OverlapPoint(position);
    }

    bool IsAdjacent(Transform fromCell, Transform toCell)
    {
        int fromIndex = System.Array.IndexOf(gridCells, fromCell.gameObject);
        int toIndex = System.Array.IndexOf(gridCells, toCell.gameObject);

        if (fromIndex == -1 || toIndex == -1) return false;

        int rowFrom = fromIndex / 3; // Индекс строки
        int colFrom = fromIndex % 3; // Индекс столбца
        int rowTo = toIndex / 3; // Индекс строки
        int colTo = toIndex % 3; // Индекс столбца

        // Ячейки считаются соседними, если они находятся в одном ряду или в одном столбце
        return (Mathf.Abs(rowFrom - rowTo) == 1 && colFrom == colTo) || (Mathf.Abs(colFrom - colTo) == 1 && rowFrom == rowTo);
    }

    string GetCellValue(GameObject cell)
    {
        var number = cell.GetComponentInChildren<UnityEngine.UI.Text>();
        if (number != null) return number.text;

        var tmpNumber = cell.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpNumber != null) return tmpNumber.text;

        return "";
    }

    bool CheckVictoryCondition()
    {
        if (selectedCells.Count < 3) return false;

        List<string> sequence = new List<string>();
        foreach (var cell in selectedCells)
        {
            string value = GetCellValue(cell.gameObject);
            if (!string.IsNullOrEmpty(value)) sequence.Add(value);
        }

        return sequence.Count == 3 && sequence[0] == "1" && sequence[1] == "2" && sequence[2] == "3";
    }

    void ShowVictoryPopup()
    {
        if (popupVictory != null)
        {
            popupVictory.SetActive(true);
        }
    }

    void AddBalance(int points)
    {
        balance += points;
        UpdateBalanceUI(); // Обновляем UI с балансом
        ShowTemporaryScore(points); // Показываем временный счет
        SaveBalance(); // Сохраняем баланс в PlayerPrefs
    }

    void UpdateBalanceUI()
    {
        if (balanceText != null)
        {
            balanceText.text = "Balance: " + balance.ToString() + " points";
        }

        if (secondaryBalanceText != null)
        {
            secondaryBalanceText.text = "Balance: " + balance.ToString() + " points";
        }
    }


    void ShowTemporaryScore(int points)
    {
        if (temporaryScoreText != null)
        {
            temporaryScoreText.text = "+" + points.ToString();
            temporaryScoreText.gameObject.SetActive(true);

            // Скрываем текст через 3 секунды
            Invoke("HideTemporaryScore", 3f);
        }
    }

    void HideTemporaryScore()
    {
        if (temporaryScoreText != null)
        {
            temporaryScoreText.gameObject.SetActive(false);
        }
    }

    void ResetDrawing()
    {
        selectedCells.Clear();
        lineRenderer.positionCount = 0;
    }

    public void ResetGame()
    {
        lineRenderer.positionCount = 0;
        selectedCells.Clear();

        if (popupVictory != null)
        {
            popupVictory.SetActive(false);
        }

        gameFinished = false;

        // Убираем сброс баланса в 0
        // balance = 0;  // Эту строку нужно убрать

        UpdateBalanceUI();  // Обновляем UI с текущим балансом
    }


    // Сохранение баланса в PlayerPrefs
    void SaveBalance()
    {
        PlayerPrefs.SetInt("Balance", balance);
        PlayerPrefs.Save();
    }

    // Загрузка баланса из PlayerPrefs
    void LoadBalance()
    {
        balance = PlayerPrefs.GetInt("Balance", 0); // Если нет сохраненного баланса, ставим 0
    }

    // Открытие попапа настроек
    public void OpenSettingsPopup()
    {
        if (musicSettingsPopup != null)
        {
            musicSettingsPopup.SetActive(true);
        }
    }

    // Закрытие попапа настроек
    public void CloseSettingsPopup()
    {
        if (musicSettingsPopup != null)
        {
            musicSettingsPopup.SetActive(false);
        }
    }

    // Открытие попапа с информацией
    public void OpenInfoPopup()
    {
        if (gameInfoPopup != null)
        {
            gameInfoPopup.SetActive(true);
        }
    }

    // Закрытие попапа с информацией
    public void CloseInfoPopup()
    {
        if (gameInfoPopup != null)
        {
            gameInfoPopup.SetActive(false);
        }
    }

    // Открытие попапа магазина
    public void OpenStorePopup()
    {
        if (storePopup != null)
        {
            storePopup.SetActive(true);
        }
    }

    // Закрытие попапа магазина
    public void CloseStorePopup()
    {
        if (storePopup != null)
        {
            storePopup.SetActive(false);
        }
    }

    // Выход в меню
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Замените "MainMenu" на название вашей сцены меню
    }
    // Выход из игры
    public void ExitGame()
    {
        #if UNITY_EDITOR
            // Если в редакторе Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Если в сборке игры
            Application.Quit();
        #endif
    }
    
    public void GoToNextLevel2()
    {
        SceneManager.LoadScene("Level2"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel3()
    {
        SceneManager.LoadScene("Level3"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel4()
    {
        SceneManager.LoadScene("Level4"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel5()
    {
        SceneManager.LoadScene("Level5"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel6()
    {
        SceneManager.LoadScene("Level6"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel7()
    {
        SceneManager.LoadScene("Level7"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel8()
    {
        SceneManager.LoadScene("Level8"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel9()
    {
        SceneManager.LoadScene("Level9"); // Замените "MainMenu" на название вашей сцены меню
    }
    public void GoToNextLevel10()
    {
        SceneManager.LoadScene("Level10"); // Замените "MainMenu" на название вашей сцены меню
    }
    
}