using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgro : FlyingBaseAgro
{
	public float ydecal = 1;


	protected override void FixedUpdate ()
	{
		move = 0;
		movey = 0;
		if (!base.cannotmove)
		{
            if (Cible && !istapping)
			{
				float distance = Vector2.Distance(Cible.position, transform.position);
				Vector2 realCible = (Vector2)Cible.position + ((new Vector2((Cible.position.x - transform.position.x < 0) ? 1 : -1, ydecal)).normalized * perfectdistancetocible);
				List<Pathfinder.Node> path = GameManager.instance.pathfinderGrid.GetAStar(GameManager.instance.pathfinderGrid.FindClosestNode(transform.position), GameManager.instance.pathfinderGrid.FindClosestNode(realCible), PathfindingProfileId);
				if (path != null)
				{
					realCible = path[1].pos;
					if (Time.frameCount % 100 == 0)
						path.ForEach(p => Debug.Log(p));
				}
				else
					Debug.Log("fdsf2"/* + nodeAttach.pos + GameManager.instance.pathfinderGrid.FindClosestNode(realCible).pos*/);
                if (distance > MaxDistance)
                {
                    Cible = null; // peut etre active reactive qaund respawn pres
                    return ;
                }
				Vector2 dir = (realCible - (Vector2)transform.position).normalized;
				move = dir.x;
				movey = dir.y;
			}
		}
		PCFixedUpdate();
	}
}
