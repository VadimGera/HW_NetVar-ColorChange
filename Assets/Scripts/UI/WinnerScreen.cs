using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button restart;

    private void Awake()
    {
        restart.onClick.AddListener(() => SceneManager.LoadScene(0));
        gameObject.SetActive(false);
    }

    public void Show(string winnerName)
    {
        gameObject.SetActive(true);
        text.SetText($"Winner: {winnerName}");
    }
}
