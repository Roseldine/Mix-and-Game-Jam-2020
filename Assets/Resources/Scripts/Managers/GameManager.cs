
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputManager _inputManager;

    public bool _managersAwake;

    public void Awake()
    {
        StartManagers();
    }


    void StartManagers()
    {
        InputManager.Instance = _inputManager;

        _managersAwake = true;
    }
}