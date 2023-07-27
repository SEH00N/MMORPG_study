using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int monsterCount = 0;
    private int reserveCount;
    [SerializeField] int keepMonsterCount = 0;

    [SerializeField] Vector3 spawnPos;
    [SerializeField] float spawnRadius = 15f;

    [SerializeField] float spawnTime = 5f;

    private void Start()
    {
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while(reserveCount + monsterCount < keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    private IEnumerator ReserveSpawn()
    {
        reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, spawnTime));

        GameObject obj = Managers.Game.Spawn(DEFINE.WorldObject.Monster, "Knight");
        NavMeshAgent nav = obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;
        while(true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
            randDir.y = 0;
            
            randPos = spawnPos + randDir;
            
            NavMeshPath path = new NavMeshPath();
            if(nav.CalculatePath(randPos, path))
                break;
        }

        obj.transform.position = randPos;
        reserveCount--;
    }

    public void AddMonsterCount(int value) => monsterCount += value;
    public void SetKeepMonsterCount(int count) => keepMonsterCount = count;
}
