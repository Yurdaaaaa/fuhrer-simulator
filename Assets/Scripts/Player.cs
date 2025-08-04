using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private SpriteRenderer sprite;
    private Animator _animator;
    private string currentSide = "L"; // R = Right, L = Left

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameOver)
        {
            sprite.flipX = false;
            _animator.Play("Die");
        }
       
    }

    void SwitchPlayerSide(string newSide)
    {
        if (currentSide != newSide)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            sprite.flipX = !sprite.flipX;
            currentSide = newSide;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.gameOver = true;
        GameManager.instance.SaveScore();
    }
    public void TouchPlayer(string touchSide)
    {
        if (!GameManager.instance.gameOver)
        {
            SwitchPlayerSide(touchSide);
            _animator.Play("Cut");
        }
    }

}
