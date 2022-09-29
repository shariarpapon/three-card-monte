using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardRandomizerEditorWindow : EditorWindow 
{
    private Transform cardHolder;

    [MenuItem("Window/Custom/Card Randomizer")]
    private static void OpenWindow() 
    {
        GetWindow<CardRandomizerEditorWindow>("Card Randomizer");
    }

    private void OnGUI() 
    {
        cardHolder = EditorGUILayout.ObjectField(cardHolder, typeof(Transform), true) as Transform;
        if (cardHolder == null || cardHolder.childCount <= 0) return;

        if (GUILayout.Button("Randomize Card Color"))
            foreach (Transform t in cardHolder)
                if (t.TryGetComponent(out Image img))
                    img.color = Random.ColorHSV();
    }
}