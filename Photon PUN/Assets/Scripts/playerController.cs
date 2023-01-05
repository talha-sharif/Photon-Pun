using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System;
using UnityEngine.UI;

public class playerController : MonoBehaviourPunCallbacks, iDamagable
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform UI;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    [SerializeField] private item[] items;

    private int itemIndex;
    private int previuosItemIndex = -1;

    private float verticalLookRotation;
    private bool isGrounded;
    private Vector3 smoothMoveVelocity, moveAmount;
    private Rigidbody rb;

    const float maxHealth = 100;
    float currentHealth = maxHealth;

    PhotonView PV;

    playerManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            equipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(UI.gameObject);
        }

        manager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<playerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        look();
        move();
        jump();

        for (int i = 0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                equipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            equipItem((itemIndex + 1) % items.Length);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if(itemIndex <= 0)
            {
                equipItem(items.Length - 1);
            }
            else
            {
                equipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].use();
        }

        if(transform.position.y < -10f)
        {
            die();
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);

        cameraHolder.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void setGrounded(bool _grounded)
    {
        isGrounded = _grounded;
    }

    void equipItem(int _index)
    {
        if (_index == previuosItemIndex)
            return;

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previuosItemIndex != -1)
        {
            items[previuosItemIndex].itemGameObject.SetActive(false);
        }

        previuosItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            equipItem((int)changedProps["itemIndex"]);
        }
    }

    public void takeDamage(float damage)
    {
        PV.RPC("rpc_takeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void rpc_takeDamage(float damage)
    {
        if (!PV.IsMine)
            return;

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        manager.die();
    }
}
