using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;
    private int killCount;


    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }
    private void Update()
    {
        timerText.text = Time.time.ToString("F2") + "s";
    }

    public void EnableGameOverUI()
    {
        Time.timeScale = 0.25f;
        gameOverUI.SetActive(true);

        Player player = FindAnyObjectByType<Player>();
        if (player != null)
            player.EnableMovementAndJump(false);

        Enemy[] enemies = FindObjectsByType<Enemy>();
        foreach (var enemy in enemies)
            enemy.EnableMovementAndJump(false);

    }
    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void AddKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
