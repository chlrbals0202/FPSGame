using UnityEngine;

public class UIOverlayAim : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        // 1. 커서를 화면 중앙에 고정 (마우스를 움직여도 커서는 중앙에 멈춰 있음)
        Cursor.lockState = CursorLockMode.Locked;

        rectTransform = GetComponent<RectTransform>();

        // 실제 마우스 커서는 숨기기 (선택 사항)
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스의 스크린 좌표를 UI의 위치로 그대로 전달
        rectTransform.position = Input.mousePosition;
    }
}