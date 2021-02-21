using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private const float SpeedMultiplier = 0.1f;

    [SerializeField]
    private float speed;

    private Vector2 direction;

    [SerializeField]
    [Tooltip("Seconds")]
    private float lifetime;

    private Player player;
    private Crosshair crosshair;
    private GameObject target;
    private new LineRenderer renderer;
    private DistanceJoint2D joint;

    public bool Seeking { get; set; } = true;

    public float Speed => speed * SpeedMultiplier;
    public Vector2 Direction => direction;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        crosshair = FindObjectOfType<Crosshair>();

        target = transform.GetChild(0).gameObject;

        renderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        direction = (crosshair.transform.position - player.transform.position).normalized;
        target.transform.SetParent(null);
        StartCoroutine(Lifetimer());
    }

    private void FixedUpdate()
    {
        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        renderer.SetPositions(new []
        {
            player.transform.position, 
            target.transform.position
        });
    }

    public void OnCollision(Collider2D collider)
    {
        Seeking = false;

        joint = player.gameObject.AddComponent<DistanceJoint2D>();
        joint.enableCollision = true;
        
        StartCoroutine(UpdateTargetPosition());
    }

    public void ChangeDistance(float input)
    {
        const float Multiplier = 0.2f;

        if (joint == null) { return; }
        joint.distance -= input * Multiplier;
    }

    private IEnumerator Lifetimer()
    {
        yield return new WaitForSeconds(lifetime);
        if (Seeking) { Destroy(gameObject); }
        yield return null;
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (joint != null && target != null)
        {
            joint.connectedAnchor = target.transform.position;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            Destroy(target);
        }

        if (joint != null)
        {
            Destroy(joint);
        }
    }
}
