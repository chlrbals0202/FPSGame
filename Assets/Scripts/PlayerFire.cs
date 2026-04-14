using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //발사 위치
    public GameObject firePosition;

    //투척 무기 오브젝트
    public GameObject bombFactory;

    //투척 파워
    public float throwPower = 15f;

    //피격 이펙트 오브제그
    public GameObject bulletEffect;

    //피격 이펙트 파티클 시스템
    ParticleSystem ps;

    //발사 무기 공격력
    public int weaponPower = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

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

        //마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사
        //마우스 왼쪽 버튼 입력
        if (Input.GetMouseButtonDown(0))
        {
            //레이를 생성한 후 발사될 위치와 진행 방향을 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            //레이가 부딪힌 대상의 정보를 저장할 변수 생성
            RaycastHit hitInfo = new RaycastHit();

            //레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트 표시
            if(Physics.Raycast(ray, out hitInfo))
            {
                //만일 레이에 부딪힌 대상의 레이어가 'Enemy'라면 데미지 함수 실행
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                //그렇지 않다면, 레이에 부딪힌 지점에 피격 이펙트 플레이
                else
                {
                    //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동
                    bulletEffect.transform.position = hitInfo.point;

                    //피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치
                    bulletEffect.transform.forward = hitInfo.normal;

                    //피격 이펙트 플레이
                    ps.Play();
                }
            }
        }
    }
}
