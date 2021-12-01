using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float rotationSpeed = 1f;
    public Color cuttingColor = Color.black;

    public Color normalColor = Color.white;

    public float byteDuration = 0.25f;

    private Rigidbody2D rb;
    private SpriteRenderer[] sr;

    private float forwardInput;
    private float rotationInput;
    private bool firePressed;
    private Color originalColor;

    public float lastClickTime = 0f;

    private readonly float CLICK_TIME_OUT = 0.1f;
    private List<GameObject> _cutObjects = new List<GameObject>();

    private Animator _animator;


    private GameManager _gameManager;

    public AudioClip footStep;
    public AudioClip zap;
    public AudioClip bugByte;
    public AudioClip successClip;
    public AudioClip exitEnabledClip;

    private AudioSource _audioSource;

    void Awake()
    {
        GameObjectUtil.KeepAliveOneCopy("Player", this.gameObject);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentsInChildren<SpriteRenderer>();
        lastClickTime = 0f;
        _gameManager = GameObjectUtil.GetGameManager();

        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            ProcessInputs();
            ProcessUpdate();
        }
    }

    private void ProcessUpdate()
    {
        if (firePressed)
        {
            if (lastClickTime > CLICK_TIME_OUT)
            {
                _animator.SetTrigger("Biting");
                CutObjects();
                lastClickTime = 0f;
            }
        }
        else
        {
            if (Math.Abs(forwardInput) >= 0.01f)
            {
                _animator.SetBool("Is_Walking", true);
            }
            else
            {
                _animator.SetBool("Is_Walking", false);
            }
        }
        lastClickTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Vertical");
        float moveY = Input.GetAxisRaw("Horizontal");

        forwardInput = moveX;
        rotationInput = - moveY;

        firePressed = Input.GetButton("Jump");

    }

    void Move()
    {
        if (!_animator.GetBool("Is_Fried"))
        {
            rb.velocity = transform.up * forwardInput * moveSpeed;
            transform.Rotate(new Vector3(0, 0, rotationInput * rotationSpeed));
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    public bool IsCutting()
    {
        return firePressed || lastClickTime < byteDuration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AddCutObjects(other.gameObject);

        if (this.IsCutting())
        {
            CutObjects();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        RemoveCutObject(other.gameObject);
    }

    private void RemoveCutObject(GameObject other)
    {
        if (other.TryGetComponent<CableScript>(out CableScript cableScript))
        {
            _cutObjects.Remove(other);
        }
    }

    private void AddCutObjects(GameObject other)
    {
        if (other.TryGetComponent<CableScript>(out CableScript cableScript))
        {
            _cutObjects.Add(other);
        }
    }

    private void CutObjects()
    {
        foreach (GameObject myObject in _cutObjects)
        {
            if (myObject != null && myObject.activeInHierarchy)
            {
                myObject.SendMessage("Cut");
            }
        }

        _gameManager.CheckExit();
    }

    public void Burn()
    {
        _animator.SetBool("Is_Fried", true);
    }

    public void Burning_Event()
    {
        _audioSource.PlayOneShot(zap);
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _animator.SetBool("Is_Fried", false);
        _animator.SetBool("Is_Walking", false);
        rb.isKinematic = false;

    }

    public void Walking_Event()
    {
        _audioSource.PlayOneShot(footStep);
    }

    public void Burned_Event()
    {
        _gameManager.Restart();
    }

    public void Success_Event()
    {
        _audioSource.PlayOneShot(successClip);
    }

    public void ExitEnabled_Event()
    {
        _audioSource.PlayOneShot(exitEnabledClip);
    }


    public void Byte_Event()
    {
        _audioSource.PlayOneShot(bugByte);
    }

}
