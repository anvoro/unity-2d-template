using UnityEngine;
using UnityEngine.UI;

// Контроллер игрока
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 8000f;
    
    private StateMachine stateMachine;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    
    [SerializeField] private Slider slider;
    private int MaxHealth = 100;
    private int _currentHealth;

    private int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (CurrentHealth <= 0)
            {
                GameManager.Instance.RestartLevel();
            }
        }
    }

    // Свойства для состояний
    public bool IsGrounded => isGrounded;
    public Rigidbody2D Rigidbody => rb;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;

    void Start()
    {
        _currentHealth = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        
        // Создаем машину состояний
        stateMachine = new StateMachine();
        
        // Добавляем состояния
        stateMachine.AddState(new PlayerIdleState(this));
        stateMachine.AddState(new PlayerWalkState(this));
        stateMachine.AddState(new PlayerRunState(this));
        stateMachine.AddState(new PlayerJumpState(this));
        
        // Начальное состояние
        stateMachine.ChangeState<PlayerIdleState>();
    }
    
    void Update()
    {
        stateMachine.Update();
        
        // Проверка земли
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f);

        if (slider)
        {
            slider.value = (float)CurrentHealth/MaxHealth;
        }
    }
    
    public void Move(Vector3 direction, float speed)
    {
        Vector3 movement = direction * (speed * Time.deltaTime);
        transform.position += movement;
        
    }
    
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            CurrentHealth -= 20;
        }
    }
    
    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }
    
    public bool IsInState<T>() where T : IState
    {
        return stateMachine.IsInState<T>();
    }
}