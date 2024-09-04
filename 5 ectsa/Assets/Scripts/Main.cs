using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool jede = false, mobitel = false, pije = false;
    public bool onCamera = false;
    public int score = 0;
    private int highscore = 100; // dobiva int iz filea, stavit u  start da dobiva iz filea

    private float timer = 0f;
    public float kolikoTrebaProc = 1f;

    public GameObject rotirajuciNino;
    public GameObject kraj;
    void Start()
    {
        //tekst koji oznacava game over je uvijek hidden na pocetku
        kraj.SetActive(false);
    }
    void Update()//svaki frame
    {
        jede = Input.GetKey(KeyCode.A);
        pije = Input.GetKey(KeyCode.S);
        mobitel = Input.GetKey(KeyCode.D);
        //eliminira double input, ako je jedan aktivan ostali ne mogu bit
        if (jede)
        {
            jede = true;
            pije = false;
            mobitel = false;
        }
        else if (pije)
        {
            jede = false;
            pije = true;
            mobitel = false;
        }
        else if (mobitel)
        {
            jede = false;
            mobitel = true;
            pije = false;
        }
        float capsuleYDegree = rotirajuciNino.transform.eulerAngles.y;

        if (capsuleYDegree < 45 && capsuleYDegree > -1)
        {
            onCamera = true;
        }
        else
        {
            onCamera = false;
        }

        if (onCamera)
        {
            if (jede || pije || mobitel)
            {

                gameOver(highscore, score);
            }


        }

        timer += Time.deltaTime;
        if (timer >= kolikoTrebaProc)
        {
            timer = 0f;
            score++;


        }

        void gameOver(int highscore, int score)//dobiva spremljeni high score iz filea
        {
            //kada je jede/pije/spava aktivan krug postaje unhiden
            kraj.SetActive(true);

            if (score > highscore)
            {
                highscore = score;
                //spremi score na memoriju highscorea
                // print NEW HIGH SCORE
                // UI ZA GAME OVER
                // UI ZA HIGH SCORE (best score:highscore)
            }
            else
            {
                // UI ZA GAME OVER
                //print score: best score: highscore
                //             current run: score
            }

            //ENDAJ PROGRAM
        }

    }

}
