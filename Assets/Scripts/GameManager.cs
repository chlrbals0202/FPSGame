using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    //싱글톤 인스턴스
    public static GameManager gm;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }

    //게임 상태 열거
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    //현재 게임 상태 변수
    public GameState gState;

    //게임 상태 UI 오브젝트 참조
    public GameObject gameLabel;

    //게임 상태 UI 텍스트 컴포넌트 (레거시 Text)
    TextMeshProUGUI gameText;

    //PlayerMove 클래스 참조
    PlayerMove player;

    void Start()
    {
        //초기 게임 상태를 준비 상태로 설정
        gState = GameState.Ready;

        //게임 상태 UI 오브젝트에서 Text 컴포넌트 가져오기
        gameText = gameLabel.GetComponent<TextMeshProUGUI>();

        //게임 텍스트를 준비 문구로 설정
        gameText.text = "준비...";

        //게임 텍스트의 글자 색상을 노란색으로 설정
        gameText.color = new Color32(255, 185, 0, 255);

        //준비 상태 --> 게임 중 상태로 전환 시작
        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "시작!";
        yield return new WaitForSeconds(1f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
    }

    void Update()
    {
        if(player.hp <= 0)
        {
            gameLabel.SetActive(true);
            gameText.text = "게임 오버";
            gameText.color = new Color32(255, 0, 0, 255);
            gState = GameState.GameOver;
        }
    }
}
