using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //발사 위치
    public GameObject firePosition;

    //투척 무기 오브젝트
    public GameObject bombFactory;

    //투척 파워
    public float throwPower = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄 투척

        //1.마우스 오른쪽 버튼 입력 받기
        if (Input.GetMouseButtonDown(1))
        {
            //수류탄 오브젝트를 생성한 후 수류탄의 생성 위치를 발사 위치로 함
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            //수류탄 오브젝트의 리지드바디 컴포넌트 가져옴
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //카메라의 정면 방향으로 수류탄에 물리적인 힘을 가함
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

        }
    }
}
