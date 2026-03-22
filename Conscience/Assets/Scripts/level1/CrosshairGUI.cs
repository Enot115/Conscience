using UnityEngine;

public class CrosshairGUI : MonoBehaviour
{
    // 1. Публичная переменная для текстуры
    public Texture2D crosshairTexture;
    // 2. Размер прицела (ширина и высота в пикселях)
    public float size = 20;

    // 3. Специальный метод Unity для GUI
    void OnGUI()
    {
        // Проверка: если текстура не назначена, ничего не делаем
        if (crosshairTexture == null) return;

        // 4. Самое интересное: расчет позиции
        // Screen.width / 2 - центр экрана по X
        // Вычитаем половину размера (size * 0.5f), чтобы центр картинки совпал с центром экрана
        float xMin = (Screen.width * 0.5f) - (size * 0.5f);
        float yMin = (Screen.height * 0.5f) - (size * 0.5f);

        // 5. Команда нарисовать текстуру
        // Rect - это прямоугольник (x, y, ширина, высота)
        GUI.DrawTexture(new Rect(xMin, yMin, size, size), crosshairTexture);
    }
}