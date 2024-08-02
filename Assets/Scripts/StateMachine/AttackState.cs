using UnityEngine;

public class AttackState : IState
{
    float timer;

    public void OnEnter(Enemy enemy)
    {
        if (enemy.target != null)
        {
            enemy.ChangeDirection(enemy.target.transform.position.x > enemy.transform.position.x);
            enemy.StopMoving();
            enemy.Attack();
        }

        timer = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}