// 여기를 건드려야 하는건가? UI 시작화면..?
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class Watchpoint : MonoBehaviour
{
    public SerialController serialController;

    private Transform tr;
    private Ray ray;
    private RaycastHit hit;

    public float dist = 10.0f;

    private GameObject preGaze; //이전에 응시중이었던것
    private GameObject curGaze; //현재 응시중인것
    
    private KeyBoardCreate keyboard; //불러올 함수가 있는 스크립트
    int[] stack = new int[300];
    int stack_point = 0;

    //main
    private MusicList music;
    public static int musicButton = 0;

    private Ranking rank;
    public static int rankButton = 0;

    private OnClickLevel level;
    
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "InputName")
        {
            keyboard = GameObject.Find("Keyboard").GetComponent<KeyBoardCreate>();
        }
        else if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            music = GameObject.Find("MusicList").GetComponent<MusicList>();
            rank = GameObject.Find("RankInfo").GetComponent<Ranking>();
            level = GameObject.Find("Difficulty").GetComponent<OnClickLevel>();
        }
    }
    void Start()
    {
        tr = GetComponent<Transform>();//메인 카메라의 Transform컴포넌트를 추출
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }
    
    void Update()
    {
        string message = serialController.ReadSerialMessage(); //아두이노

        //광선생성
        ray = new Ray(tr.position, tr.forward * dist);

        //광선을 씬뷰에 시각적으로 표시
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.green);

        //충돌된 hit 검출하는 raycast
        if(Physics.Raycast(ray,out hit, dist))
        {
            GameObject target = hit.collider.gameObject;
            Debug.Log("충돌함:" + target.name);
            CrossHair.isGaze = true;
            if (message == "R")
            {
                Search(target);
            }
        }
        else
        {
            CrossHair.isGaze = false;
        }

        //오브젝트의 응시여부
        CheckGaze();//????왜 필요한지 이해 못하겠다. 
    }
    void CheckGaze()
    {
        //포인터 이벤트 정보 추출
        PointerEventData data = new PointerEventData(EventSystem.current);

        //검출
        if(Physics.Raycast(ray,out hit, dist))
        {
            curGaze = hit.collider.gameObject;

            //이전 응시와 현재 응시 오브젝트가 다른겅우
            if (curGaze != preGaze)
            {
                //현재 오브젝트에 이벤트 전달
                ExecuteEvents.Execute(curGaze, data, ExecuteEvents.pointerEnterHandler);
                //이전 버튼에 PinterExit이벤트 전달
                ExecuteEvents.Execute(preGaze, data, ExecuteEvents.pointerExitHandler);
                //이전 오브젝트 정보 갱신
                preGaze = curGaze;
            }
        }
        else
        {
            //기존 오브젝트에서 벗어나 다른 오브젝트에 PointExit이벤트를 전달한다.
            if (preGaze != null)
            {
                ExecuteEvents.Execute(preGaze, data, ExecuteEvents.pointerExitHandler);
                preGaze = null;
            }
        }
    }
    void Search(GameObject target) # 여기서 난이도 및 노래 선택하면 랭킹에 띄우는 쿼리 필요!
    {
        switch (target.name)
        {
            case "PlayButton":
                //게임 리스트(레벨선택->게임선택->그래야 게임시작가능)에서 선택할 수 있도록 해야함.
                Debug.Log("GO!");
                SceneManager.LoadScene("Scenes/Map/"+SelectPlay.musicNum);
                break;
            case "easy":
                //레벨(EASY)
                level.OnClickEasy();
                break;
            case "hard":
                //레벨(HARD)
                level.OnClickHard();
                break;
            case "scrollUp":
                //MusicList의 출력 아이템 변경(앞->뒤)
                musicButton += 3;
                musicButton = music.SetMusicItem(musicButton);
                break;
            case "scrollDown":
                //MusicList의 출력 아이템 변경(뒤->앞)
                musicButton -= 3;
                musicButton = music.SetMusicItem(musicButton);
                break;
            case "item1":
                //첫 번째 노래 선택
                music.OnClickMusicItem(0);
                break;
            case "item2":
                //두 번째 노래 선택
                music.OnClickMusicItem(1);
                break;
            case "item3":
                //세 번째 노래 선택
                music.OnClickMusicItem(2);
                break;
            case "rankScrollUp":
                //MusicList의 출력 아이템 변경(앞->뒤)
                rankButton += 1;
                rankButton = rank.SetPlayer(rankButton);
                break;
            case "rankScrollDown":
                //MusicList의 출력 아이템 변경(뒤->앞)
                rankButton -= 1;
                rankButton = rank.SetPlayer(rankButton);
                break;
            case "ChangeNicknameBtn":
                break;
            case "soundUp":
                break;
            case "soundDown":
                break;
            case "vibrationDown":
                break;
            case "vibrationUp":
                break;
            case "InputButton":
                keyboard.create();
                stack = new int[300];
                stack_point = 0;
                break;
            case "ButtonEnter":
                stack[stack_point] = 10;//끝남을 의미
                stack_point++;
                keyboard.done(stack);
                break;
            case "ButtonCancel":
                /*[완료]
                 * 0은 2이상일때만 하나의 문자로 취급해야한다.
                *중복된 숫자는 하나로 취급해야한다.
                */
                bool re = true;
                while (re)
                {
                    if (stack[stack_point] == stack[stack_point - 1])
                    {
                        if (stack[stack_point] == stack[stack_point - 2])
                        {
                            //세번중복
                            stack_point -= 3;
                            stack[stack_point] = 10;//커서가 있는곳
                        }
                        else
                        {
                            //두번중복
                            stack_point -= 2;
                            stack[stack_point] = 10;//커서가 있는곳
                        }
                    }
                    else
                    {
                        if (stack[stack_point] != 0)
                        {
                            //띄어쓰기 1단계-문자로 취급해서는 안된다. 그 전에 있는 것을 다시 실행
                            re = false;
                        }
                        //한번중복
                        stack_point--;
                        stack[stack_point] = 10;//커서가 있는곳
                    }
                }
                
                break;
            case "Button (0)":
                stack[stack_point] = 0;
                stack_point++;
                break;
            case "Button (1)":
                stack[stack_point] = 1;
                stack_point++;
                break;
            case "Button (2)":
                stack[stack_point] = 2;
                stack_point++;
                break;
            case "Button (3)":
                stack[stack_point] = 3;
                stack_point++;
                break;
            case "Button (4)":
                stack[stack_point] = 4;
                stack_point++;
                break;
            case "Button (5)":
                stack[stack_point] = 5;
                stack_point++;
                break;
            case "Button (6)":
                stack[stack_point] = 6;
                stack_point++;
                break;
            case "Button (7)":
                stack[stack_point] = 7;
                stack_point++;
                break;
            case "Button (8)":
                stack[stack_point] = 8;
                stack_point++;
                break;
            case "Button (9)":
                stack[stack_point] = 9;
                stack_point++;
                break;
            case "Register":
                //DB에 사용자 정보 저장하기
                ClearInfo.insert = true; //DB 입력 후, 씬 이동
                break;
            case "None":
                //메인화면으로 돌아가기
                SceneManager.LoadScene("Scenes/MainMenu");
                break;
        }
    }
}
