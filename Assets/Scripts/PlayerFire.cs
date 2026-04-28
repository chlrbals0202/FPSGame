using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //발사 위치
    public GameObject firePosition;

    //폭탄 프리팹 오브젝트
    public GameObject bombFactory;

    //폭탄 파워
    public float throwPower = 15f;

    //폭탄 생성 위치를 플레이어 앞으로 얼마나 띄울지 (인스펙터에서 조절 가능)
    public float throwOffset = 1.5f;

    //총알 이펙트 오브젝트
    public GameObject bulletEffect;

    //총알 이펙트 파티클 시스템
    ParticleSystem ps;

    //총알 공격 공격력
    public int weaponPower = 5;

    //레이저 공격 공격력
    public int laserPower = 10;

    //레이저 피격 이펙트 오브젝트
    public GameObject laserEffect;

    //레이저 빔 색상 (인스펙터에서 바꿀 수 있음)
    public Color laserColor = new Color(1f, 0.2f, 0.2f);

    //레이저 빔 굵기 (인스펙터에서 바꿀 수 있음)
    public float laserWidth = 0.05f;

    //레이저 빔이 화면에 보이는 시간 (초 단위, 인스펙터에서 바꿀 수 있음)
    public float laserDuration = 0.1f;

    //레이저 빔 선을 그리는 컴포넌트
    LineRenderer laserLine;

    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        //이 오브젝트에 LineRenderer 컴포넌트를 코드로 추가
        laserLine = gameObject.AddComponent<LineRenderer>();

        //빔은 시작점(총구)과 끝점(피격 위치), 2개의 점으로 이루어짐
        laserLine.positionCount = 2;

        //빔의 시작 굵기와 끝 굵기 설정 (끝으로 갈수록 가늘어짐)
        laserLine.startWidth = laserWidth;
        laserLine.endWidth = laserWidth * 0.3f;

        //빔 머티리얼 생성 후 HideAndDontSave로 설정해 도메인 리로드 시 파괴 방지
        Material laserMat = new Material(Shader.Find("Sprites/Default"));
        laserMat.hideFlags = HideFlags.HideAndDontSave;
        laserLine.material = laserMat;

        //빔의 시작 색상 설정
        laserLine.startColor = laserColor;

        //빔의 끝 색상은 같은 색이지만 투명하게 설정 (끝으로 갈수록 사라지는 효과)
        laserLine.endColor = new Color(laserColor.r, laserColor.g, laserColor.b, 0f);

        //빔의 위치를 월드 좌표 기준으로 설정 (오브젝트 이동에 영향받지 않음)
        laserLine.useWorldSpace = true;

        //처음에는 빔을 숨겨둠
        laserLine.enabled = false;
    }

    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            //카메라 앞 방향으로 throwOffset만큼 띄운 위치에 생성
            bomb.transform.position = firePosition.transform.position + Camera.main.transform.forward * throwOffset;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                    DamageCount.instance.AddDamage(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;
                    bulletEffect.transform.forward = hitInfo.normal;
                    ps.Play();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("레이저 발사!");

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            //빔의 시작점은 총구(발사 위치)
            Vector3 startPoint = firePosition.transform.position;
            Vector3 endPoint;

            if (Physics.Raycast(ray, out hitInfo))
            {
                //레이가 뭔가에 맞으면 끝점을 피격 위치로 설정
                endPoint = hitInfo.point;

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(laserPower);
                    DamageCount.instance.AddDamage(laserPower);
                }

                //피격 위치에 이펙트 프리팹을 생성하고, 2초 후 자동 삭제
                GameObject effect = Instantiate(laserEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(effect, 2f);
            }
            else
            {
                //아무것도 안 맞으면 끝점을 카메라 정면 100m 앞으로 설정
                endPoint = Camera.main.transform.position + Camera.main.transform.forward * 100f;
            }

            //코루틴으로 빔을 잠깐 켰다가 끔
            StartCoroutine(ShowLaserBeam(startPoint, endPoint));
        }
    }

    void OnDestroy()
    {
        //런타임에 생성한 머티리얼을 직접 해제해 메모리 누수 방지
        if (laserLine != null && laserLine.material != null)
            Destroy(laserLine.material);
    }

    IEnumerator ShowLaserBeam(Vector3 start, Vector3 end)
    {
        //빔의 시작점과 끝점 위치를 설정
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);

        //빔을 화면에 표시
        laserLine.enabled = true;

        //laserDuration초 동안 기다림
        yield return new WaitForSeconds(laserDuration);

        //빔을 다시 숨김
        laserLine.enabled = false;
    }
}
