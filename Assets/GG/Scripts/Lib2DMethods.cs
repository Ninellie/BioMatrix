using UnityEngine;
using UnityEngine.InputSystem;

public static class Lib2DMethods
{
    /// <summary>
    /// Moves Rigidbody2D of the object in a direction
    /// </summary>
    /// <param name="rigidBody2D">The Rigidbody2D component of the object to be moved.</param>
    /// <param name="movementVector">2D vector towards which the object will be moved</param>
    /// <param name="movementSpeed">The speed at which the object will move</param>
    public static void MovePhys2D(Rigidbody2D rigidBody2D, Vector2 movementVector, float movementSpeed)
    {
        rigidBody2D.MovePosition(rigidBody2D.position + movementVector.normalized * movementSpeed * Time.fixedDeltaTime);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rigidBody2D"></param>
    public static void LookToPlayer(Rigidbody2D rigidBody2D)
    {
        float angle = (Mathf.Atan2(DirectionToPlayer(rigidBody2D.position).y, DirectionToPlayer(rigidBody2D.position).x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        rigidBody2D.rotation = angle;
    }
    public static Vector2 PlayerPos
    {
        get
        {
            var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
            var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;

            return new Vector2(horizontal, vertical);
        }
     }
    //Actually this is distance, not direction

    /// <summary>
    /// 
    /// </summary>
    /// <param name="myPos"></param>
    /// <returns></returns>
    public static Vector2 DirectionToPlayer(Vector2 myPos)
    {
        var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x - myPos.x;
        var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y - myPos.y;

        return new Vector2(horizontal, vertical);
    }

    public static Vector2 DirectionToMe (Vector2 myPos)
    {
        //var horizontal = myPos.x - GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
        //var vertical = myPos.y - GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;

        //return new Vector2(horizontal, vertical);
        return DirectionToTarget(myPos, GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
    }

    public static Vector2 DirectionToTarget(Vector2 myPos, Vector2 targetPos)
    {
        var horizontal = myPos.x - targetPos.x;
        var vertical = myPos.y - targetPos.y;

        return new Vector2(horizontal, vertical);
    }

    //public static Vector2 inputVector
    //{
    //    get
    //    {
    //        var horizontal = Input.GetAxisRaw("Horizontal");
    //        var vertical = Input.GetAxisRaw("Vertical");

    //        return new Vector2(horizontal, vertical);
    //    }
    //}

    //public static Vector2 ReportMousePosition()
    //{
    //    Vector2 mousePosition = Mouse.current.position.ReadValue();
    //    return mousePosition;
    //}

    public static Vector2 RandOnCircle(float radius)
    {
        float randAng = Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(randAng) * radius, Mathf.Sin(randAng) * radius);

    }
    public static float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
    public static float Range(float minInclusive, float maxInclusive)
    {
        float std = PeterAcklamInverseCDF.NormInv(Random.value);
        return PeterAcklamInverseCDF.RandomGaussian(std, minInclusive, maxInclusive);
    }
    public static float Range2(float minInclusive, float maxInclusive)
    {
        return PeterAcklamInverseCDF.RandomGaussian(minInclusive, maxInclusive);
    }
    public static Vector2 Rotate(Vector2 point, float angle)
    {
        Vector2 rotated_point;
        rotated_point.x = point.x * Mathf.Cos(angle)
                        - point.y * Mathf.Sin(angle);
        rotated_point.y = point.x * Mathf.Sin(angle)
                        + point.y * Mathf.Cos(angle);
        return rotated_point;
    }
    public static bool CheckVisibilityOnCamera(Camera camera, GameObject gameObject)
    {
        var screenPos = camera.WorldToScreenPoint(gameObject.transform.position);
        var onScreen = screenPos.x > 0f &&
                       screenPos.x < Screen.width &&
                       screenPos.y > 0f &&
                       screenPos.y < Screen.height;
        return onScreen;
    }
}
