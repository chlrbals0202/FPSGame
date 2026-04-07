using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //자기자신의 방향을 카메라의 방향과 일치
        transform.forward = Camera.main.transform.forward;
    }
}
