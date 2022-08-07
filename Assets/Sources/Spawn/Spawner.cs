using UnityEngine;

namespace Sources.Spawn
{
    public abstract class Spawner : MonoBehaviour
    {
        public abstract void StartSpawning();

        public abstract void ClearSpawnedAndStopSpawning();
    }
}