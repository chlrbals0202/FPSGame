using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //목표가 될 트랜스폼 컴포넌트
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //카메라의 위치를 목표 트랜스폼의 위치에 일치
        transform.position = target.position;
        
    }
}
