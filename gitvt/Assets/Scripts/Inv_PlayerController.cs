using UnityEngine;

public class Inv_PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    float currentSpeed;
    Rigidbody rb;
    Vector3 direction;
    float jumpForce = 7f;
    bool isGrounded;

    //Скрипт инвентаря
    private Inv_Inventory inventory;
    void Start()
    {
        //Получаем объект, в котором есть скрипт инвенторя и заносим его в переменную 
        inventory = FindObjectOfType<Inv_Inventory>();
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            isGrounded = false;
            rb.AddForce(new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    //Если мы стокнулись с предметом у которого есть галочка isTrigger, то...
    private void OnTriggerEnter(Collider other)
    {                
        //Если у данного объекта есть скрипт Collected, то..
        if(other.gameObject.GetComponent<Inv_Collected>() == true)
        {
            //Получаем имя этого объекта из скрипта
            string name = other.gameObject.GetComponent<Inv_Collected>().name;
            //Получаем картинку этого объекта из скрипта
            Sprite image = other.gameObject.GetComponent<Inv_Collected>().image;
            //Вызываем метод AddItem, который находится в скрипте инвентаря и передаем ему имя, картинку и сам GameObject, который мы взяли
            inventory.AddItem(image, name, other.gameObject);
        }
    }
}
