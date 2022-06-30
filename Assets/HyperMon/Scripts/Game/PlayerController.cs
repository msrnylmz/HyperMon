using UnityEngine;
using TMPro;

public enum PlayerAnimatorParameters
{
    IsRunning,
    IsAttacking
}

public class PlayerController : MonoBehaviour
{
    #region Public Field
    public GameManager GameManager;
    public bool IsMovement;
    public bool FinishWay;
    public bool PlayerAttackAnimationFinish;
    #endregion

    #region Private Field
    [Space]

    [Range(0, 1000)]
    [SerializeField] private float m_HorizontalSpeed;
    [Range(0, 1000)]
    [SerializeField] private float m_RotationSpeed;
    [Range(0, 1000)]
    [SerializeField] private float m_ForwardSpeed;

    [SerializeField] private Animator m_Animator;

    [SerializeField] private Transform m_MaxLimit;
    [SerializeField] private Transform m_MinLimit;
    [SerializeField] private Transform m_PokeballPoint;
    [SerializeField] private Transform m_PokeballPanel;

    [SerializeField] private TextMeshPro m_PokeballCountText;
    [SerializeField] private Transform PlayerAttackTransform;

    private Touch m_Touch;
    private Vector3 m_MousePosition;
    private Vector3 m_Position;
    private int m_OldPokeballCount;
    private int m_PokeballCount;
    private float m_ClampX;
    private float m_Direct;
    private float m_DisplayPokeballCount;
    private float m_DisplayAnimTimer;
    private bool InsufficientPokeballControl;
    private Vector3 InsufficientPokeballTargetPosition;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Initialize();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "GreenPokeball":
                PokeballOnTriggerEnter(+100, other.gameObject);
                break;
            case "BlackPokeball":
                PokeballOnTriggerEnter(-50, other.gameObject);
                break;
            case "Card":
                CardOnTriggerEnter(other.gameObject);
                break;

