using UnityEngine;
public class CharacterBase : MonoBehaviour
{
    public float jumpForce;
    public Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
    }


    private void OnGameStateChanged(GameStates newState)
    {
        enabled = newState == GameStates.Play;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
    }
}

