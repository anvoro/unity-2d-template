using UnityEngine;

// Контроллер игрока
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    
    private StateMachine stateMachine;
    private Rigidbody rb;
    private bool isGrounded = true;
    
    // Свойства для состояний
    public bool IsGrounded => isGrounded;
    public Rigidbody Rigidbody => rb;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    
    public void Move(Vector3 direction, float speed)
    {
        Vector3 movement = direction * (speed * Time.deltaTime);
        transform.position += movement;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpForce, rb.velocity.z);
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