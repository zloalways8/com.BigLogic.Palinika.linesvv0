using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererSettings : MonoBehaviour
{
    [Header("Настройки LineRenderer")]
    public float startWidth = 0.1f; // Начальная ширина линии
    public float endWidth = 0.1f;   // Конечная ширина линии
    public Color startColor = Color.white; // Начальный цвет линии
    public Color endColor = Color.white;   // Конечный цвет линии
    public Material lineMaterial;         // Материал для LineRenderer

    private LineRenderer lineRenderer;

    void Start()
    {
        // Получаем LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        // Устанавливаем настройки из инспектора
        ApplySettings();
    }

    void Update()
    {
        if (lineRenderer.positionCount > 0)
        {
            Debug.Log("LineRenderer is drawing with " + lineRenderer.positionCount + " points.");
        }
    }

    void ApplySettings()
    {
        if (lineRenderer == null) return;

        // Устанавливаем начальную и конечную ширину
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;

        // Устанавливаем начальный и конечный цвет
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        // Если материал указан, применяем его
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        else
        {
            // Назначаем стандартный материал, если материал не указан
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        // Важно: установите `useWorldSpace` в false, чтобы линии рисовались в локальных координатах UI
        lineRenderer.useWorldSpace = false;
    }
}