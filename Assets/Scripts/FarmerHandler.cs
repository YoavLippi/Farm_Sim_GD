using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmerHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer selectionRenderer;
    [SerializeField] private GameObject taskBar;
    [SerializeField] private Slider barSlider;
    
    [SerializeField] private Texture2D spriteSheet;

    [SerializeField] private float movementSpeed;
    [SerializeField] private bool selected = false;
    [SerializeField] private bool occupied = false;
    [SerializeField] private bool active = true;
    [SerializeField] private GameObject animalCollided;
    [SerializeField] private AnimalHandler collidedHandler;
    
    [SerializeField] private GameObject miniWheel;
    [SerializeField] private Rigidbody2D thisBody;
    [SerializeField] private Camera mainCamera;

    private Animator animator;
    private string currentState;
    
    private Vector2 desiredPosition;
    private Vector2 direction;
    private bool movedToPos;
    private bool selectionBuffer = false;

    private enum Animations
    {
        Farmer_Idle,
        Walk_Left,
        Walk_Right,
        Walk_Up,
        Walk_Down
    }

    public AnimalHandler CollidedHandler
    {
        get => collidedHandler;
        set => collidedHandler = value;
    }

    public bool Active
    {
        get => active;
        set => active = value;
    }

    public bool Occupied => occupied;

    public void setOccupied(bool val)
    {
        occupied = val;
        taskBar.SetActive(val);
        if (val)
        {
            if (collidedHandler) collidedHandler.Occupied = true;
        }
        else
        {
            if (selected)
            {
                miniWheel.SetActive(true);
            }
            StartCoroutine(setAnimalOccupied(collidedHandler));
        }
    }

    public void setTaskbarSize(float size)
    {
        barSlider.maxValue = size;
    }

    public void setTaskbarValue(float value)
    {
        barSlider.value = value;
    }

    private IEnumerator setAnimalOccupied(AnimalHandler ah)
    {
        yield return new WaitForSeconds(2f);
        if (!occupied || ah != collidedHandler) ah.Occupied = false;
    }

    public void setSelected(bool val)
    {
        selected = val;
        selectionRenderer.enabled = val;
        if (val)
        {
            miniWheel.SetActive(animalCollided);
        }
    }

    public bool isSelected()
    {
        return selected;
    }

    // Start is called before the first frame update
    void Start()
    {
        //miniWheel = GameObject.Find("Mini-Wheel");
        desiredPosition.x = transform.position.x;
        desiredPosition.y = transform.position.y;
        animator = GetComponent<Animator>();
    }

    void ChangeAnimationState(Animations a)
    {
        string newState = a.ToString();
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }
    // Update is called once per frame

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero,
                    Mathf.Infinity, LayerMask.GetMask("Farmers"));
                if (hit)
                {
                    if (hit.collider.attachedRigidbody == thisBody)
                    {
                        MouseClick();
                    }
                    else
                    {
                        selectionBuffer = true;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        bool idle = true;
        if (active && !occupied)
        {

            if (selected)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
                    {
                        ChangeAnimationState(Animations.Walk_Up);
                    }
                    transform.Translate(Vector3.up * (movementSpeed * Time.deltaTime));
                    idle = false;
                    movedToPos = true;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
                    {
                        ChangeAnimationState(Animations.Walk_Down);
                    }
                    transform.Translate(Vector3.down * (movementSpeed * Time.deltaTime));
                    idle = false;
                    movedToPos = true;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    if (!Input.GetKey(KeyCode.D))
                    {
                        ChangeAnimationState(Animations.Walk_Left);
                    }
                    transform.Translate(Vector3.left * (movementSpeed * Time.deltaTime));
                    idle = false;
                    movedToPos = true;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (!Input.GetKeyDown(KeyCode.A))
                    {
                        ChangeAnimationState(Animations.Walk_Right);
                    }
                    transform.Translate(Vector3.right * (movementSpeed * Time.deltaTime));
                    idle = false;
                    movedToPos = true;
                }
            }

            if (Vector2.Distance(desiredPosition, new Vector2(transform.position.x, transform.position.y)) > 0.15f)
            {
                if (!movedToPos)
                {
                    //transform.Translate(direction * (movementSpeed * Time.deltaTime));
                    thisBody.MovePosition(thisBody.position + (direction * (movementSpeed * Time.deltaTime)));
                    if (Math.Abs(direction.x) > Math.Abs(direction.y))
                    {
                        //horizontal
                        if (direction.x > 0)
                        {
                            ChangeAnimationState(Animations.Walk_Right);
                        }
                        else
                        {
                            ChangeAnimationState(Animations.Walk_Left);
                        }
                    }
                    else
                    {
                        //vertical
                        if (direction.y > 0)
                        {
                            ChangeAnimationState(Animations.Walk_Up);
                        }
                        else
                        {
                            ChangeAnimationState(Animations.Walk_Down);
                        }
                    }
                }
            }
            else
            {
                movedToPos = true;
            }
        }

        if (idle && movedToPos)
        {
            ChangeAnimationState(Animations.Farmer_Idle);
        }
    }

    private void MouseClick()
    {
        /*if (!selected && !occupied)
        {
            selectionBuffer = true;
        }*/
        SendMessageUpwards("ChangeSelectedToSpecific", this);
    }

    public void MoveToPos(Vector2 position)
    {
        /*if (selectionBuffer)
        {
            selectionBuffer = false;
            return;
        }*/
        if (selected)
        {
            movedToPos = false;
            desiredPosition = position;
            //Debug.Log($"position x: {position.x}, position y: {position.y}");
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            direction = desiredPosition - currentPos;
            direction.Normalize();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        movedToPos = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Animal") && !occupied)
        {
            animalCollided = col.transform.parent.gameObject;
            collidedHandler = animalCollided.GetComponent<AnimalHandler>();
            if (selected)
            {
                miniWheel.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!occupied && !miniWheel.activeSelf)
        {
            if ((other.gameObject.CompareTag("Animal") || animalCollided) && selected)
            {
                miniWheel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Animal") && this.gameObject.GetComponent<CapsuleCollider2D>().enabled)
        {
            animalCollided = null;
            collidedHandler = null;
            miniWheel.SetActive(false);
        }
    } 
}
