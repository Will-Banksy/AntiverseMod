using System;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections;

namespace AntiverseMod.Utils; 

public static class EntityHelper {
	public delegate bool EntityFilter(EntityRef entity);

	/// <summary>
	/// Filters out entites that are not enemies with the specified "protagonist" entity
	/// </summary>
	public static EntityFilter EntityFilterEnemy(EntityRef protagonist) {
		return (entity) => Enemies(protagonist, entity);
	}

	/// <summary>
	/// Filters out entities that are not active. This should almost always be used
	/// </summary>
	public static EntityFilter EntityFilterActive() {
		return (entity) => entity.Generic().active;
	}

	/// <summary>
	/// Filter out any NPCs that can't be chased by the specified projectile
	/// </summary>
	public static EntityFilter EntityFilterNPCCanBeChased(Projectile proj) {
		return (entity) => {
			if (entity.type == EntityRef.Type.NPC) {
				return entity.NPC().CanBeChasedBy(proj);
			}

			return true;
		};
	}

	/// <summary>
	/// Filters out all entites that do not have a line of sight from fromPoint with dimensions width and height
	/// </summary>
	public static EntityFilter EntityFilterCollisionCanHit(Vector2 fromPoint, int width = 1, int height = 1) {
		return (entity) => Collision.CanHit(fromPoint, width, height, entity.Generic().position,
			entity.Generic().width, entity.Generic().height);
	}

	public static EntityFilter EntityFilterAll(EntityRef protagonist, Projectile proj, Vector2 fromPoint,
		int width = 1, int height = 1
	) {
		return EntityFilterActive() + EntityFilterEnemy(protagonist) + EntityFilterNPCCanBeChased(proj) +
			   EntityFilterCollisionCanHit(fromPoint, width, height);
	}

	public enum IterTypes {
		NPC,
		PLAYER,
		BOTH
	}

	public class AllEntities : IEnumerable {
		private readonly EntityFilter filter = null;
		public static readonly EntityFilter defaultFilter = EntityFilterActive();
		private readonly IterTypes iterTypes = IterTypes.BOTH;

		/// <summary>
		/// Collection of all entities (NPCs and Players). Produces an iterator through entites that pass the filter and are of type iterTypes
		/// </summary>
		public AllEntities(EntityFilter filter = null, IterTypes iterTypes = IterTypes.BOTH) {
			this.filter = filter;
			if (this.filter == null) {
				this.filter = defaultFilter;
			}

			this.iterTypes = iterTypes;
		}

		public IEnumerator GetEnumerator() {
			return new AllEntitiesIter(filter, iterTypes);
		}
	}

	public class AllEntitiesIter : IEnumerator {
		private enum ArrayIdx {
			NPC_ARRAY,
			PLAYER_ARRAY
		}

		public object Current {
			get {
				switch (idx) {
					case ArrayIdx.NPC_ARRAY:
						return new EntityRef(Main.npc[i]);

					case ArrayIdx.PLAYER_ARRAY:
						return new EntityRef(Main.player[i]);

					default:
						throw new Exception("idx is an unexpected value 😕");
				}
			}
		}

		private int i = 0;
		private ArrayIdx idx = ArrayIdx.NPC_ARRAY;
		private readonly EntityFilter filter = null;
		public static readonly EntityFilter defaultFilter = EntityFilterActive();
		private readonly IterTypes iterTypes = IterTypes.BOTH;

		/// <summary>
		/// Iterator through all entities (NPCs and Players). Iterates through entities that pass the filter and are of type iterTypes
		/// </summary>
		public AllEntitiesIter(EntityFilter filter = null, IterTypes iterTypes = IterTypes.BOTH) {
			this.filter = filter ?? defaultFilter;
			this.iterTypes = iterTypes;
			if (this.iterTypes == IterTypes.PLAYER) {
				idx = ArrayIdx.PLAYER_ARRAY;
			}
		}

		public bool MoveNext() {
			switch (idx) {
				case ArrayIdx.NPC_ARRAY:
					do {
						i++;
						if (i == Main.npc.Length) {
							if (iterTypes == IterTypes.NPC) {
								i--;
								return false;
							}

							idx = ArrayIdx.PLAYER_ARRAY;
							i = 0;
							return true;
						}
					} while (!Filter(new EntityRef(Main.npc[i])));

					return true;

				case ArrayIdx.PLAYER_ARRAY:
					do {
						i++;
						if (i == Main.player.Length) {
							i--;
							return false;
						}
					} while (!Filter(new EntityRef(Main.player[i])));

					return true;

				default:
					return false;
			}
		}

		public void Reset() {
			i = 0;
			idx = ArrayIdx.NPC_ARRAY;
		}

		public bool Filter(EntityRef entity) {
			foreach (EntityFilter fl in filter.GetInvocationList()) {
				if (!fl(entity)) {
					return false;
				}
			}

			return true;
		}
	}

	/// <summary>
	/// Sorts the combinations so the only possible combinations are NPC:NPC, NPC:PLAYER, PLAYER:PLAYER / Gets rid of the PLAYER:NPC combination possibility
	/// Might not work
	/// </summary>
	//public static void SortCombinations(ref EntityUnion one, ref EntityUnion other) {
	//	switch(one.type) {
	//		case EntityUnion.Type.NPC:
	//			return;

	//		case EntityUnion.Type.PLAYER:
	//			switch(other.type) {
	//				case EntityUnion.Type.NPC:
	//					EntityUnion tmp = one;
	//					one = other;
	//					other = tmp;
	//					break;
	//			}
	//			break;
	//	}
	//}

	/// <summary>
	/// Returns true if one and other are enemies - Like hostile NPC and friendly NPC, player and hostile NPC, or 2 players on different teams with pvp turned on
	/// </summary>
	public static bool Enemies(EntityRef one, EntityRef other) {
		//SortCombinations(ref one, ref other);
		switch (one.type) {
			case EntityRef.Type.NPC:
				switch (other.type) {
					case EntityRef.Type.NPC:
						return one.NPC().friendly != other.NPC().friendly;

					case EntityRef.Type.PLAYER:
						return !one.NPC().friendly;
				}

				return false;

			case EntityRef.Type.PLAYER:
				switch (other.type) {
					case EntityRef.Type.NPC:
						return !other.NPC().friendly;

					case EntityRef.Type.PLAYER:
						bool opposingTeams = (one.Player().team != other.Player().team) || (one.Player().team == 0 && other.Player().team == 0);
						bool bothPvp = one.Player().hostile && other.Player().hostile;

						return opposingTeams && bothPvp;
				}

				return false;

			default:
				return false;
		}
	}

	public static EntityRef AcquireTarget(Projectile proj, EntityRef projOwner, float? minDist = null, EntityFilter filter = null) {
		bool iterThroughPlayers = (projOwner.type == EntityRef.Type.PLAYER) && projOwner.Player().hostile;
		IterTypes iterTypes = iterThroughPlayers ? IterTypes.BOTH : IterTypes.NPC;
		EntityRef target = default;
		EntityFilter eFilter = filter ?? (EntityFilterActive() + EntityFilterEnemy(projOwner) + EntityFilterNPCCanBeChased(proj));
		foreach (EntityRef entity in new AllEntities(eFilter, iterTypes)) {
			float dist = Vector2.Distance(entity.Generic().Center, proj.Center);
			if (minDist == null || dist < minDist) {
				minDist = dist;
				target = entity;
			}
		}

		return target;
	}
}