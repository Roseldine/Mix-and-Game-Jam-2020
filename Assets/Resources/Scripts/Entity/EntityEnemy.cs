
using UnityEngine;

public class EntityEnemy : MonoBehaviour
{
    public enum enemyType { none, zombie, boss }
    public enum enemySprot { none, basketball, football, baseball}


    [Header("Enemy Type")]
    public enemyType _enemyType;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}