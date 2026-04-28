using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    //제거될 시간 변수
    public float destroyTime = 1.5f;

    //경과 시간 측정용 변수
    float currentTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //만일 경과 시간이 제거될 시간을 초과하면 자기 자신 제거
        if (currentTime > destroyTime)
        {
            
            Destroy(gameObject);
        }
        //경과 시간 누적
        currentTime += Time.deltaTime;

    }
}
