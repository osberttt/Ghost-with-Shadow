using System;
using UnityEngine;

public class ShadowCam : MonoBehaviour
{
    public Camera mainCam;
    public Transform shadow;
    public GameObject shadowViewUI;

    void Update()
    {
        Vector3 viewportPos = mainCam.WorldToViewportPoint(shadow.position);

        bool isOnScreen =
            viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1 &&
            viewportPos.z > 0;

        shadowViewUI.SetActive(!isOnScreen);
    }
    
    private void LateUpdate()
    {
        transform.position = new Vector3(shadow.position.x, shadow.position.y, transform.position.z);
    }
}
