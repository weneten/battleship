using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    public float force = 10;
    public float forceOffset = 0.1f;
    public float springForce = 20f;
    public float damping = 5f;
    // public InputActionAsset InputActions;
    // private InputAction m_mouseButton;
    // private Vector2 m_mousePosition;

    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;
    float uniformScale = 1f;


    // private void OnEnabled()
    // {
    //     InputActions.FindActionMap("Raycast").Enable();
    // }

    // private void OnDisable()
    // {
    //     InputActions.FindActionMap("Raycast").Disable();

    // }


    // private void Awake()
    // {
    //     m_mouseButton = InputSystem.actions.FindAction("ray");
    // }

    void Start()
    {
        //TODO: check if gameobj has collider...

        //init Mesh for deforming <- runtime copy of mesh
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        // m_mousePosition = m_mouseButton.ReadValue<Vector2>();
        uniformScale = transform.localScale.x;
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }


        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();


    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>(); //get class from hitted Obj
            if (deformer)
            {
                Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
            }
        }
    }



    public void AddDeformingForce(Vector3 point, float force)
    {
        //TODO: add mass to the equation! to calculate how strom the deformation will be
        point = transform.InverseTransformPoint(point);

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }

        Debug.DrawLine(Camera.main.transform.position, point);
    }

    void AddForceToVertex(int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        pointToVertex *= uniformScale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
        displacement *= uniformScale;
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
    }
}
