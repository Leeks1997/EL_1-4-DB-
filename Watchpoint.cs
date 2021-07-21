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
    
    //main
    private MusicList music; //음악 리스트 스크립트(함수 사용을 위함)
    public static int musicButton = 0; //메인 음악 리스트 pageNumber

    private OnClickLevel level; //메인 난이도 선택 스크립트(함수 사용을 위함)
    
    void Awake()
    {
        music = GameObject.Find("MusicList").GetComponent<MusicList>(); //main
        level = GameObject.Find("Difficulty").GetComponent<OnClickLevel>(); //main
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
        }
    }
}
