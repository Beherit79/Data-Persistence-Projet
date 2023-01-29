using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    public UnityEvent<int> onDestroyed;

    public int pointValue;

    private void Start()
    {
        var brickRender = GetComponentInChildren<Renderer>();

        var block = new MaterialPropertyBlock();
        switch (pointValue)
        {
            case 1:
                block.SetColor(BaseColor, Color.green);
                break;
            case 2:
                block.SetColor(BaseColor, Color.yellow);
                break;
            case 5:
                block.SetColor(BaseColor, Color.blue);
                break;
            default:
                block.SetColor(BaseColor, Color.red);
                break;
        }

        brickRender.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(pointValue);

        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.2f);
    }
}