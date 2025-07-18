using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAppearance : MonoBehaviour
{
public GameObject playerprefub;     // プレイヤーオブジェクト
    public GameObject shouheki;        // 障壁オブジェクト
    public Renderer botonRenderer;      // ボタンの色を変えるRenderer

    public float triggerDistance = 0.5f;
    public float cooldownTime = 5f;

    private bool isInCooldown = false;
    private Color originalColor;

    void Start()
    {
        // 初期色を保存
        if (botonRenderer != null)
        {
            originalColor = botonRenderer.material.color;
        }

        // 初期状態では壁を非表示にしておく（必要に応じて）
        if (shouheki != null)
        {
            shouheki.SetActive(false);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(playerprefub.transform.position, transform.position);

        if (!isInCooldown && distance <= triggerDistance)
        {
            StartCoroutine(HandleWallToggle());
        }
    }

    IEnumerator HandleWallToggle()
    {
        isInCooldown = true;

        // 壁を表示
        if (shouheki != null)
            shouheki.SetActive(true);

        // ボタンを白に変更
        if (botonRenderer != null)
            botonRenderer.material.color = Color.white;

        // 指定時間待機
        yield return new WaitForSeconds(cooldownTime);

        // 壁を非表示に戻す
        if (shouheki != null)
            shouheki.SetActive(false);

        // ボタンの色を元に戻す
        if (botonRenderer != null)
            botonRenderer.material.color = originalColor;

        isInCooldown = false;
    }
}
