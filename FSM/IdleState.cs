using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FSM;

public class IdleState : IState
{
    private FSM manager;
    private Parameter parameter;
    private float timer;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        manager.SetAnimation(manager.wait, true, 1f);
    }
    public void OnUpdate()
    {
        if (parameter.hp <= 0)
        {
            manager.ChangeState(StateType.Dead);
        }
        manager.SetAnimation(manager.wait, true, 1f);
        timer += Time.deltaTime;

        if(parameter.target != null &&
            parameter.target.position.x >= parameter.chasePionts[0].position.x &&
            parameter.target.position.x <= parameter.chasePionts[1].position.x)
        {
            manager.ChangeState(StateType.Chase);
        }

        if (timer >= parameter.idleTime)
        {
            manager.ChangeState(StateType.Patrol);
        }
    }
    public void OnExit()
    {
        timer = 0;
    }
}
public class PatrolState : IState
{
    private FSM manager;
    private Parameter parameter;

    private int patrolPosition;
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        manager.SetAnimation(manager.move, true, 1f);
    }
    public void OnUpdate()
    {
        if (parameter.hp <= 0)
        {
            manager.ChangeState(StateType.Dead);
        }
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);
        if (parameter.target != null &&
            parameter.target.position.x >= parameter.chasePionts[0].position.x &&
            parameter.target.position.x <= parameter.chasePionts[1].position.x)
        {
            manager.ChangeState(StateType.Chase);
        }
        manager.SetAnimation(manager.move, true, 1f);
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);
        if(Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position)
            < .1f){
            manager.ChangeState(StateType.Idle);
        }
    }
    public void OnExit()
    {
        patrolPosition++;
        if(patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;
        }
    }
}

public class ChaseState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        manager.FlipTo(GameObject.Find("player").transform);
        manager.SetAnimation(manager.move, true, 1.5f);
    }
    public void OnUpdate()
    {
        if (parameter.hp <= 0)
        {
            manager.ChangeState(StateType.Dead);
        }
        manager.FlipTo(parameter.target);
        Debug.Log(parameter.target + "ChaseMode");
        manager.SetAnimation(manager.move, true, 1.5f);
        if (parameter.target != null)
        {
            if (parameter.target.CompareTag("12Skill"))
            {
                manager.ChangeState(StateType.Idle);
            }
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        }
        if(parameter.target == null ||
            manager.transform.position.x < parameter.chasePionts[0].position.x ||
            manager.transform.position.x > parameter.chasePionts[1].position.x)
        {
            manager.ChangeState(StateType.Idle);
        }

        if(Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targerLayer))
        {
            manager.ChangeState(StateType.Attack);
        }
    }
    public void OnExit()
    {

    }
}

public class AttackState : IState
{
    private FSM manager;
    private Parameter parameter;
    private float attackcd;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        manager.SetAnimation(manager.attack, false, 1.5f);
    }
    public void OnUpdate()
    {
        if (parameter.hp <= 0)
        {
            manager.ChangeState(StateType.Dead);
        }
        attackcd += Time.deltaTime;
        manager.SetAnimation(manager.attack, false, 1f);
        if (attackcd > 1f)
        {
            if (GameObject.Find("player") != null) { 
            GameObject.Find("player").SendMessage("damage");
            attackcd = 0;
            manager.ChangeState(StateType.Chase);
                }
        }
    }
    public void OnExit()
    {

    }
}
public class DeadState : IState
{
    private FSM manager;
    private Parameter parameter;
    float deadTime;

    public DeadState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }


    public void OnEnter()
    {
        manager.SetAnimation(manager.die, false, 1f);
        deadTime = 0;
        parameter.isDead = true;
    }
    public void OnUpdate()
    {
        manager.SetAnimation(manager.die, false, 1f);
        deadTime += Time.deltaTime;
        manager.dead();
    }
    public void OnExit()
    {

    }
}