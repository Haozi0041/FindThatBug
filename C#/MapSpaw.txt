using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float cameraPositionOffest;
    private PlayerControl playerControl;
    private void Awake()
    {
        playerControl = player.GetComponent<PlayerControl>();
    }
    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + cameraPositionOffest, -9);
    }
}
