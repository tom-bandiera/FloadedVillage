using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridManager.Runtime
{
    public class EnumTile : MonoBehaviour
    {
	    public enum TYPE
	    {
		    NONE,
		    EMPTY,
		    WATER,
		    SAND,
		    STONE,
		    SEEDS,
		    CROPS,
		    BRIDGE,
		    BRIDGE_WATER,
		    VILLAGER,
		    VILLAGER_DROWNED,
		    ZOMBIE,
		    ZOMBIE_DROWNED
	    }
    }

}
