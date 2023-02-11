using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Pot_Movement : MonoBehaviour
{

    public GameObject Apple;
    bool playing = true;
    float Speed = 3f;
    int Score = -1;

    public GameObject EndGameCanvas;
    public Text scoreText;
    public TextMeshProUGUI ScoreTextInCanvasEnd;

    // Start is called before the first frame update
    void Start()
    {
        spwanApple();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, transform.position.y, -2f);
            Apple.transform.Translate(Vector3.down * Time.deltaTime * Speed);

            IfGameObjectIsNear();
        }
    }

    void IfGameObjectIsNear()
    {
        float dist = Vector3.Distance(transform.position, Apple.transform.position);
        //Debug.Log(dist);

        if (dist < 1.5)
        {
            spwanApple();
        }
        if (Apple.transform.position.y < -5)
        {
            EndGame();
        }
    }

    void spwanApple()
    {
        System.Random r = new System.Random();
        Apple.transform.position = new Vector3(r.Next(-7, 7), 6.12f, -1.916f);
        Speed += 0.2f;
        Score += 1;
        scoreText.text = "Score: " + Score;
    }

    public async void EndGame()
    {
        await (Main.Instance.FirebaseHelper.UpdateUsersCoins(Score));
        playing = false;
        ScoreTextInCanvasEnd.text = " םייתסה קחשמה\n" + " תועבטמ " + Score + "-ב םתיכזו";
        EndGameCanvas.SetActive(true);
    }

    public void closeWindow()
    {
        EndGameCanvas.SetActive(false);
        SceneManager.LoadScene(sceneName: "Ringo's Hill");
    }
}