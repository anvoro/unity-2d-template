using UnityEngine;

// Контроллер врага
public class EnemyController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    
    [Header("Точки патрулирования")]
    public Transform[] patrolPoints;
    
    [Header("Цель")]
    public Transform player;
    
    private StateMachine stateMachine;
    private int currentPatrolIndex = 0;
    
    // Свойства для доступа из состояний
    public int CurrentPatrolIndex 
    { 
        get { return currentPatrolIndex; } 
        set { currentPatrolIndex = value; }
    }
    
    void Start()
    {
        // Создаем машину состояний
        stateMachine = new StateMachine();
        
        // Создаем и добавляем состояния
        stateMachine.AddState(new IdleState(this));
        stateMachine.AddState(new PatrolState(this));
        stateMachine.AddState(new ChaseState(this));
        stateMachine.AddState(new AttackState(this));
        
        // Устанавливаем начальное состояние
        stateMachine.ChangeState<IdleState>();
    }
    
    void Update()
    {
        // Обновляем текущее состояние
        stateMachine.Update();
    }
    
    // Методы для использования в состояниях
    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Vector3.Distance(transform.position, player.position);
    }
    
    public void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        // Поворот в сторону движения
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    // Публичный доступ к StateMachine для состояний
    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }
    
    // Визуализация для отладки
    void OnDrawGizmosSelected()
    {
        // Радиус обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Радиус атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}