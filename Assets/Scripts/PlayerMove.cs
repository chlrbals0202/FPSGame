using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //이동 속도 변수
    public float moveSpeed = 7f;

    //캐릭터 컨트롤러 변수
    CharacterController cc;

    //중력 변수
    float gravity = -20f;

    //수직 속력 변수
    float yVelocity = 0;

    //점프력 변수
    public float jumpPower = 10f;

    //점프 상태 변수
    public bool isJumping = false;


    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //WASD 키를 누르고 입력하면 캐릭터를 그 방향으로 이동
        //spacebar 키를 누르면 캐릭터가 수직으로 점프

        //1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. 이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1. 메인 카메라를 기준으로 방향 변환
        dir = Camera.main.transform.TransformDirection(dir);

        //2-2. 만일 바닥에 다시 착지했다면
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        //2-2. 만일 spacebar 키를 눌렀다면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            //캐릭터 수직 속도에 점프력을 적용
            yVelocity = jumpPower;
            isJumping = true;
        }

        //2-3. 캐릭터 수직 속도에 중력 값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //이동 속도에 맞춰 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
