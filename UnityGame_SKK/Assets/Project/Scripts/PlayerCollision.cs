using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public GameObject playerprefub;     // プレイヤーオブジェクト
    public GameObject shouheki6;        // 障壁オブジェクト
    public Renderer botonRenderer;      // ボタンの色を変えるRenderer

    public float triggerDistance = 0.5f;
    public float cooldownTime = 5f;

    private bool isInCooldown = false;
    private Color originalColor;

    void Start()
    {
        // 初期色を保存（ボタンのマテリアルの色）
        if (botonRenderer != null)
        {
            originalColor = botonRenderer.material.color;
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

        // 壁を非表示
        if (shouheki6 != null)
            shouheki6.SetActive(false);

        // ボタンを白に変更
        if (botonRenderer != null)
            botonRenderer.material.color = Color.white;

        // 指定時間待機
        yield return new WaitForSeconds(cooldownTime);

        // 壁を再表示
        if (shouheki6 != null)
            shouheki6.SetActive(true);

        // ボタンの色を元に戻す
        if (botonRenderer != null)
            botonRenderer.material.color = originalColor;

        isInCooldown = false;
    }

}
