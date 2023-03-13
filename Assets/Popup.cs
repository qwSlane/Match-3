using CodeBase.UIScripts;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button _restart;

    public void Construct(UIKernel kernel)
    {
        _restart.onClick.AddListener(kernel.Restart);
    }
}