using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;

    [SerializeField] private TextMeshProUGUI timeLeft;
    [SerializeField] private TextMeshProUGUI rollsCounter;

    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private Player player;

    private int winStatus = 0;
    private int rollCount = 0;
    private float remainingTime = 30f;

    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI TimeLeft { get => timeLeft; set => timeLeft = value; }
    public TextMeshProUGUI RollsCounter { get => rollsCounter; set => rollsCounter = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        inputManager.OnJump += Play;
        inputManager.OnRestart += Restart;
    }

    private void Update()
    {
        if (winStatus == 1)
        {
            if (!winMenu.activeSelf)
            {
                AudioManager.Instance.PlaySFX(1, 1f);
                AudioManager.Instance.musicSource.Stop();
            }
            
            player.canMove = false;
            winMenu.SetActive(true);
        }
        else if (winStatus == 2)
        {
            if (!loseMenu.activeSelf)
            {
                AudioManager.Instance.PlaySFX(2, 1f);
                AudioManager.Instance.musicSource.Stop();
            }

            player.canMove = false;
            loseMenu.SetActive(true);
        }

        if (remainingTime > 0 && !startMenu.activeSelf && winStatus != 1)
            remainingTime -= Time.deltaTime;
        else if(!startMenu.activeSelf && winStatus != 1)
        {
            remainingTime = 0;
            winStatus = 2;
        }

        if (rollCount == 40)
            winStatus = 1;

        UpdateUI();
    }

    private void Play()
    {
        if (winStatus == 0)
        {
            startMenu.SetActive(false);
            AudioManager.Instance.PlayMusic(0);
            player.canMove = true;
        }
        else if (winStatus == 1 || winStatus == 2)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateUI()
    {
        timeLeft.text = FormatTime(remainingTime);
        rollsCounter.text = rollCount.ToString() + "/40";
    }

    public static string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    public void AddRoll()
    {
        rollCount++;
    }

    private void OnDisable()
    {
        inputManager.OnJump -= Play;
    }

}
