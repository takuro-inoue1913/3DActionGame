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
  //! 攻撃アニメーション中フラグ.
  bool isAttack = false;
  // 接地フラグ.
  bool isGround = false;

  // Start is called before the first frame update
  void Start()
  {
    // Animatorを取得し保管.
    animator = GetComponent<Animator>();
    // Rigidbodyの取得
    rigid = GetComponent<Rigidbody>();
    // 攻撃判定用オブジェクトを非表示に.
    attackHit.SetActive(false);

    // FootSphereのイベント登録.
    footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
    footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
    Debug.Log(animator.GetBool("isGround"));

  }

  // Update is called once per frame
  void Update()
  {

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

  // ---------------------------------------------------------------------
  /// <summary>
  /// FootSphereトリガーステイコール.
  /// </summary>
  /// <param name="col"> 侵入したコライダー. </param>
  // ---------------------------------------------------------------------
  void OnFootTriggerStay(Collider col)
  {
    if (col.gameObject.tag == "Ground")
    {
      if (isGround == false) isGround = true;
      if (animator.GetBool("isGround") == false) animator.SetBool("isGround", true);
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
}