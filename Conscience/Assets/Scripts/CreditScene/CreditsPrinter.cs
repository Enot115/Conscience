using UnityEngine;
using UnityEngine.UI;
using TMPro; // если используете TextMeshPro
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsPrinter : MonoBehaviour
{
    public TextMeshProUGUI creditsText; // компонент текста (перетащите в инспекторе)
    public float printSpeed = 0.05f;    // скорость печати (секунд на символ)
    public string mainMenuSceneName = "Main Menu"; // имя сцены главного меню
    public float delayBeforePrint = 2f; // задержка перед началом печати (в секундах)

    private string fullCreditsText;     // полный текст титров
    private int currentIndex = 0;

    void Start()
    {
        if (creditsText == null)
            creditsText = GetComponent<TextMeshProUGUI>();

        // Здесь напишите ваш текст титров
        fullCreditsText = "ИГРА ЗАВЕРШЕНА\n\n" +
                          "Вы смогли выбраться из своего сознания.\n" +
                          "Когда вы очнулись, вы лежали на кровати в своем доме.\n" +
                          "В нем царила тишина и покой.\n\n" +
                          "Но вдруг кто-то очень сильно начал стучать в окно.\n" +
                          "Вы сначала испугались, но когда подошли к окну замерли\n\n" +
                          "Вы посмотрели на человека и поняли что это ....\n\n" +
                          "";

        creditsText.text = ""; // очищаем
        currentIndex = 0;

        // Скрываем курсор в титрах (если нужно)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Запускаем корутину с задержкой
        StartCoroutine(StartWithDelay());
    }

    IEnumerator StartWithDelay()
    {
        // Ждём указанное количество секунд перед началом печати
        yield return new WaitForSeconds(delayBeforePrint);

        // Начинаем печать текста
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        while (currentIndex < fullCreditsText.Length)
        {
            // Добавляем один символ
            creditsText.text += fullCreditsText[currentIndex];
            currentIndex++;
            yield return new WaitForSeconds(printSpeed);
        }

        // Когда весь текст напечатан — ждём 3 секунды и переходим в меню
        yield return new WaitForSeconds(3f);

        // Показываем курсор перед загрузкой меню
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}