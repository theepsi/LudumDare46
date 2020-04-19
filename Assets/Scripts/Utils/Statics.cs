using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statics
{
    public struct Events
    {
        public static readonly string hullDamaged = "hullDamaged";
        public static readonly string oxygenLost = "oxygenLost";

        public static readonly string gameOver = "gameOver";

        public static readonly string moduleHitPlayer = "moduleHitPlayer";
        public static readonly string moduleHitAsteroid = "moduleHitAsteroid";

        public static readonly string baseFound = "baseFound";
    }
}
