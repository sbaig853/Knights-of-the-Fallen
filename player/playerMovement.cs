using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private bool isFacingRight = true;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float slopeDownAngleOld;
    private float horizontal;
    private bool isOnSlope;
    private bool isJumping;
    private bool isFalling;
    public bool isRolling;
    private CapsuleCollider2D cc;
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private PhysicsMaterial2D noFriction;
    private bool grounded = false;
    private bool wasGrounded = false;   // check if play was grounded last frame
    [SerializeField] float slopeCheckDistance;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speed;
    [SerializeField] public Transform weapon;
    public Text interactUIHeart;
    private float rollTimerDuration = 1.10f;
    private float rollTimer = 0f;
     private int enemyhealth = 10; 


    [SerializeField] private AudioClip runningSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip landSoundClip;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip attackSoundClip;
    private bool isGameActive = true;
    private bool isPlayingRunSound = false;
    public Text interactUISword;
    public Text interactUIAxe;
    public InventoryUI InventoryUI;
 

    // Reference to the Inventory component
    private Inventory inventory;

    public CoinManager coinManager;

    // Weapon-related fields
    // public Transform weaponHolder; // The position where the weapon will be attached (hand)
    private GameObject currentWeapon; // The current equipped weapon
    private int currentAttackPower; // The current attack power based on equipped weapon
    private PlayerStats playerStats;
    public ItemData iteminfo;
    public float pickupRadius = 2f;
    private GameObject nearbyItem = null;

    private void Awake()
    {
        // References for rigidbody and animator from the object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = FindObjectOfType<Inventory>(); // Get the Inventory component in the scene
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
   
    }
    private void Start()
    {
        grounded = IsGrounded();
        wasGrounded = grounded;
        // Find the PlayerStats component attached to the Player GameObject
        playerStats = GetComponent<PlayerStats>();
        if (SaveInventory.selectedItemData != null)
        {
            Debug.Log("Run Incremented");
            EquipWeapon(SaveInventory.selectedItemData.itemPrefab, SaveInventory.selectedItemData.attackBuffAmount, SaveInventory.selectedItemData);
            // Clear the persistent data after equipping the weapon
            InventoryUI.UpdateWeaponUI(SaveInventory.selectedItemData);
            SaveInventory.selectedItemData = null;
        }
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not attached to the player GameObject!");
        }
    }

    private void Update() 
    {
    
        playerInput();
    }

    private void FixedUpdate()
    {
        SlopeCheck();
        HandleRunningSound();
    }

    private void playerInput()
    {


        if (!PauseMenu.GameActive)
        {
            return;
        }
        
            grounded = IsGrounded();

            if (!wasGrounded && grounded && !isJumping)
            {
                Sound.instance.PlaySoundFXClip(landSoundClip, transform, 1f);
            }
        wasGrounded = grounded;
        


        horizontal = Input.GetAxis("Horizontal");
        if (nearbyItem != null)
        {
            ItemData itemData = nearbyItem.GetComponent<Item>().itemData;

            if (itemData != null)
            {
                // Check item type and display corresponding UI
                if (itemData.type == ItemData.Type.Consumable && itemData.itemName == "Heart")
                {
                    interactUIHeart.gameObject.SetActive(true); // Show "Press F to Pickup" UI for Heart
                }
                else if (itemData.type == ItemData.Type.Sword)
                {
                    interactUISword.gameObject.SetActive(true); // Show "Press F to Pickup" UI for Sword
                }
                else if (itemData.type == ItemData.Type.Axe)
                {
                    interactUIAxe.gameObject.SetActive(true); // Show "Press F to Pickup" UI for Axe
                }
            }

            if (Input.GetKeyDown(KeyCode.F)) // Pickup when F is pressed
            {
                PickupItem();
            }
        }
        else
        {
            // Hide all UI if not near any item
            interactUIHeart.gameObject.SetActive(false);
            interactUISword.gameObject.SetActive(false);
            interactUIAxe.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            enemyController enemy = FindObjectOfType<enemyController>();
            enemyhealth = enemy.health;
          
   

            Debug.Log("K pressed");

            if (playerStats.hasWeapon)
            {
                anim.SetTrigger("hasWeapon");
                if(enemyhealth >=10)
                {
                    Sound.instance.PlaySoundFXClip(attackSoundClip, transform, 1f);

                }
            }
            else if (playerStats.hasAxe)
            {
                anim.SetTrigger("hasAxe");
                Sound.instance.PlaySoundFXClip(attackSoundClip, transform, 1f);

            }
            else
            {
                anim.SetTrigger("attack");
                   if(enemyhealth >=10)
                    {
                        Sound.instance.PlaySoundFXClip(attackSoundClip, transform, 1f);

                    }
            }
            }
        



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            anim.SetTrigger("isRolling");
            isRolling = true;
            rollTimer = 0f;
        }
        if (isRolling)
        {
            rollTimer += Time.deltaTime;

            if (rollTimer > rollTimerDuration)
            {
                isRolling = false;
                rollTimer = 0f;
            }
        }


        // Movement Logic


        if (grounded && isOnSlope)
        {
            // Move along the slope using the slope's perpendicular vector
            body.velocity = new Vector2(horizontal * slopeNormalPerp.x * -speed, slopeNormalPerp.y * -speed);
        }
        else if (grounded)
        {
            body.velocity = new Vector2(horizontal * speed, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(horizontal * speed, body.velocity.y);
        }

        // Update jumping state
        if (body.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.Space) && grounded && !isJumping)
        {
            Jump();
            Sound.instance.PlaySoundFXClip(jumpSoundClip, transform, 1f);
        }

        // Movement animation

        if (grounded && isOnSlope)
        {
            anim.SetBool("isMoving", Mathf.Abs(horizontal) > 0f);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else if (!grounded && !isOnSlope)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isJumping", body.velocity.y > 0);
            anim.SetBool("isFalling", body.velocity.y < 0);
        }
        else
        {
            anim.SetBool("isMoving", Mathf.Abs(horizontal) > 0);
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }


        // Call the flip function when looking in the opposite direction
        Flip(horizontal);


    }

    // Flip the player based on their direction
    private void Flip(float horizontal)
    {
        if ((horizontal < 0 && isFacingRight) || (horizontal > 0 && !isFacingRight))
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }

    private bool IsGrounded()
    {
        
        isJumping = anim.GetBool("isJumping");
        isFalling = anim.GetBool("isFalling");
        if (isJumping || isFalling) return false;

        Vector3 raycastOrigin = transform.position - new Vector3(0, GetComponent<Collider2D>().bounds.extents.y + 0.01f, 0);
        float length = 0.2f;

        Debug.DrawRay(raycastOrigin, Vector2.down * length, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, length);

      

        return hit.collider != null;
    }

    private void HandleRunningSound()
    {
        bool isMoving = Mathf.Abs(horizontal) > 0.1f && grounded;

      
        if (PauseMenu.GameActive)
        {

            if (isMoving && !isPlayingRunSound)
            {
                Sound.instance.PlayLoopingSound(runningSoundClip, transform, 0.5f);
                isPlayingRunSound = true;
            }
            else if (!isMoving && isPlayingRunSound)
            {
                Sound.instance.StopLoopingSound();
                isPlayingRunSound = false;
            }
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce); // Maintain the same speed assigned of the jump as the movement 
        grounded = false;
        isJumping = true;
    }

    private void SlopeCheck()
    {
        Vector2 checkPosition = (Vector2)transform.position - new Vector2(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPosition);
        SlopeCheckVertical(checkPosition);
    }

    private void SlopeCheckHorizontal(Vector2 checkPosition)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPosition, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPosition, -transform.right, slopeCheckDistance, whatIsGround);

        if(slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if(slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else 
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }
    private void SlopeCheckVertical(Vector2 checkPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            // Set `isOnSlope` only if the slope angle is significant
            if (slopeDownAngle > 0 && slopeDownAngle <= 45.0f) // Adjust the angle range as needed
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

            slopeDownAngleOld = slopeDownAngle; // Track previous slope angle
        }
        else
        {
            isOnSlope = false; // Not on a vertical slope
        }

        // Adjust physics material to prevent sliding
        if (isOnSlope && Mathf.Abs(horizontal) < 0.1f)
        {
            body.sharedMaterial = fullFriction; // High friction when stationary
        }
        else
        {
            body.sharedMaterial = noFriction; // Low friction for movement
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            isJumping = false;
        }
    }

    private void DisableItemPhysicsAndCollision(GameObject item)
    {
        // Disable the hover script to stop the hovering effect
        ItemHover itemHoverScript = item.GetComponent<ItemHover>();
        if (itemHoverScript != null)
        {
            itemHoverScript.enabled = false;
        }

        // Disable Rigidbody2D (if present)
        Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.isKinematic = true; // Disable physics interactions
            itemRb.velocity = Vector2.zero; // Ensure no lingering motion
            itemRb.angularVelocity = 0f; // Ensure no spinning
            itemRb.gravityScale = 0f; // Disable gravity to stop any downward motion
        }

        Collider2D itemCollider = item.GetComponent<Collider2D>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false; // Disable collisions
        }

        // Disable the SpriteRenderer to make the item invisible
        SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void PickupItem()
    {
        if (nearbyItem != null)
        {
            Item item = nearbyItem.GetComponent<Item>();
            if (item != null)
            {
                // Debug log to track the item type
                Debug.Log($"Picked up item: {item.itemData.itemName}");

                // Add item to inventory
                inventory.AddItem(item.itemData);

                // Check item type (like health items or weapons) and apply effects
                if (item.itemData.type == ItemData.Type.Consumable && item.itemData.itemName == "Heart")
                {
                    Health healthComponent = GetComponent<Health>();
                    if (healthComponent != null)
                    {
                        healthComponent.AddHealth(item.itemData.healthRestoreAmount);
                    }
                }
                else if (item.itemData.type == ItemData.Type.Sword)
                {
                    Debug.Log("inside if pick sword");
                    EquipWeapon(item.gameObject, item.itemData.attackBuffAmount, item.itemData);
                }
                else if (item.itemData.type == ItemData.Type.Axe)
                {
                    Debug.Log("inside if pickup axe");
                    EquipWeapon(item.gameObject, item.itemData.attackBuffAmount, item.itemData);
                }

                Debug.Log("Item picked up and removing");

                // Disable item physics and remove from scene
                DisableItemPhysicsAndCollision(nearbyItem);

                Destroy(nearbyItem);  // Remove the item from the scene
                nearbyItem = null;    // Reset the reference
            }
        }
    }

    private void ResetRunSoundFlag()
    {
        isPlayingRunSound = false;
    }


    public void EquipWeapon(GameObject weaponPrefab, int attackBuff, ItemData itemInfo)
    {
        inventory.AddItem(itemInfo);

        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon Prefab is null");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is null");
            return;
        }
        if (itemInfo.type.ToString() == "Sword")
        {
            playerStats.hasAxe = false;
            playerStats.hasWeapon = true;
        }
        if (itemInfo.type.ToString() == "Axe")
        {
            playerStats.hasWeapon = false;
            playerStats.hasAxe = true;
        }

        //Destroy(weaponPrefab);
        // Destroy the current weapon if there is one


        // Find the "Weapon" GameObject attached to the player
        Transform weaponTransform = transform.Find("Weapon");

        if (weaponTransform == null)
        {
            Debug.LogError("Weapon GameObject not found!");
            return;
        }

        // Instantiate the new weapon and parent it to the "Weapon" GameObject
        currentWeapon = Instantiate(weaponPrefab);//, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        // Disable the ItemHover script if it's attached to the weapon
        ItemHover itemHoverScript = currentWeapon.GetComponent<ItemHover>();
        if (itemHoverScript != null)
        {
            itemHoverScript.enabled = false; // Disable the hover effect
        }

        // Apply the attack buff from the weapon directly to playerStats
        playerStats.attackDamage = attackBuff;  // Directly update the attack damage in PlayerStats
        Debug.Log(playerStats.attackDamage);

        // Disable the weapon's Rigidbody2D physics interaction
        Rigidbody2D weaponRb = currentWeapon.GetComponent<Rigidbody2D>();
        if (weaponRb != null)
        {
            weaponRb.isKinematic = true; // Make the weapon Kinematic to prevent physics interaction
        }

        Collider2D weaponCollider = currentWeapon.GetComponent<Collider2D>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }

        Destroy(currentWeapon);
        // Debug log to confirm the attack power is updated
        Debug.Log($"Equipped {weaponPrefab.name}. Current attack power: {playerStats.attackDamage}");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset grounded state if the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            Debug.Log("Out of range, hiding UI");

            nearbyItem = null; // Reset when leaving range of item

            // Hide all interactable UI texts
            interactUIHeart.gameObject.SetActive(false);
            interactUISword.gameObject.SetActive(false);
            interactUIAxe.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (anim.GetBool("isMoving") || anim.GetBool("isJumping") || anim.GetBool("isFalling"))
        {
            // Check for item collection (weapon or consumable)
            if (other.CompareTag("Coin"))
            {
                Destroy(other.gameObject);
                coinManager.coinCount++;
                coinManager.UpdateCoinText();
            }

            if (other.CompareTag("Item"))
            {
                Item item = other.GetComponent<Item>();
                nearbyItem = other.gameObject;

                // Show the appropriate UI text based on the item type
                if (item != null && item.itemData != null)
                {
                    switch (item.itemData.type)
                    {
                        case ItemData.Type.Consumable:
                            interactUIHeart.gameObject.SetActive(true);
                            break;
                        case ItemData.Type.Sword:
                            interactUISword.gameObject.SetActive(true);
                            break;
                        case ItemData.Type.Axe:
                            interactUIAxe.gameObject.SetActive(true);
                            break;
                        // case ItemData.Type.Shield:

                        default:
                            break;
                    }
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            nearbyItem = null;

            // Check if the UI elements are still valid before attempting to deactivate them
            if (interactUIHeart != null)
                interactUIHeart.gameObject.SetActive(false);

            if (interactUISword != null)
                interactUISword.gameObject.SetActive(false);

            if (interactUIAxe != null)
                interactUIAxe.gameObject.SetActive(false);
        }
    }
}
