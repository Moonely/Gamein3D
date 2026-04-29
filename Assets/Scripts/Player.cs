using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float Speed = 10f;

    [SerializeField]
    private float SpeedDecrease = 0.9f;

    [SerializeField]
    private Rigidbody Body;

    private Vector2 _movement;

    [SerializeField]
    private int HP = 10;

    [SerializeField]
    private Slider HPSlider;

    [SerializeField]
    private GameObject GameOverScreen;

    [SerializeField]
    private GameObject ExplosionPrefab;

    [SerializeField]
    private TextMeshProUGUI CountScore;

    void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_movement.magnitude > 0)
        {
            Body.AddForce((Vector3)_movement * Speed);
        }
        else
        {
            Body.linearVelocity *= SpeedDecrease;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacle o = other.GetComponent<Obstacle>();
        if (o != null)
        {
            CountScore cs = FindAnyObjectByType<CountScore>();
            cs.score -= 75;
            int damages = o.Explode();
            HP -= damages;

            if (HPSlider != null)
                HPSlider.value = HP;

            transform.localScale += Vector3.one * 0.2f;
            if (HP <= 0)
            {
                ExplodePlayer();
                return;
            }
        }
    }

    private void ExplodePlayer()
    {
        if (ExplosionPrefab != null)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        }
        //enabled = false;
        GameOverScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}