﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public Text rank1;
    public Text name1;
    public Text score1;
    public Text rank2;
    public Text name2;
    public Text score2;
    public Text rank3;
    public Text name3;
    public Text score3;
    public Text rank4;
    public Text name4;
    public Text score4;
    public Text rank5;
    public Text name5;
    public Text score5;
    public Text rank6;
    public Text name6;
    public Text score6;
    public Text rank7;
    public Text name7;
    public Text score7;
    public Text rank8;
    public Text name8;
    public Text score8;
    public Text rank9;
    public Text name9;
    public Text score9;
    public Text rank10;
    public Text name10;
    public Text score10;

    int selectNum = 0;
    public static string[] player;

    void Update()
    {
        if (i >= player.Length - 1) //마지막 배열 값 ""이므로 제외, 배열 크기를 초과할 경우 0으로 리셋
        {
            i = 0;
        }
        else if (i < 0) //index가 0이하일 경우 마지막 첫번째 값으로 설정
        {
            i = (player.Length / 10) * 10;
        }

        Text b = GameObject.Find("rankCount").GetComponent<Text>(); //랭킹 페이지 버튼 값 설정
        b.text = "" + i + 1;

        for (int a = 0; a < 10; a++)
        {
            if (i * 10 + a > player.Length - 1 || player[i * 10 + a] == "") //배열 크기 확인 및 값의 유무(존재 X)
            {
                //값 초기화
                Text t = GameObject.Find("rank" + a).GetComponent<Text>();
                Text n = GameObject.Find("name" + a).GetComponent<Text>();
                Text s = GameObject.Find("score" + a).GetComponent<Text>();

                t.text = "";
                n.text = "";
                s.text = "";
            }
            else
            {
                //값 지정
                Text t = GameObject.Find("rank" + a).GetComponent<Text>();
                Text n = GameObject.Find("name" + a).GetComponent<Text>();
                Text s = GameObject.Find("score" + a).GetComponent<Text>();

                t.text = "" + i * 10 + a;
                n.text = GetDataValue(player[i * 10 + a], "username:");
                s.text = GetDataValue(player[i * 10 + a], "score:");
            }
        }

        return i;
    }

    public int SetPlayer(int i)
    {
        if (i >= player.Length - 1) //마지막 배열 값 ""이므로 제외, 배열 크기를 초과할 경우 0으로 리셋
        {
            i = 0;
        }
        else if (i < 0) //index가 0이하일 경우 마지막 첫번째 값으로 설정
        {
            i = (player.Length / 10) * 10;
        }

        while (true)
        {
            if(player[i] == "")
            {
                break;
            }
        }
        return i;
    }

    IEnumerator GetRanking(int num) //랭킹(player)의 DB값 받아오기
    {
        WWWForm form = new WWWForm();
        form.AddField("cmd", "all");
        form.AddField("numPost", num);

        WWW playerData = new WWW("http://122.32.165.55/musicPlayer_coex_D1.php", form);
        yield return playerData;
        string playerDataString = playerData.text;
        Debug.Log(playerDataString); //받아온 값 확인
        player = playerDataString.Split(';');
        //Debug.Log(player.Length); //받아온 값 확인
        
        SetPlayer(0); //초기 출력 설정
    }

    string GetDataValue(string data, string index1) //각 음악의 세부 정보 분리
    {
        if (data.Equals(""))
        {
            return "";
        }
        else
        {
            string value = data.Substring(data.IndexOf(index1) + index1.Length);
            if (!(index1 == "score:")) //현 음악의 마지막 데이터
            {
                value = value.Remove(value.IndexOf("|")); //구분자 이후 제거()
            }
            return value;
        }
    }
}
