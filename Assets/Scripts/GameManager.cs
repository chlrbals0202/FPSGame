using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GameManager : MonoBehaviour
{
    //싱글턴 변수
    public static GameManager gm;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }

    //게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    // 현재 게임 상태 변수
    public GameState gState;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // PlayerMove 클래스 변수
    PlayerMove player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 초기 게임 상태는 준비 상태로 설정
        gState = GameState.Ready;

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져옴
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 내용을 아래와 같이 한다
        gameText.text = "Ready...";

        // 상태 텍스트의 색상을 주황색으로 설정
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 --> 게임 중 상태로 전환
        StartCoroutine(ReadyToStart());

        //
        player = GameObject.Find("Player").GetComponent<PlayerMove>(); 
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        //
        if(player.hp <= 0)
        {
            gameLabel.SetActive(true);
            gameText.text = "Game Over";
            gameText.color = new Color32(255, 0, 0, 255);
            gState = GameState.GameOver;
        }
    }
}
