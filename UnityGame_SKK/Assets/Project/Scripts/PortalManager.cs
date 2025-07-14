using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private GameObject player;

    private GameObject exitPortal = null;
    private GameObject entrancePortal = null;

    private void OnEnable()
    {
        Bullet.OnBulletHit += HandleBulletHit;
    }

    private void OnDisable()
    {
        Bullet.OnBulletHit -= HandleBulletHit;
    }

    private void Update()
    {
        // 任意のタイミングで出口ポータルを生成（Eキー）
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 playerPosition = player.transform.position;
            if (exitPortal != null)
            {
                Destroy(exitPortal); // 既存の出口ポータルを削除
                Debug.Log("Previous exit portal destroyed.");
            }

            exitPortal = Instantiate(portalPrefab, playerPosition, Quaternion.identity);
            exitPortal.name = "ExitPortal";

            Debug.Log("Exit portal placed at player position: " + playerPosition);
        }
    }

    private void HandleBulletHit(Vector3 hitPosition)
    {
        // 既存の入口ポータルを削除
        if (entrancePortal != null)
        {
            Destroy(entrancePortal);
            Debug.Log("Previous entrance portal destroyed.");
        }

        // 弾が当たった位置に入口ポータルを生成
        entrancePortal = Instantiate(portalPrefab, hitPosition, Quaternion.identity);
        entrancePortal.name = "EntrancePortal";

        if (exitPortal != null)
        {
            // ワープ先が設定されていればワープ可能
            Portal portalScript = entrancePortal.AddComponent<Portal>();
            portalScript.SetWarpTarget(exitPortal.transform.position, player, exitPortal);
            Debug.Log("Entrance portal created at: " + hitPosition + ". Touch to warp to: " + exitPortal.transform.position);
        }
        else
        {
            Debug.LogWarning("No exit portal set yet! Press 'E' to place one at the player's current position.");
        }
    }
}
