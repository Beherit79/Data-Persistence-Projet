using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxMovement = 2.0f;

    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        var input = Input.GetAxis("Horizontal");

        var pos = transform.position;
        pos.x += input * speed * Time.deltaTime;

        if (pos.x > maxMovement)
            pos.x = maxMovement;
        else if (pos.x < -maxMovement)
            pos.x = -maxMovement;

        transform.position = pos;
    }
}