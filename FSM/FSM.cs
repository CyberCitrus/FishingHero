using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;
using System.Security.Cryptography;
using UnityEngine.UI;

public class FSM : MonoBehaviour
{
    public Image hpbar;
    public enum StateType
    {
        Idle, Patrol, Chase, Attack, Dead
    }

    [Serializable]
    public class Parameter
    {
        // 敌人的参数
        public float hp;
        public float hpMax;
        public float moveSpeed;
        public float chaseSpeed;
        public float idleTime;
        public Transform[] patrolPoints;
        public Transform[] chasePionts;
        public Transform target;
        public SkeletonAnimation ani;
        public LayerMask targerLayer;
        public Transform attackPoint;
        public float attackArea;
        public bool isDead;
    }

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    public Parameter parameter;
    public AnimationReferenceAsset wait, move, attack, die;
    private string curAni;
    void Start()
    {
        parameter.ani = GetComponent<SkeletonAnimation>();
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Dead, new DeadState(this));

        ChangeState(StateType.Idle);
        curAni = "wait";
        parameter.isDead = false;
        parameter.hp = 50f;
        parameter.hpMax = parameter.hp;
        hpBarENE.healthMax = parameter.hp;
    }

    void Update()
    {
        currentState.OnUpdate();
        hpBarENE.healthCurrent = parameter.hp;
        hpbar.fillAmount = parameter.hp / parameter.hpMax;
    }

    public void ChangeState(StateType state)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[state];

    }

    public void FlipTo(Transform target)
    {
        if(target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(curAni))
        {
            return;
        }
        parameter.ani.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        curAni = animation.name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            parameter.target = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
    void damage()
    {
        parameter.hp -= 1;
        if (parameter.hp < 0)
        {
            parameter.hp = 0;
        }
    }
    public void dead()
    {
        Destroy(this.gameObject, 2f);
    }
}
