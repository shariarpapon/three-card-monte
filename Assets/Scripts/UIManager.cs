using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using SimpleFileBrowser;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject cardPanel;

    [Header("User Input UI")]
    [SerializeField] private Slider masterVol;
    [SerializeField] private Slider musicVol;
    [SerializeField] private Slider sfxVol;
    [SerializeField] private TMP_InputField shuffleSpeedInput;
    [SerializeField] private TMP_InputField startShuffleSpeedInput;
    [SerializeField] private Button logoButton;
    [SerializeField] private RawImage logoImage;

    private bool isPickingFile = false;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        #region Assigning UI Events
        masterVol.onValueChanged.AddListener( (float vol) => {
            AudioManager.Instance.SetMasterVolume(vol);
        } ) ;

        musicVol.onValueChanged.AddListener((float vol) => {
            AudioManager.Instance.SetMusicVolume(vol);
        });

        sfxVol.onValueChanged.AddListener((float vol) => {
            AudioManager.Instance.SetSFXVolume(vol);
        });

        shuffleSpeedInput.onValueChanged.AddListener((string s) => {
            if (float.TryParse(s, out float speed)) ShuffleManager.Instance.SetShuffleSpeed(speed); 
        });

        startShuffleSpeedInput.onValueChanged.AddListener((string s) => {

            if (float.TryParse(s, out float startSpeed)) ShuffleManager.Instance.SetStartShuffleSpeed(startSpeed);
        });

        logoButton.onClick.AddListener(ChangeLogoImage);
        #endregion
    }

    private void Start() 
    {
        InitializeUI();
        settingsPanel.SetActive(false);
        buttonPanel.SetActive(false);
        cardPanel.SetActive(true);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (settingsPanel.activeSelf == false)
            {
                if (buttonPanel.activeSelf) HideButtonPanel();
                else ShowButtonPanel();
            }
            else settingsPanel.SetActive(false);
        }
    }

    private void ShowButtonPanel() 
    {
        buttonPanel.SetActive(true);
    }

    public void HideButtonPanel() 
    {        
        buttonPanel.SetActive(false);
    }

    public void OpenSettings() 
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings() 
    {
        if (isPickingFile == true) return;
        settingsPanel.SetActive(false);
    }

    public void ResetButton() 
    {
        GameManager.Instance.ResetGame();
    }

    public void ShuffleButton() 
    {
        GameManager.Instance.ShuffleCards();
    }

    public void ShowButton() 
    {
        GameManager.Instance.ShowCards();
    }

    public void HideButton() 
    {
        GameManager.Instance.HideCards();
    }

    public void QuitButton() 
    {
        Application.Quit();
    }

    private void InitializeUI()
    {
        shuffleSpeedInput.text = ShuffleManager.Instance.GetShuffleSpeed().ToString();
        startShuffleSpeedInput.text = ShuffleManager.Instance.GetStartShuffleSpeed().ToString();
        masterVol.value = AudioManager.Instance.GetMasterVolume();
        musicVol.value = AudioManager.Instance.GetMusicVolume();
        sfxVol.value = AudioManager.Instance.GetSFXVolume();
    }

    private void ChangeLogoImage()
    {
        if (isPickingFile == false)
        {
            isPickingFile = true;
            FileBrowser.ShowLoadDialog(OnImageUploadSuccess, OnImageUploadCanceled, FileBrowser.PickMode.Files);
        }
    }

    public void OnImageUploadSuccess(string[] paths) 
    {
        var content = File.ReadAllBytes(paths[0]);
        Texture2D texture = new Texture2D(115, 115);
        texture.LoadImage(content);
        logoImage.texture = texture;
        isPickingFile = false;
    }

    public void OnImageUploadCanceled() 
    {
        isPickingFile = false;
    }

    public Texture GetLogoImage { get { return logoImage.mainTexture; } }

}
