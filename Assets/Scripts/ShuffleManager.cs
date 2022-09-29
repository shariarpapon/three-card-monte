using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShuffleManager : MonoBehaviour
{
    public static ShuffleManager Instance { get; private set; }

    [SerializeField] private float shuffleHeight = 200;
    [SerializeField] private float shuffleSpeed = 1;
    [SerializeField] private float initSpeed = 0.25f;
    [SerializeField] private int shuffleCount = 50;
    [SerializeField] private AnimationCurve shuffleSpeedCurve;
    [SerializeField] private AnimationCurve deltaPosCurve;

    private List<int> indexesRound1 = new() { 0, 1, 2 };
    private List<int> indexesRound2 = new() { 0, 1, 2, 3, 4 };

    public Transform[] cards_Round1;
    public Transform[] cards_Round2;
    private Vector3[] card_1_pos;
    private Vector3[] card_2_pos;
    private Coroutine shuffleRoutine = null;

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

        card_1_pos = new Vector3[3];
        card_2_pos = new Vector3[5];
        for (int i = 0; i < 5; i++) 
        {
            card_2_pos[i] = cards_Round2[i].localPosition;
            if (i > 2) continue;
            card_1_pos[i] = cards_Round1[i].localPosition;
        }
    }

    public void Shuffle(System.Action onShuffleEnd) 
    {
        shuffleRoutine = StartCoroutine(ShuffleRoutine(GameManager.RoundIndex == 1 ? cards_Round1 : cards_Round2, onShuffleEnd));
    }

    private IEnumerator ShuffleRoutine(Transform[] cards, System.Action onShuffleEnd) 
    {
        Debug.Log("<color=cyan>Shuffle started...</color>");
        List<int> indexList = (GameManager.RoundIndex == 1) ? indexesRound1 : indexesRound2;

        for (int i = 0; i < shuffleCount; i++) 
        {
            if (GameManager.CurrentCardState != CardState.Shuffling) break;

            #region Animate Switching Place
            float dt = i / (float)shuffleCount;
            int[] ids = GetUniqueRandomPair(ref indexList);
            Transform t0 = cards[ids[0]];
            Transform t1 = cards[ids[1]];
            Vector2 pos0 = t0.localPosition;
            Vector2 pos1 = t1.localPosition;
            float time = 0;
            while (time <= 1) 
            {
                if (GameManager.CurrentCardState != CardState.Shuffling) break;

                float x0 = Mathf.Lerp(pos0.x, pos1.x, time);
                float y0 = pos0.y + (deltaPosCurve.Evaluate(time) * shuffleHeight);

                float x1 = Mathf.Lerp(pos1.x, pos0.x, time);
                float y1 = pos1.y + (deltaPosCurve.Evaluate(time) * shuffleHeight * -1.0f);

                t0.localPosition = new Vector3(x0, y0, 0);
                t1.localPosition = new Vector3(x1, y1, 0);

                time += Time.deltaTime * shuffleSpeedCurve.Evaluate(dt) * shuffleSpeed + initSpeed;
                yield return null;
                
            t0.localPosition = pos1;
            t1.localPosition = pos0;
            #endregion
            }
        }

        Debug.Log("<color=green>Shuffle finished...</color>");

        if (GameManager.RoundIndex == 1) indexesRound1 = new() { 0, 1, 2 };
        else indexesRound2 = new() { 0, 1, 2, 3, 4 };

        onShuffleEnd?.Invoke();
    }

    private int[] GetUniqueRandomPair(ref List<int> list) 
    {
        int[] pair = new int[2];
        int _i = Random.Range(0, list.Count);
        pair[0] = list[_i];
        list.Remove(_i);

        int _i2 = Random.Range(0, list.Count);
        pair[1] = list[_i2];

        if (GameManager.RoundIndex == 1) list = new() { 0, 1, 2 };
        else list = new() { 0, 1, 2, 3 , 4};

        return pair;
    }

    public void StopShuffling() 
    {
        if (shuffleRoutine != null) 
        {
            if (GameManager.RoundIndex == 1) indexesRound1 = new() { 0, 1, 2 };
            else indexesRound2 = new() { 0, 1, 2, 3, 4 };

            StopCoroutine(shuffleRoutine); 
        }

        for (int i = 0; i < 5; i++) 
        {
            cards_Round2[i].localPosition = card_2_pos[i];
            if (i > 2) continue;
            cards_Round1[i].localPosition = card_1_pos[i];
        }
    }

    public void SetShuffleSpeed(float value) 
    {
        shuffleSpeed = value;
    }
    public float GetShuffleSpeed() => shuffleSpeed;

    public void SetStartShuffleSpeed(float value) 
    {
        initSpeed = value;
    }
    public float GetStartShuffleSpeed() => initSpeed;
}
