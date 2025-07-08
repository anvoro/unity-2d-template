using UnityEngine;

// Состояние покоя игрока
public class PlayerIdleState : IState
{
    private PlayerController player;
    
    public PlayerIdleState(PlayerController player)
    {
        this.player = player;

        InputManager.Instance.OnJumpPressed += () =>
        {
            if (player.IsGrounded)
            {
                player.ChangeState<PlayerJumpState>();
            }
        };
    }
    
    public void Enter()
    {
        Debug.Log("Игрок в состоянии покоя");
    }
    
    public void Execute()
    {
        // Получаем ввод
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Проверяем переходы
        if (horizontal != 0 || vertical != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.ChangeState<PlayerRunState>();
            }
            else
            {
                player.ChangeState<PlayerWalkState>();
            }
        }
    }
    
    public void Exit()
    {
        // Ничего не делаем при выходе
    }
}

// Состояние ходьбы
public class PlayerWalkState : IState
{
    private PlayerController player;
    
    public PlayerWalkState(PlayerController player)
    {
        this.player = player;
    }
    
    public void Enter()
    {
        Debug.Log("Игрок идет");
    }
    
    public void Execute()
    {
        // Получаем ввод
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        // Движение
        player.Move(direction, player.WalkSpeed);
        
        // Проверяем переходы
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
        {
            player.ChangeState<PlayerJumpState>();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            player.ChangeState<PlayerRunState>();
        }
        else if (direction == Vector3.zero)
        {
            player.ChangeState<PlayerIdleState>();
        }
    }
    
    public void Exit()
    {
        // Ничего не делаем при выходе
    }
}

// Состояние бега
public class PlayerRunState : IState
{
    private PlayerController player;
    
    public PlayerRunState(PlayerController player)
    {
        this.player = player;
    }
    
    public void Enter()
    {
        Debug.Log("Игрок бежит");
    }
    
    public void Execute()
    {
        // Получаем ввод
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        // Движение
        player.Move(direction, player.RunSpeed);
        
        // Проверяем переходы
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
        {
            player.ChangeState<PlayerJumpState>();
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && direction != Vector3.zero)
        {
            player.ChangeState<PlayerWalkState>();
        }
        else if (direction == Vector3.zero)
        {
            player.ChangeState<PlayerIdleState>();
        }
    }
    
    public void Exit()
    {
        // Ничего не делаем при выходе
    }
}

// Состояние прыжка
public class PlayerJumpState : IState
{
    private PlayerController player;
    private float jumpTimer = 0f;
    
    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
    }
    
    public void Enter()
    {
        Debug.Log("Игрок прыгает");
        player.Jump();
        jumpTimer = 0f;
    }
    
    public void Execute()
    {
        jumpTimer += Time.deltaTime;
        
        // Движение в воздухе
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        // Уменьшенная скорость в воздухе
        player.Move(direction, player.WalkSpeed * 0.7f);
        
        // Проверяем приземление
        if (player.IsGrounded && jumpTimer > 0.1f)
        {
            if (direction != Vector3.zero)
            {
                player.ChangeState<PlayerWalkState>();
            }
            else
            {
                player.ChangeState<PlayerIdleState>();
            }
        }
    }
    
    public void Exit()
    {
        // Ничего не делаем при выходе
    }
}