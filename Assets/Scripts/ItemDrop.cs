using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    
    public float dropForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // ini supaya pas drop soulnya, dia agak loncat dikit baru jatuh
        // rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
    }
}