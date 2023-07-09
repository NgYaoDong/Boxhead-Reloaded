using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CodeMonkey.Utils;
using Pathfinding;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioClip spawnClip;
    public float moveSpeed = 5f;

    [Header("Slow Settings")]
    [SerializeField] private float recoveryTime = 0.5f;
    [SerializeField] private AudioClip slowClip;

    private Rigidbody2D rigidBody;
    [SerializeField] private Animator bottomAnimator;
    private float timer;
    private Vector2 movement;
    private CinemachineConfiner2D confiner;
    private SpriteRenderer rend;

    private GameObject checkpoint;
    private Transform arrow;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 90f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private AudioClip dashClip;
    private bool isDashing = false;
    private bool canDash = true;

    [Header("Aegis Settings")]
    [SerializeField] private float aegisDuration = 0.2f;
    [SerializeField] private float aegisCooldown = 5f;
    [SerializeField] private AudioClip aegisClip;
    private bool canAegis = true;
    private bool isAegis = false;
    
    [Header("Invisible Settings")]
    [SerializeField] private float invisibleDuration = 0.2f;
    [SerializeField] private float invisibleCooldown = 5f;
    [SerializeField] private AudioClip invisibleClip;
    [SerializeField] private AudioClip uninvisibleClip;
    private bool canInvis = true;
    
    [Header("Particles Settings")]
    [SerializeField] private ParticleSystem movementParticle;
    [Range(0, 0.2f)]
    [SerializeField] private float dustFormationPeriod;
    private float counter;

    private void Start()
    {
        confiner = GetComponentInChildren<CinemachineConfiner2D>();
        confiner.m_BoundingShape2D = GameObject.Find("CamBounds").GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        AudioSource.PlayClipAtPoint(spawnClip, transform.position, PlayerPrefs.GetFloat("SFX"));
        checkpoint = GameObject.Find("Checkpoint");
        arrow = transform.Find("Arrow");
    }

    private void Update()
    {
        if (!InGame.isPaused)
        {
            Recovery();
            Animate();
            CreateDust();
            Arrow();

            if (isAegis) AegisProjectiles();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (transform.name == "Player1(Clone)" && canDash) StartCoroutine(Dash());
                if (transform.name == "Player2(Clone)" && canAegis) StartCoroutine(Aegis());
                if (transform.name == "Fang(Clone)" && canInvis) StartCoroutine(Invisible());
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rigidBody.MovePosition(rigidBody.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
    }

    private void Animate()
    {
        if (isDashing) return;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bottomAnimator.SetFloat("Horizontal", movement.x);
        bottomAnimator.SetFloat("Vertical", movement.y);
        bottomAnimator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            bottomAnimator.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            bottomAnimator.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }

    private void Recovery()
    {
        if (moveSpeed < 5f)
        {
            timer += Time.deltaTime;
            Color32 lightBlue = new(0, 169, 255, 255);
            rend.material.color = lightBlue;
            bottomAnimator.GetComponentInParent<Renderer>().material.color = lightBlue;
            if (timer > recoveryTime)
            {
                timer = 0;
                moveSpeed = 5f;
                rend.material.color = Color.white;
                bottomAnimator.GetComponentInParent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void SlowEffect()
    {
        FindObjectOfType<InGame>().Slow();
        AudioSource.PlayClipAtPoint(slowClip, transform.position, 0.6f * PlayerPrefs.GetFloat("SFX"));
    }

    private void Arrow()
    {
        Vector2 dir = checkpoint.transform.position - arrow.position;
        if (checkpoint.activeSelf && dir.sqrMagnitude > 20f)
        {
            arrow.gameObject.SetActive(true);
            float angle = Mathf.Atan2(dir.y + 1.2f, dir.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else arrow.gameObject.SetActive(false);
    }

    private void CreateDust()
    {
        counter += Time.deltaTime;
        if (Mathf.Abs(movement.sqrMagnitude) > 0 && counter > dustFormationPeriod)
        {
            movementParticle.Play();
            counter = 0;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        FindObjectOfType<InGame>().InAbility(dashDuration);
        Vector3 movedir = new Vector3(movement.x, movement.y).normalized;
        transform.Find("Dash").localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(movedir));
        transform.Find("Dash").GetComponent<Animator>().SetTrigger("Dash");
        rigidBody.velocity = new Vector2(movement.normalized.x * dashSpeed, movement.normalized.y * dashSpeed);
        AudioSource.PlayClipAtPoint(dashClip, transform.position, 0.2f * PlayerPrefs.GetFloat("SFX"));
        yield return new WaitForSeconds(dashDuration);
        FindObjectOfType<InGame>().UseAbility(dashCooldown);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Aegis()
    {
        canAegis = false;
        isAegis = true;
        FindObjectOfType<InGame>().InAbility(aegisDuration);
        transform.Find("Aegis").GetComponent<Animator>().SetTrigger("Aegis");
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D target in targets)
        {
            if(target) target.GetComponent<AIPath>().maxSpeed = 0.5f;
        }
        AudioSource.PlayClipAtPoint(aegisClip, transform.position, PlayerPrefs.GetFloat("SFX"));
        yield return new WaitForSeconds(aegisDuration);
        FindObjectOfType<InGame>().UseAbility(aegisCooldown);
        isAegis = false;
        foreach (Collider2D target in targets)
        {
            if (target.name == "Arkzom(Clone)") target.GetComponent<AIPath>().maxSpeed = 2.5f;
            else if (target.name == "Skeleboar(Clone)") target.GetComponent<AIPath>().maxSpeed = 2f;
            else if (target.name == "Dopant(Clone)") target.GetComponent<AIPath>().maxSpeed = 2.5f;
            else if (target.name == "Orphenoch(Clone)") target.GetComponent<AIPath>().maxSpeed = 4.5f;
            else continue;
        }
        yield return new WaitForSeconds(aegisCooldown);
        canAegis = true;
    }

    private void AegisProjectiles()
    {
        Collider2D[] projectiles = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("Projectile"));
        foreach (Collider2D projectile in projectiles)
        {
            if (projectile) projectile.GetComponent<EnemyBullet>().Destroy();
        }
    }

    private IEnumerator Invisible()
    {
        canInvis = false;
        FindObjectOfType<InGame>().InAbility(invisibleDuration);
        transform.Find("Smoke").GetComponent<Animator>().SetTrigger("Smoke");
        AudioSource.PlayClipAtPoint(invisibleClip, transform.position, PlayerPrefs.GetFloat("SFX"));
        yield return new WaitForSeconds(0.517f);
        FindObjectOfType<InGame>().Smoke();
        Physics2D.IgnoreLayerCollision(6, 8, true);
        Physics2D.IgnoreLayerCollision(6, 9, true);
        Color32 invisColor = new(255, 255, 255, 170);
        rend.material.color = invisColor;
        bottomAnimator.GetComponentInParent<Renderer>().material.color = invisColor;
        yield return new WaitForSeconds(invisibleDuration - 0.5f);
        AudioSource.PlayClipAtPoint(uninvisibleClip, transform.position, PlayerPrefs.GetFloat("SFX"));
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<InGame>().UseAbility(invisibleCooldown);
        Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(6, 9, false);
        rend.material.color = Color.white;
        bottomAnimator.GetComponentInParent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(invisibleCooldown);
        canInvis = true;
    }
}