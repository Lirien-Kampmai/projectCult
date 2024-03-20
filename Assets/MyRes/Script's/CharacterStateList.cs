using UnityEngine;

public class CharacterStateList : MonoBehaviour
{
    private bool isJumping = false;
    private bool isDashing = false;

    public bool GetStateIsJumping() { return isJumping; }
    public bool GetStateIsDashing() { return isDashing; }
    public void SetStateIsJumping(bool state) { isJumping = state; }
    public void SetStateIsDashing(bool state) { isDashing = state; }
}
