using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{

    [SerializeField] private Transform playerCamera = null;

    public float detectionRadius = 20f;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    FPSController controller = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FPSController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetThings()
    {
        Collider[] colliders = Physics.OverlapSphere(playerCamera.position, detectionRadius, targetLayer);

        foreach(Collider collider in colliders)
        {
            Transform currentTransform = collider.transform;

            Vector3 targetDirection = (currentTransform.position - playerCamera.position).normalized;

            //float angle = Vector3.Angle(playerCamera.forward, targetDirection);

            //Debug.Log(angle);

            Quaternion q = Quaternion.FromToRotation(playerCamera.forward, targetDirection);

            Debug.Log(q.eulerAngles.x+","+q.eulerAngles.y);

            //not working because of FPSController script
            //playerCamera.transform.localEulerAngles = new Vector3(q.eulerAngles.x, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z);

            //transform.Rotate(transform.up * q.eulerAngles.y);
            controller.PerformAssistedRotation(q.eulerAngles.x, q.eulerAngles.y);
           
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetThings();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCamera.position, detectionRadius);
    }
}
