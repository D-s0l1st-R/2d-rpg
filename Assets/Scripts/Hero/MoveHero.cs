using UnityEngine;

public class MoveHero : MonoBehaviour
{
    [Header("Настройки")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f; // Скорость поворота (градусов в секунду)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    // Хранит последнее направление. Нужно для анимации "простоя" (Idle), 
    // чтобы персонаж смотрел туда, куда шел перед остановкой.
    private Vector2 lastMovement;

    // Сохраняем исходный масштаб для корректного отзеркаливания
    private float originalScaleX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastMovement = Vector2.left; // Направление по умолчанию (например, вниз)
        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        // Считываем ввод: Horizontal (A=-1, D=1), Vertical (S=-1, W=1)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical);

        // Нормализация: чтобы движение по диагонали (W+D) не было быстрее прямого
        movement = movement.normalized;

        // Если есть движение, запоминаем направление и поворачиваем спрайт
        if (movement != Vector2.zero)
        {
            lastMovement = movement;

            // --- ВЫБЕРИТЕ ОДИН ИЗ ДВУХ ВАРИАНТОВ ПОВОРОТА НИЖЕ ---

            // ВАРИАНТ А: Плавный поворот трансформации (для вид сверху / изометрии)
            // RotateSpriteSmoothly(movement);

            // ВАРИАНТ Б: Простое отзеркаливание по оси X (для платформеров / вид сбоку)
            FlipSpriteX(movement.x);
        }

        // Здесь можно передать данные в Animator, если он есть:
        // animator.SetFloat("Speed", movement.magnitude);
        // animator.SetFloat("LastX", lastMovement.x);
        // animator.SetFloat("LastY", lastMovement.y);
    }

    void FixedUpdate()
    {
        // MovePosition учитывает коллайдеры, персонаж не пройдет сквозь стены
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // ==========================================
    // МЕТОДЫ ПОВОРОТА (Используйте только один)
    // ==========================================

    // Вариант Б: Отзеркаливание спрайта (только влево/вправо)
    private void FlipSpriteX(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            // Если идем вправо (1), масштаб положительный. Влево (-1) - отрицательный.
            float targetScaleX = Mathf.Sign(horizontalInput) * originalScaleX;
            transform.localScale = new Vector3(targetScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
}

