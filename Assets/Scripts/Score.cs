using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject scoreBoard;
    private int[] highscore;

    int highscoreCounter = 11;

    // Use this for initialization
    void Start () {
        highscore = new int[11];
        scoreBoard.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetScore(int destroyedVillages, int hits)
    {
        float score = destroyedVillages*20 / (hits+1);
        SetScore((int) score);

        Debug.Log(
            "Highscore_0 : " + PlayerPrefs.GetInt("highscore_0") + "\n" +
            "Highscore_1 : " + PlayerPrefs.GetInt("highscore_1") + "\n" +
            "Highscore_2 : " + PlayerPrefs.GetInt("highscore_2") + "\n" +
            "Highscore_3 : " + PlayerPrefs.GetInt("highscore_3") + "\n" +
            "Highscore_4 : " + PlayerPrefs.GetInt("highscore_4") + "\n" +
            "Highscore_5 : " + PlayerPrefs.GetInt("highscore_5") + "\n" +
            "Highscore_6 : " + PlayerPrefs.GetInt("highscore_6") + "\n" +
            "Highscore_7 : " + PlayerPrefs.GetInt("highscore_7") + "\n" +
            "Highscore_8 : " + PlayerPrefs.GetInt("highscore_8") + "\n" +
            "Highscore_9 : " + PlayerPrefs.GetInt("highscore_9") + "\n");


        return (int) score;
    }

    public void SetScore(int score)
    {
        highscore[0] = PlayerPrefs.GetInt("highscore_0");
        highscore[1] = PlayerPrefs.GetInt("highscore_1");
        highscore[2] = PlayerPrefs.GetInt("highscore_2");
        highscore[3] = PlayerPrefs.GetInt("highscore_3");
        highscore[4] = PlayerPrefs.GetInt("highscore_4");
        highscore[5] = PlayerPrefs.GetInt("highscore_5");
        highscore[6] = PlayerPrefs.GetInt("highscore_6");
        highscore[7] = PlayerPrefs.GetInt("highscore_7");
        highscore[8] = PlayerPrefs.GetInt("highscore_8");
        highscore[9] = PlayerPrefs.GetInt("highscore_9");
        highscore[10] = score;

        for (int i = 9; i >= 0; i--)
        {
            if (highscore[i] < highscore[i + 1])
            {
                int save = highscore[i + 1];
                highscore[i + 1] = highscore[i];
                highscore[i] = save;
                highscoreCounter--;
            }
        }
        PlayerPrefs.SetInt("highscore_0", highscore[0]);
        PlayerPrefs.SetInt("highscore_1", highscore[1]);
        PlayerPrefs.SetInt("highscore_2", highscore[2]);
        PlayerPrefs.SetInt("highscore_3", highscore[3]);
        PlayerPrefs.SetInt("highscore_4", highscore[4]);
        PlayerPrefs.SetInt("highscore_5", highscore[5]);
        PlayerPrefs.SetInt("highscore_6", highscore[6]);
        PlayerPrefs.SetInt("highscore_7", highscore[7]);
        PlayerPrefs.SetInt("highscore_8", highscore[8]);
        PlayerPrefs.SetInt("highscore_9", highscore[9]);

        Text first = scoreBoard.transform.GetChild(1).GetComponent<Text>();
        Text second = scoreBoard.transform.GetChild(2).GetComponent<Text>();
        Text third = scoreBoard.transform.GetChild(3).GetComponent<Text>();
        Text fourth = scoreBoard.transform.GetChild(4).GetComponent<Text>();
        Text fifth = scoreBoard.transform.GetChild(5).GetComponent<Text>();
        Text sixth = scoreBoard.transform.GetChild(6).GetComponent<Text>();
        Text seventh = scoreBoard.transform.GetChild(7).GetComponent<Text>();
        Text eighth = scoreBoard.transform.GetChild(8).GetComponent<Text>();
        Text ninth = scoreBoard.transform.GetChild(9).GetComponent<Text>();
        Text tenth = scoreBoard.transform.GetChild(10).GetComponent<Text>();

        first.text  = "First:   " + highscore[0];
        second.text = "Second:  " + highscore[1];
        third.text  = "Third:   " + highscore[2];
        fourth.text = "4:   " + highscore[3];
        fifth.text  = "5:   " + highscore[4];
        sixth.text  = "6:   " + highscore[5];
        seventh.text= "7:   " + highscore[6];
        eighth.text = "8:   " + highscore[7];
        ninth.text  = "9:   " + highscore[8];
        tenth.text  = "10:  " + highscore[9];

        for (int i = 1; i < 12; i++)
        {
            scoreBoard.transform.GetChild(i).GetComponent<Text>().color = new Color(0, 0, 0);
        }

        if(highscoreCounter == 11)
        {
            Text notOnBoard = scoreBoard.transform.GetChild(11).GetComponent<Text>();
            notOnBoard.text = "Your Score: "+score;
        }
        else
        {
            Text notOnBoard = scoreBoard.transform.GetChild(11).GetComponent<Text>();
            notOnBoard.text = "";
        }
        Text colour = scoreBoard.transform.GetChild(highscoreCounter).GetComponent<Text>();
        colour.color = new Color(255, 0, 0);

        scoreBoard.transform.GetChild(highscoreCounter).GetComponent<Text>();
        highscoreCounter = 11;
        scoreBoard.SetActive(true);

    }

    public void delete()
    {
        Debug.Log("Delete");
        PlayerPrefs.SetInt("highscore_0", 0);
        PlayerPrefs.SetInt("highscore_1", 0);
        PlayerPrefs.SetInt("highscore_2", 0);
        PlayerPrefs.SetInt("highscore_3", 0);
        PlayerPrefs.SetInt("highscore_4", 0);
        PlayerPrefs.SetInt("highscore_5", 0);
        PlayerPrefs.SetInt("highscore_6", 0);
        PlayerPrefs.SetInt("highscore_7", 0);
        PlayerPrefs.SetInt("highscore_8", 0);
        PlayerPrefs.SetInt("highscore_9", 0);

        Debug.Log(
    "Highscore_0 : " + PlayerPrefs.GetInt("highscore_0") + "\n" +
    "Highscore_1 : " + PlayerPrefs.GetInt("highscore_1") + "\n" +
    "Highscore_2 : " + PlayerPrefs.GetInt("highscore_2") + "\n" +
    "Highscore_3 : " + PlayerPrefs.GetInt("highscore_3") + "\n" +
    "Highscore_4 : " + PlayerPrefs.GetInt("highscore_4") + "\n" +
    "Highscore_5 : " + PlayerPrefs.GetInt("highscore_5") + "\n" +
    "Highscore_6 : " + PlayerPrefs.GetInt("highscore_6") + "\n" +
    "Highscore_7 : " + PlayerPrefs.GetInt("highscore_7") + "\n" +
    "Highscore_8 : " + PlayerPrefs.GetInt("highscore_8") + "\n" +
    "Highscore_9 : " + PlayerPrefs.GetInt("highscore_9") + "\n");
    }

    public void turnOffScoreBoard()
    {
        scoreBoard.SetActive(false);
    }
}
