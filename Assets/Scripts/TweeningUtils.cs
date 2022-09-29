using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class TweeningUtils
{
    public readonly static HashSet<Transform> CurrentTweenInstances = new HashSet<Transform>();

    public static IEnumerator FlipCard(Transform tf, float speed) 
    {
        if (CurrentTweenInstances.Contains(tf) == false)
        {
            CurrentTweenInstances.Add(tf);
            float t = 0;
            float startAngle = tf.rotation.eulerAngles.y;
            while (t <= 1)
            {
                float angleStep = Mathf.Lerp(0.0f, 180.0f, t);
                tf.rotation = Quaternion.Euler(0, startAngle + angleStep, 0);

                if (angleStep > 90.0f) tf.GetComponent<Image>().color = Random.ColorHSV();
                t += Time.deltaTime * speed;

                yield return null;
            }
            tf.rotation = Quaternion.Euler(0, startAngle + 180, 0);
            CurrentTweenInstances.Remove(tf);
        }
    }
}
