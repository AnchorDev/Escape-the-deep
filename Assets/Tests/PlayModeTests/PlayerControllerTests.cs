using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    private GameObject playerObject;
    private PlayerController playerController;
    private GatherInput gatherInput;

    [SetUp]
    public void SetUp()
    {
        playerObject = new GameObject("Player");
        playerController = playerObject.AddComponent<PlayerController>();
        gatherInput = playerObject.AddComponent<GatherInput>();

        playerObject.AddComponent<Rigidbody2D>();
        playerObject.AddComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = playerObject.AddComponent<SpriteRenderer>();

        playerController.Start();
    }

    [Test]
    public void PlayerJumpsWhenGroundedAndInputGiven()
    {
        playerController.grounded = true;

        gatherInput.jumpInput = true;

        playerController.PlayerJump();

        Assert.Greater(playerController.jumpForce, 0.0f);
    }


    [Test]
    public void PlayerCannotDoubleJump()
    {
        playerController.grounded = true;

        gatherInput.jumpInput = true;
        playerController.PlayerJump();

        playerController.grounded = false;

        gatherInput.jumpInput = true;
        playerController.PlayerJump();

        Assert.AreEqual(playerObject.transform.position.y, playerController.jumpStartHeight);
    }

    [Test]
    public void PlayerChangesAnimationWhenChargingJump()
    {
        playerController.grounded = true;

        gatherInput.jumpInput = true;

        Assert.AreEqual(playerController.spriteRenderer.sprite, playerController.chargingSprite);
    }

    [Test]
    public void PlayerChangesAnimationToIdleAfterJump()
    {
        playerController.grounded = true;

        gatherInput.jumpInput = true;
        playerController.PlayerJump();

        playerController.grounded = false;

        gatherInput.jumpInput = false;
        playerController.ResetJump();

        Assert.AreEqual(playerController.spriteRenderer.sprite, playerController.idleSprite);
    }

    [Test]
    public void PlayerDoesNotMoveOnSlipperySurface()
    {
        playerController.grounded = true;

        playerController.isOnSlipperySurface = true;

        gatherInput.valueX = 1;

        playerController.PlayerMove();

        Assert.AreEqual(playerController.rb.velocity.x, 0);
    }

    [Test]
    public void PlayerResetsJumpWhenNotGrounded()
    {
        playerController.grounded = true;

        gatherInput.jumpInput = true;
        playerController.PlayerJump();

        playerController.grounded = false;

        Assert.Greater(playerController.jumpForce, 0.0f);

        playerController.ResetJump();

        Assert.AreEqual(playerController.jumpForce, 0.0f);
    }

    [Test]
    public void PlayerMovesLeftOrRightWhenInputGiven()
    {
        playerController.grounded = true;

        gatherInput.valueX = 1;
        playerController.PlayerMove();

        Assert.AreEqual(playerController.rb.velocity.x, playerController.speed);

        gatherInput.valueX = -1;
        playerController.PlayerMove();

        Assert.AreEqual(playerController.rb.velocity.x, -playerController.speed);

        gatherInput.valueX = 0;
        playerController.PlayerMove();

        Assert.AreEqual(playerController.rb.velocity.x, 0);
    }


    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerObject);
    }
}
