
using UnityEngine;

public class UiUtilityBase : MonoBehaviour
{
    public enum targetAxis { none, horizontal, vertical, both }

    [Header("Base Components")]
    [SerializeField] protected RectTransform _container;
}
