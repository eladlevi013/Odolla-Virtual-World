using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBubble : MonoBehaviour {

	public static bool isPlaying = true;
	public float speed = 4f;

	public Rigidbody2D rb;

	private float movement = 0f;

	// Update is called once per frame
	void Update () {
		if (isPlaying)
		{
			movement = Input.GetAxisRaw("Horizontal") * speed;

			if (movement == 4)
			{
				GetComponent<Animator>().SetBool("walk", true);
				GetComponent<SpriteRenderer>().flipX = false;
			}
			if (movement == -4)
			{
				GetComponent<Animator>().SetBool("walk", true);
				GetComponent<SpriteRenderer>().flipX = true;
			}
			if (movement == 0)
			{
				GetComponent<Animator>().SetBool("walk", false);
			}
		}
	}

	void FixedUpdate ()
	{
		rb.MovePosition(rb.position + new Vector2 (movement * Time.fixedDeltaTime, 0f));
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.collider.tag == "Ball")
		{
			//Debug.Log("GAME OVER!");
			GameObject.Find("Chain_GFX").GetComponent<ChainCollision>().ENDGAME();
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

}
