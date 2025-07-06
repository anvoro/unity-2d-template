using UnityEngine;

// Контроллер игрока
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    
    private StateMachine stateMachine;
    private Rigidbody rb;
    private bool isGrounded = true;
    
    // Свойства для состояний
    public bool IsGrounded => isGrounded;
    public Rigidbody Rigidbody => rb;
    
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
        Vector3 movement = direction * speed * Time.deltaTime;
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
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
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