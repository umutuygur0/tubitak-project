using UnityEngine;
using UnityEngine.UI;
public class PoliceWalker : MonoBehaviour
{
    [Header("Yürüyüş Ayarları")]
    public float walkSpeed = 2f;
    [SerializeField] private Transform target;
    private Animator animator;
    [Header("Mesaj Ayarları")]
    public Canvas messageCanvas; // Polis objesinin child Canvas'ı
    public Text messageText;     // Text hâlâ var, ama değiştirmek zorunda değilsin
    private bool messageShown = false;
    public void SetTarget(Transform walkTarget)
    {
        target = walkTarget;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        // Başlangıçta Canvas pasif olsun
        if (messageCanvas != null)
            messageCanvas.gameObject.SetActive(false);
    }
    void Update()
    {
        if (target == null)
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
            return;
        }
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
            transform.LookAt(target);
            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
            if (!messageShown)
            {
                messageShown = true;
                ShowMessage();
            }
        }
    }
    void ShowMessage()
    {
        if (messageCanvas != null)
            messageCanvas.gameObject.SetActive(true); // Canvas aktif oluyor
    }
}