using UnityEngine;

public class vegitableMovementForce : MonoBehaviour
{
    public bool MoveForceToZ;
    // Start is called before the first frame update
    int[] randomYDir;//= new int[2] {8, -8};
    private int yDir;
    public void Starter()
    {
        randomYDir = new int[2] { 8, -8 };
        Debug.Log("Random Direction ="+ randomYDir);
        yDir = randomYDir[Random.Range(0, randomYDir.Length)];
        Debug.Log("Y Direction ="+ yDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveForceToZ == true)
        {
            gameObject.GetComponent<Rigidbody>()
                .AddForce(yDir, 0, 0, ForceMode.Acceleration);
        }
    }
}
