using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // 銃弾のプレハブ
    public float shootSpeed = 20f;  // 投げる力
    public float spawnDistance = 0.5f; // カメラから前方にずらす距離

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 spawnPos = ray.origin + ray.direction * spawnDistance;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = ray.direction.normalized * shootSpeed;
        }
    }
}
