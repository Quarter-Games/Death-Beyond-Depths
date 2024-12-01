using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public bool isInstancing = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.material.enableInstancing = isInstancing;
        var material = new Material(spriteRenderer.material);
        spriteRenderer.material = material;
    }
    [ContextMenu("ChangeColor")]
    public void ChangeColor()
    {
        spriteRenderer.material.color = Random.ColorHSV();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
