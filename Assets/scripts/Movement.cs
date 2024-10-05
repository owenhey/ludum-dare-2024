using UnityEngine;

public class Movement : MonoBehaviour {
    public float Speed = 3;
    public float Damping = .1f;
    public CharacterController CC;
    
    public Camera Cam;
    public LayerMask GroundLayerMask;

    
    private Vector3 _vel;
    
    private void Update() {
        bool isDirectingToMove = Input.GetKey(KeyCode.Mouse0);
        if (isDirectingToMove) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, GroundLayerMask)) {
                MoveTowards(hit.point);
            }
        }
    }

    private void MoveTowards(Vector3 hitpoint) {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, hitpoint, ref _vel, Damping, Speed);
        Vector3 torwards = targetPos - transform.position;
        CC.Move(torwards);
    }
}