            case "Finish":
                FinishOnTriggerEnter();
                break;
        }
    }
    #endregion

    #region Private Methods
    private void Initialize()
    {
        m_ClampX = transform.position.x;
        IsMovement = true;

    }

    private void FinishOnTriggerEnter()
    {
        SetPlayerAnimation(PlayerAnimatorParameters.IsRunning, false);
        FinishWay = true;
        transform.position = PlayerAttackTransform.position;
        GameManager.CameraMovement.CameraFieldOfViewReset();
        GameManager.MonsterController.MonstersParent.gameObject.SetActive(false);
        m_PokeballPanel.gameObject.SetActive(false);
        GameManager.UIManager.ChangePanel(Panels.Attack);
        GameManager.AttackController.WayFinishedInitialize();
    }
    private void CardOnTriggerEnter(GameObject other)
    {
        MonsterCard selectedMonsterCard = other.GetComponent<MonsterCard>();
        if (selectedMonsterCard.MonsterCardObject.Price <= m_PokeballCount)
        {
            GameManager.InsufficientCountReset();
            m_OldPokeballCount = m_PokeballCount;
            m_PokeballCount -= selectedMonsterCard.MonsterCardObject.Price;
            m_DisplayAnimTimer = 0;

            selectedMonsterCard.DuoMonsterCardDestroy();
            GameManager.MonsterController.AddPlayerMonsterCard(selectedMonsterCard);
        }
        else
        {
            InsufficientPokeballControl = true;
            InsufficientPokeballTargetPosition = transform.position - new Vector3(0, 0, 8);
        }

    }
    private void PokeballOnTriggerEnter(int increaseValue, GameObject otherGameobject)
    {
        m_OldPokeballCount = m_PokeballCount;
        m_PokeballCount += increaseValue;
        Destroy(otherGameobject);
        m_DisplayAnimTimer = 0;
    }

    private void InsufficientPokeball()
    {
        if (InsufficientPokeballControl)
        {
            IsMovement = false;
            transform.position = Vector3.MoveTowards(transform.position, InsufficientPokeballTargetPosition, m_ForwardSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, InsufficientPokeballTargetPosition) < 0.01f)
            {
                IsMovement = true;
                InsufficientPokeballControl = false;

                GameManager.IncraseInsufficientCount();
                GameManager.InsufficientLevelFailedControl();
            }
        }
    }
    #endregion

    #region Public Methods
    public void Controller()
    {
        if (!FinishWay)
        {
            if (IsMovement)
            {
                PlayerMovement();
            }
            SetPlayerRotation();
            SetPositionScorePanel();
            DisplayPokeballCountCalculate();
            InsufficientPokeball();
        }
    }

    public void SetPlayerAnimation(PlayerAnimatorParameters selectedParameters, bool control)
    {
        m_Animator.SetBool(selectedParameters.ToString(), control);
    }

    public void SetBoolPlayerAttackAnimationFinish()
    {
        PlayerAttackAnimationFinish = true;
        SetPlayerAnimation(PlayerAnimatorParameters.IsAttacking, false);
    }
    #endregion


    #region Private Methods

    private void DisplayPokeballCountCalculate()
    {
        if (m_DisplayPokeballCount != m_PokeballCount)
        {
            m_DisplayAnimTimer += Time.deltaTime;
            float t = m_DisplayAnimTimer / 0.25f;
            m_DisplayPokeballCount = Mathf.Lerp(m_OldPokeballCount, m_PokeballCount, t);
            m_PokeballCountText.text = ((int)m_DisplayPokeballCount).ToString();
        }
    }

    private void SetRotate(float y, float speed)
    {
        if (!Util.Approximately(transform.rotation, Quaternion.Euler(0, y, 0), Mathf.Epsilon))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, y, 0), speed * Time.deltaTime);
        }
    }

    private void PlayerMovement()
    {
        // Mobile
        if (Input.touchCount > 0)
        {
            m_Touch = Input.GetTouch(0);
            if (m_Touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPos = m_Touch.deltaPosition;
                m_Direct = touchPos.x == 0 ? 0 : touchPos.x / Mathf.Abs(touchPos.x);

                float horizontalSpeed = m_HorizontalSpeed / 100 * Time.deltaTime;
                m_Position = transform.position + new Vector3(touchPos.x, 0, 0) * horizontalSpeed;
                m_ClampX = Mathf.Clamp(m_Position.x, m_MinLimit.position.x, m_MaxLimit.position.x);
            }
            else
                m_Direct = 0;
        }
        // PC
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 normalized = Input.mousePosition - m_MousePosition;
            if (normalized.x != 0)
            {
                m_Direct = normalized.x == 0 ? 0 : normalized.x / Mathf.Abs(normalized.x);
                float horizontalSpeed = m_HorizontalSpeed / 10 * Time.deltaTime;
                m_Position = transform.position + new Vector3(m_Direct, 0, 0) * horizontalSpeed;
                m_ClampX = Mathf.Clamp(m_Position.x, m_MinLimit.position.x, m_MaxLimit.position.x);
            }
            else
                m_Direct = 0;

            m_MousePosition = Input.mousePosition;
        }
        else
        {
            m_Direct = 0;
        }

        float forwardSpeed = m_ForwardSpeed / 10 * Time.deltaTime;
        Vector3 pos = new Vector3(m_ClampX, transform.position.y, transform.position.z + forwardSpeed);
        transform.position = pos;
    }
    private void SetPlayerRotation()
    {
        switch (m_Direct)
        {
            case 0:
                SetRotate(0, m_RotationSpeed / 5);
                break;

            case 1:
                SetRotate(30, m_RotationSpeed);
                break;

            case -1:
                SetRotate(-30, m_RotationSpeed);
                break;
        }
    }

    private void SetPositionScorePanel()
    {
        m_PokeballPanel.position = m_PokeballPoint.position;
    }
    #endregion
}
