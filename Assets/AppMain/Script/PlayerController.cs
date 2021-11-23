using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  // 攻撃判定用オブジェクト.
  [SerializeField] GameObject attackHit = null;
  // 設置判定用ColliderCall.
  [SerializeField] ColliderCallReceiver footColliderCall = null;
  // ジャンプ力.
  [SerializeField] float jumpPower = 20f;
  // アニメーター.
  Animator animator = null;
  // リジッドボディ.
  Rigidbody rigid = null;
  // 攻撃アニメーション中フラグ.
  bool isAttack = false;
  // 接地フラグ.
  bool isGround = false;

  // PCキー横方向入力.
  float horizontalKeyInput = 0;
  // PCキー縦方向入力.
  float verticalKeyInput = 0;


  // Start is called before the first frame update
  void Start()
  {
    // Animatorを取得し保管.
    animator = GetComponent<Animator>();
    // Rigidbodyの取得.
    rigid = GetComponent<Rigidbody>();
    // 攻撃判定用オブジェクトを非表示に.
    attackHit.SetActive(false);

    // FootSphereのイベント登録.
    footColliderCall.TriggerEnterEvent.AddListener(OnFootTriggerEnter);
    footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
  }

  // Update is called once per frame
  void Update()
  {
    // PCキー入力取得.
    horizontalKeyInput = Input.GetAxis("Horizontal");
    verticalKeyInput = Input.GetAxis("Vertical");

    // プレイヤーの向きを調整.
    bool isKeyInput = (horizontalKeyInput != 0 || verticalKeyInput != 0);
    if (isKeyInput == true && isAttack == false)
    {
      bool currentIsRun = animator.GetBool("isRun");
      if (currentIsRun == false) animator.SetBool("isRun", true);
      Vector3 dir = rigid.velocity.normalized;
      dir.y = 0;
      this.transform.forward = dir;
    }
    else
    {
      bool currentIsRun = animator.GetBool("isRun");
      if (currentIsRun == true) animator.SetBool("isRun", false);
    }
  }

  void FixedUpdate()
  {
    if (isAttack == false)
    {
      Vector3 input = new Vector3(horizontalKeyInput, 0, verticalKeyInput);
      Vector3 move = input.normalized * 2f;
      Vector3 cameraMove = Camera.main.gameObject.transform.rotation * move;
      cameraMove.y = 0;
      Vector3 currentRigidVelocity = rigid.velocity;
      currentRigidVelocity.y = 0;

      rigid.AddForce(cameraMove - currentRigidVelocity, ForceMode.VelocityChange);
    }
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// 攻撃ボタンクリックコールバック.
  /// </summary>
  // ---------------------------------------------------------------------
  public void OnAttackButtonClicked()
  {
    if (isAttack == false)
    {
      // AnimationのisAttackトリガーを起動.
      animator.SetTrigger("isAttack");
      // 攻撃開始.
      isAttack = true;
    }
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// ジャンプボタンクリックコールバック.
  /// </summary>
  // ---------------------------------------------------------------------
  public void OnJumpButtonClicked()
  {
    if (isGround == true)
    {
      rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// FootSphereトリガーエンターコール.
  /// </summary>
  /// <param name="col"> 侵入したコライダー. </param>
  // ---------------------------------------------------------------------
  void OnFootTriggerEnter(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      isGround = true;
      animator.SetBool("isGround", true);
    }
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// FootSphereトリガーイグジットコール.
  /// </summary>
  /// <param name="col"> 侵入したコライダー. </param>
  // ---------------------------------------------------------------------
  void OnFootTriggerExit(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      isGround = false;
      animator.SetBool("isGround", false);
    }
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// 攻撃アニメーションHitイベントコール.
  /// </summary>
  // ---------------------------------------------------------------------
  void Anim_AttackHit()
  {
    Debug.Log("Hit");
    // 攻撃判定用オブジェクトを表示.
    attackHit.SetActive(true);
  }

  // ---------------------------------------------------------------------
  /// <summary>
  /// 攻撃アニメーション終了イベントコール.
  /// </summary>
  // ---------------------------------------------------------------------
  void Anim_AttackEnd()
  {
    Debug.Log("End");
    // 攻撃判定用オブジェクトを非表示に.
    attackHit.SetActive(false);
    // 攻撃終了.
    isAttack = false;
  }
}