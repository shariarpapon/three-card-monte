using UnityEngine;

public class InputControlManager : MonoBehaviour
{
    public InputControlManager Instance { get; private set; }

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
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameManager.Instance.StartNextRound();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            GameManager.Instance.ShowCards();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            GameManager.Instance.HideCards();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            GameManager.Instance.ShuffleCards();
    }
}
