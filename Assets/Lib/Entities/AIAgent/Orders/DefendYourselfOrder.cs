public class DefendYourselfOrder : AIOrder
{
    private SpaceEntity enemy;

    public DefendYourselfOrder(AIAgent a): base(AIOrderType.DefendYourself, a)
    {
        enemy = null;
    }

    public override void Execute()
    {
        if (enemy == null)
        {
            enemy = agent.manager.FindNearbyEnemy(agent.entity);
            if (enemy == null)
            {
                completed = true;
                return;
            }
        }

        agent.controller.MoveToLocation(enemy.transform, 2f, false);

        if (agent.IsLookingAtTheTarget(enemy.transform))
        {
            agent.entity.ShootCannonsInRange(enemy.transform);
        }

    }
}

