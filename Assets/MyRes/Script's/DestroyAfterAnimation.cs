using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    private void Start() { Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); }
}