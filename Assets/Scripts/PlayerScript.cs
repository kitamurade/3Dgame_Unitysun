using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    CharacterController con;
    Animation anim;

    float normalSpeed = 3f; //通常時の移動速度
    float sprintSpeed = 5f; //ダッシュ時の移動速度
    float jump = 8f;        //ジャンプ力
    float gravity = 10f;    //重力の大きさ

    Vector3 moveDirection= Vector3.zero;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        con = GetComponent & lt; CharacterController & gt;();
        anim = GetComponent & lt; Animator & gt;();

        //マウスカーソルを非表示にし、位置を固定
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //移動速度を取得
        float speed=Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

        //カメラの向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //前後左右の入力から、移動のためのベクトルを計算
        Vector3 moveZ = cameraForward * Input.GetAxis("Vertical") * speed;
        Vector3 moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed;

        //地面にいるときはジャンプを可能に
        if(con.isGrounded)
        {
            moveDirection = moveZ + moveX;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }
        else
        {
            //重力を効かせる
            moveDirection=moveZ+moveX+new Vector3(0,moveDirection.y,0);
            moveDirection.y-=gravity*Time.deltaTime;
        }
        //移動のアニメーション
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);
        //プレイヤーの向きを入力の向きに変更
        transform.LookAt(transform.position + moveZ + moveX);
        //moveは指定したベクトルだけを移動させる命令
        con.Move(moveDirection*Time.deltaTime);
    }
}
