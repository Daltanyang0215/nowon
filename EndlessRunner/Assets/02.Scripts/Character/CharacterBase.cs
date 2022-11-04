using UnityEngine;
[RequireComponent(typeof(AnimationManager))]
public abstract class CharacterBase : MonoBehaviour
{
    public float jumpForce;
    public int atk =1;
    protected Rigidbody rb;
    protected AnimationManager animationManager;
    public LayerMask targetLayer;
    public GameObject target;
    public float detectRange;
    public float detectAttackRange;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animationManager = GetComponent<AnimationManager>();
        GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
    }


    private void OnGameStateChanged(GameStates newState)
    {
        enabled = newState == GameStates.Play;

        switch (newState)
        {
            case GameStates.None:
            case GameStates.Idle:
            case GameStates.Play:
                animationManager.speed = 1;
                break;
            case GameStates.Paused:
                animationManager.speed = 0;
                break;
            default:
                break;
        }
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
    }
}

