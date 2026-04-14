using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200;

    float mx = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float mouse_X = Input.GetAxis("Mouse X");
       

        //1-1. 회전 값 변수에 마우스 입력 값으로 미리 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;


        //2. 회전 방향으로 물체 회전
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
