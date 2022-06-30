using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Public Field
    public GameManager GameManager;

    #endregion


    #region Private Field

    private Transform m_Target;
    private Vector3 m_RefPosition;

    [SerializeField] private Camera m_Camera;

    [Range(-50, 100)]
    [SerializeField] private float m_OffsetZ;
    [Range(-50, 100)]
    [SerializeField] private float m_OffsetY;
    [Range(0, 100)]
    [SerializeField] private float m_MovementSmoothness;

    [SerializeField] private Transform m_AttackCameraPosition;

    private bool m_AttackCameraPositionReached;
    #endregion


    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        if (!GameManager.PlayerController.FinishWay)
            SetWayMovementCameraPosition();
        else if (!m_AttackCameraPositionReached)
            SetAttackCameraPosition();
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        m_Target = GameManager.PlayerController.transform;
    }
    private void SetWayMovementCameraPosition()
    {
        Vector3 desiredPosition;
        Vector3 smoothedPosition;
        desiredPosition = m_Target.position + new Vector3(0, m_OffsetY, m_OffsetZ);
        smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_RefPosition, m_MovementSmoothness);
        transform.position = new Vector3(smoothedPosition.x, desiredPosition.y, desiredPosition.z);
    }

    private void SetAttackCameraPosition()
    {
        if (Vector3.Distance(transform.position, m_AttackCameraPosition.position) > 0.01f || !Util.Approximately(transform.rotation, m_AttackCameraPosition.rotation, Mathf.Epsilon))
        {
            transform.position = Vector3.Lerp(transform.position, m_AttackCameraPosition.position, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_AttackCameraPosition.rotation, 5 * Time.deltaTime);



        }
        else
            m_AttackCameraPositionReached = true;
    }

    float GetDistance()
    {
        Bounds bounds = new Bounds(GameManager.PlayerController.transform.position, Vector3.zero);
        for (int i = 0; i < GameManager.MonsterController.PlayerMonsterCards.Count; i++)
        {
            bounds.Encapsulate(GameManager.MonsterController.PlayerMonsterCards[i].Monster.transform.position);
        }
        return bounds.size.x;
    }
    #endregion

    #region Public Methods

    public void SetZoom()
    {
        float newZoom = Mathf.Lerp(60, 85, GetDistance() / 2);
        m_Camera.fieldOfView = newZoom;
    }

    public void CameraFieldOfViewReset()
    {
        m_Camera.fieldOfView = 60;
    }

    #endregion
}
