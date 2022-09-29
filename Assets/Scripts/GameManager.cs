using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Static Variables
    public static GameManager Instance { get; private set; }
    public static CardState CurrentCardState { get; private set; }
    public static int RoundIndex = 1;
    #endregion

    #region Inspector Variables
    public int roundCardCount = 5;
    [SerializeField] private GameObject round1Cards;
    [SerializeField] private GameObject round2Cards;
    #endregion

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

    private void Start() 
    {
        InitRound(1);
    }

    public void IdleCards() 
    {
        CurrentCardState = CardState.Idle;
    }

    public void ShowCards() 
    {
        if (CurrentCardState == CardState.Shuffling) return;

        GameObject currentCardHolder = RoundIndex == 1 ? round1Cards : round2Cards;

        //Show inside the cards

        Debug.Log("Showing cards");
        SetCardState(CardState.Showing);
    }

    public void HideCards() 
    {
        if (CurrentCardState == CardState.Shuffling) return;

        GameObject currentCardHolder = RoundIndex == 1 ? round1Cards : round2Cards;

        //Hide all the cards

        Debug.Log("Hiding cards");
        SetCardState(CardState.Hidden);
    }

    public void ShuffleCards() 
    {
        if (CurrentCardState == CardState.Shuffling) return;

        Debug.Log("Shuffling cards");
        SetCardState(CardState.Shuffling);

        ShuffleManager.Instance.Shuffle( delegate {
            IdleCards();
        });
    }

    public void SetCardState(CardState state) 
    {
        CurrentCardState = state;
    }

    public void ResetGame() 
    {
        Debug.Log("Reset game");
        ForceStopShuffle();
        InitRound(1);
    }

    public void StartNextRound() 
    {
        InitRound(RoundIndex + 1);
    }

    private void InitRound(int roundIndex) 
    {
        Debug.Log($"Starting round {RoundIndex}");

        if (roundIndex > 2) roundIndex = 1;
        RoundIndex = roundIndex;

        if (RoundIndex == 1)
        {
            round2Cards.SetActive(false);
            round1Cards.SetActive(true);
        }
        else 
        {
            round1Cards.SetActive(false);
            round2Cards.SetActive(true);
        }

        ShowCards();
        RandomizeLogoCard(RoundIndex == 1 ? round1Cards : round2Cards);
    }

    private void RandomizeLogoCard(GameObject cardHolder) 
    {
        Debug.Log("Randomize card");
        return;
    }

    private void ForceStopShuffle() 
    {
        SetCardState(CardState.Idle);
        ShuffleManager.Instance.StopShuffling();
    }
}

[System.Serializable]
public enum CardState 
{
    Showing,
    Hidden,
    Shuffling,
    Idle
}
