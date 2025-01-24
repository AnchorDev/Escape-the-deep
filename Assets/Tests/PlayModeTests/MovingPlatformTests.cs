using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovingPlatformTests
{
    private GameObject platformObject;
    private MovingPlatform movingPlatform;
    private GameObject playerObject;

    [SetUp]
    public void SetUp()
    {
        platformObject = new GameObject("MovingPlatform");
        movingPlatform = platformObject.AddComponent<MovingPlatform>();
        movingPlatform.pointA = new GameObject("PointA").transform;
        movingPlatform.pointB = new GameObject("PointB").transform;

        movingPlatform.pointA.position = new Vector3(0, 0, 0);
        movingPlatform.pointB.position = new Vector3(5, 0, 0);

        var rb = platformObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        platformObject.AddComponent<BoxCollider2D>();
        platformObject.AddComponent<SpriteRenderer>();

        playerObject = new GameObject("Player");
        var playerRb = playerObject.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 0;
        playerObject.AddComponent<BoxCollider2D>();
    }

    [UnityTest]
    public IEnumerator PlatformMovesBetweenPoints()
    {
        platformObject.transform.position = movingPlatform.pointA.position;

        yield return new WaitForSeconds(2f);

        Assert.AreEqual(movingPlatform.pointB.position, movingPlatform.targetPosition);

        platformObject.transform.position = movingPlatform.pointB.position;
        movingPlatform.Update();

        yield return new WaitForSeconds(2f);

        Assert.AreEqual(movingPlatform.pointA.position, movingPlatform.targetPosition);
    }

    [UnityTest]
    public IEnumerator PlayerParentedToPlatformOnCollision()
    {
        playerObject.transform.position = platformObject.transform.position;

        yield return null;
        playerObject.transform.SetParent(platformObject.transform);

        Assert.AreEqual(playerObject.transform.parent, platformObject.transform);

        playerObject.transform.SetParent(null);
        Assert.IsNull(playerObject.transform.parent);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(platformObject);
        Object.Destroy(playerObject);
    }
}
