using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class time : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private TextMeshProUGUI GoalMesseage;

    [SerializeField] private Transform targetObject; // 監視対象のオブジェクト（プレイヤーなど）

    private float elapsedTime;
    private bool goalReached;

    private float goalX = 420f; // ← ここでx座標のゴールを設定
    private float threshold = 0.1f; // 許容誤差（近さ）

    void Start()
    {
        elapsedTime = 0.0f;
        goalReached = false;
        GoalMesseage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!goalReached)
        {
            elapsedTime += Time.deltaTime;

            float currentX = targetObject.position.x;

            // x座標がゴールに近づいたら達成とみなす
            if (Mathf.Abs(currentX - goalX) < threshold)
            {
                goalReached = true;
                GoalMesseage.gameObject.SetActive(true);
                //Debug.Log("X座標が目標に到達しました！");
            }
        }

        TextTime.text = string.Format("Time {0:f2} sec", elapsedTime);
    }
}


