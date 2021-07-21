using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    //ListMusic->item1->text
    public Text musicTitleOne;
    public Text musicComposerOne;
    public Text genreOne;
    //ListMusic->item2->text
    public Text musicTitleTwo;
    public Text musicComposerTwo;
    public Text genreTwo;
    //ListMusic->item3->text
    public Text musicTitleThree;
    public Text musicComposerThree;
    public Text genreThree;
    //SelectedMusic->text
    public Text runTime;
    public Text bpm;
    public Text selectedMusic;

    int musicButton = 0; //Watchpoint로 입력받은 기존 값
    public static string selectedMusicValue = null;

    string[] music;

    void Start()
    {
        StartCoroutine(GetMusicItem());
    }

    void Update()
    {
        if(musicButton != Watchpoint.musicButton) //onClickButton
        {
            musicButton = Watchpoint.musicButton;
            SetMusicItem(musicButton); //출력 값 변경
        }
    }

    IEnumerator GetMusicItem() //음악 리스트(ListMusic)의 DB값 받아오기
    {
        WWWForm form = new WWWForm();

        WWW musicData = new WWW("http://122.32.165.55/musicList_coex_D1.php", form);
        yield return musicData;
        string musicDataString = musicData.text;
        print(musicDataString); //받아온 값 확인
        music = musicDataString.Split(';'); //세미콜론을 이용하여 각 음악을 분리하여 저장

        SetMusicItem(musicButton); //초기 출력 설정
    }

    public int SetMusicItem(int i) //메인 메뉴에 음악 리스트(ListMusic) 출력
    {
        if (i >= music.Length - 1) //마지막 배열 값 ""이므로 제외, 배열 크기를 초과할 경우 0으로 리셋
        {
            i = 0;
        }
        else if (i < 0) //index가 0이하일 경우 마지막 첫번째 값으로 설정
        {
            i = (music.Length / 3) * 3;
        }

        musicTitleOne.text = GetDataValue(music[i], "title:");
        musicComposerOne.text = GetDataValue(music[i], "composer:");
        genreOne.text = GetDataValue(music[i], "genre:");

        //초기 선택된 음악(SelectedMusic) 출력 값 설정(첫 번째 아이템)
        runTime.text = GetDataValue(music[i], "runtime:");
        bpm.text = GetDataValue(music[i], "bpm:");
        selectedMusic.text = GetDataValue(music[i], "title:");
        //PLAY에 전달할 초기 title(곡 명) 값 지정
        selectedMusicValue = GetDataValue(music[i], "title:"); 

        if (i + 1 <= music.Length - 1) //2번째 item 값 존재
        {
            musicTitleTwo.text = GetDataValue(music[i + 1], "title:");
            musicComposerTwo.text = GetDataValue(music[i + 1], "composer:");
            genreTwo.text = GetDataValue(music[i + 1], "genre:");
            if (i + 2 <= music.Length - 1) //3번째 item 값 존재
            {
                musicTitleThree.text = GetDataValue(music[i + 2], "title:");
                musicComposerThree.text = GetDataValue(music[i + 2], "composer:");
                genreThree.text = GetDataValue(music[i + 2], "genre:");
            }
            else
            {
                //초기화
                musicTitleThree.text = "";
                musicComposerThree.text = "";
                genreThree.text = "";
            }
        }
        else
        {
            //초기화
            musicTitleTwo.text = GetDataValue(music[i + 1], "");
            musicComposerTwo.text = GetDataValue(music[i + 1], "");
            genreTwo.text = GetDataValue(music[i + 1], "");
            musicTitleThree.text = "";
            musicComposerThree.text = "";
            genreThree.text = "";
        }

        return i; //변경된 값 반환
    }

    //음악 리스트(ListMusic)의 아이템을 선택했을 경우, 선택된 음악(SelectedMusic)의 출력 값 변경
    public void OnClickMusicItem(int i)
    {
        runTime.text = GetDataValue(music[musicButton + i], "runtime:");
        bpm.text = GetDataValue(music[musicButton + i], "bpm:");
        selectedMusic.text = GetDataValue(music[musicButton + i], "title:");

        selectedMusicValue = GetDataValue(music[musicButton + i], "title:");
    }

    string GetDataValue(string data, string index1) //각 음악의 세부 정보 분리(곡 이름, 작곡가 등)
    {
        if (data.Equals(""))
        {
            return "";
        }else
        {
            string value = data.Substring(data.IndexOf(index1) + index1.Length);
            if (!(index1 == "bpm:")) //현 음악의 마지막 데이터
            {
                value = value.Remove(value.IndexOf("|")); //구분자 이후 제거()
            }
            return value;
        }
    }

    /*
    //사용자 스코어는 다른 테이블에 ...! 이건 통째로 옮겨야할수도...!
    IEnumerator GetPlayerScore(string player, string musicNum) //음악에 대한 사용자 스코어
    {
        WWWForm form = new WWWForm();
        form.AddField("idPost", player);
        form.AddField("musicPost", int.Parse(musicNum));

        WWW ScoreData = new WWW("http://122.32.165.55/selectedMusicScore_coex.php", form); //변경필요! => 아직 못정함!
        yield return ScoreData;

        int PlayerScoreData = int.Parse(ScoreData.text);
        playerScore.text = "my score: "+PlayerScoreData;
        musicPercent.text = ""+PlayerScoreData+ "%"; //나중에 (실제 노래 플레이 타임/풀타임)으로 변경

        if (PlayerScoreData == 100) {
            playerScore_text.text = "S";
        } else if(PlayerScoreData > 80){
            playerScore_text.text = "A";
        } else if (PlayerScoreData > 60){
            playerScore_text.text = "B";
        } else if (PlayerScoreData > 40){
            playerScore_text.text = "C";
        } else if (PlayerScoreData > 20){
            playerScore_text.text = "D";
        } else if (PlayerScoreData > 0){
            playerScore_text.text = "E";
        } else {
            playerScore_text.text = "F";
        }
    }
    */
}
