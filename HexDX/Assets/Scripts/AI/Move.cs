using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move
{
    public Unit unit;
    public Tile destination;
    public int facing;
    public Unit target;
    private float netkills = 0;
    private Unit closestEnemy;
    private float remainingHealth=300;
    public static Dictionary<Tile, Unit> closestEnemyCache;
    public static Dictionary<Tile, float> closestDistanceCache;
    public Move(Unit u, Tile d, int f, Unit t)
    {
        unit = u;
        destination = d;
        facing = f;
        target = t;
    }

    public float score()
    {
        float netdamage = NetDamage();
        float distanceToEnemy = 0;
        if (netdamage > 0)
            distanceToEnemy = closestEnemyDist();
        else
            distanceToEnemy -= closestEnemyDist();
        float score = 10000.0f * netkills + 10 * netdamage + 100*distanceToEnemy-remainingHealth;
        if (closestEnemy)
        {
            Vector3 directionVec = closestEnemy.transform.position - destination.transform.position;
            int bestfacing = unit.GetFacing(new Vector2(directionVec.x, directionVec.y));
            if (facing == bestfacing)
                score += 1;
        }
        return score;
    }

    private float NetDamage()
    {
        float damage = 0;
        damage -= HexMap.GetPathDamage(unit.GetShortestPath(destination));
        if (destination == unit.currentTile)
        {
            if (target && unit.GetDamage(target, destination)<target.Health && target.HasInAttackRange(unit))
                damage-= target.GetDamage(unit, target.ZOCModifer);
        }
        damage = Mathf.Max(-unit.Health, damage);
        if (unit.Health + damage <= 0)
        {
            netkills -= 1;
        }


        if (target && netkills >= 0)
        {
            damage += Mathf.Min(unit.GetDamage(target, destination), target.Health);
            if (target.Health <= unit.GetDamage(target, destination))
            {
                netkills += 1;
                remainingHealth = 0;
            }
            remainingHealth = target.Health - unit.GetDamage(target, destination);
        }
        return damage;
    }


    private float closestEnemyDist()
    {
        // calculate distance to nearest enemy (1.0 if right next to ai 1.0 / dist otherwise)
        float minDistance = 100.0f;
        if (closestEnemyCache.ContainsKey(destination))
        {
            closestEnemy = closestEnemyCache[destination];
            return closestDistanceCache[destination];
        }

        foreach (Unit u in BattleController.instance.player.units)
        {
            if (u.enabled)
            {
                float dist = HexMap.Cost(u.currentTile, destination);
                if (dist < 10)
                    dist += Mathf.Min(50.0f, unit.GetShortestPathLength(u.currentTile, destination));
                else
                    dist += 50.0f;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestEnemy = u;
                }
            }
        }
        closestDistanceCache[destination] = minDistance;
        closestEnemyCache[destination] = closestEnemy;
        return minDistance;
    }

}
