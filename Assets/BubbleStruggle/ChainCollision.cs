using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChainCollision : MonoBehaviour {

    public static int Score=0;
    public GameObject EndGameCanvas;
    public TextMeshProUGUI ScoreTextInCanvasEnd;
    public Text scoreText;

    public static int final_balls = 0;

    private void Start()
    {
        final_balls = 0;
        Score = 0;
        PlayerBubble.isPlaying = true;
    }

    void OnTriggerEnter2D (Collider2D col)
	{

		if (col.tag == "Ball")
		{
            Score++;
            scoreText.text = "Score: " + Score;

			Chain.IsFired = false;
			col.GetComponent<Ball>().Split();
            
            if(col.GetComponent<Ball>().nextBall == null)
                final_balls += 1;

            if (final_balls == 16)
            {
                ENDGAME();
                Score = 50;
            }
        }
		if(col.tag == "Top Wall")
        {
			Chain.IsFired = false;
		}
	}

    public void ENDGAME()
    {
        StartCoroutine(addCoins(Score));

        //StartCoroutine(Main.Instance.Web.addCoins(Score));
        PlayerBubble.isPlaying = false;

        //playing = false;
        //ScoreTextInCanvasEnd.text = "המשחק הסתיים\n" + "וזכיתם ב-" + Score + " מטבעות ";
        ScoreTextInCanvasEnd.text = " םייתסה קחשמה\n" + " תועבטמ " + Score + "-ב םתיכזו";
        EndGameCanvas.SetActive(true);
    }

    public void closeWindow()
    {
        EndGameCanvas.SetActive(false);
        SceneManager.LoadScene(sceneName: "Layla's Path");
    }

    public IEnumerator addCoins(int coins_add)
    {
        WWWForm form = new WWWForm();
        form.AddField("coins_to_add", coins_add);
        form.AddField("username", OnSceneSpawn.player_name);

        using (UnityWebRequest www = UnityWebRequest.Post("https://play-odolla.000webhostapp.com/addCoins.php", form))
        {
            yield return www.SendWebRequest();
        }
    }
}
