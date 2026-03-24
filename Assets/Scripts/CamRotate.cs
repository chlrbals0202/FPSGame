using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전 속도 변수
    public float rotSpeed = 200f;

    //회전 값 변수
    float mx = 0;
    float my = 0;

    // Update is called once per frame
    void Update()
    {
        //사용자의 입력을 받아 물체 회전
        //1. 마우스 입력을 받는다.
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //1-1. 회전 값 변수에 마우스 입력 값으로 미리 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //1-2. 마우스 상하 이동 회전 변수의 값을 -90~90도로 제한
        my = Mathf.Clamp(my, -90f, 90f);

        //2. 회전 방향으로 물체 회전
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
