using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraSwitcherTests
{
    private GameObject cameraObject;
    private CameraSwitch cameraSwitcher;
    private GameObject player;

    [SetUp]
    public void SetUp()
    {
        cameraObject = new GameObject("MainCamera");
        Camera cam = cameraObject.GetComponent<Camera>();
        if (cam == null) cam = cameraObject.AddComponent<Camera>();

        GameObject switcherObject = new GameObject("CameraSwitch");
        cameraSwitcher = switcherObject.AddComponent<CameraSwitch>();

        GameObject cameraPositionUp = new GameObject("CameraPositionUp");
        cameraSwitcher.nextCameraPositionUp = cameraPositionUp.transform;

        player = new GameObject("Player");

        BoxCollider2D trigger = switcherObject.AddComponent<BoxCollider2D>();
        trigger.isTrigger = true;
        player.AddComponent<BoxCollider2D>();
        player.tag = "PlayerCameraTrigger";
    }

    [Test]
    public void CameraSwitcher_ChangesCameraPosition_WhenPlayerEntersTrigger()
    {
        cameraObject.transform.position = Vector3.zero;
        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);

        cameraSwitcher.OnTriggerEnter2D(player.GetComponent<BoxCollider2D>());

        Assert.AreEqual(cameraSwitcher.nextCameraPositionUp.position, cameraObject.transform.position);
    }

    [Test]
    public void Camera_PositionChanges_WhenSetToNewPosition()
    {
        cameraObject.transform.position = Vector3.zero;
        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);

        cameraObject.transform.position = new Vector3(5f, 10f, -5f);

        Assert.AreEqual(new Vector3(5f, 10f, -5f), cameraObject.transform.position);
    }

    [Test]
    public void CameraSwitcher_ResetsCameraPosition_WhenPlayerExitsTrigger()
    {
        cameraObject.transform.position = Vector3.zero;
        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);

        cameraSwitcher.OnTriggerEnter2D(player.GetComponent<BoxCollider2D>());
        Assert.AreEqual(cameraSwitcher.nextCameraPositionUp.position, cameraObject.transform.position);

        cameraSwitcher.OnTriggerExit2D(player.GetComponent<BoxCollider2D>());

        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);
    }

    [Test]
    public void CameraDoesNotChangePosition_WhenPlayerDoesNotEnterTrigger()
    {
        cameraObject.transform.position = Vector3.zero;
        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);

        Assert.AreEqual(Vector3.zero, cameraObject.transform.position);
    }
}
