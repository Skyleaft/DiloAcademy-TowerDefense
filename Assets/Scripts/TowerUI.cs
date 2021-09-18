using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _towerIcon;

    private Tower _towerPrefab;
    private Tower _currentSpawnedTower;
    Vector2 currentSize;
    RectTransform rectTransform;



    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTowerPrefab (Tower tower)
    {
        _towerPrefab = tower;
        _towerIcon.sprite = tower.GetTowerHeadIcon ();
    }

    // Implementasi dari Interface IBeginDragHandler
    // Fungsi ini terpanggil sekali ketika pertama men-drag UI
    public void OnBeginDrag (PointerEventData eventData)
    {
        GameObject newTowerObj = Instantiate (_towerPrefab.gameObject);
        _currentSpawnedTower = newTowerObj.GetComponent<Tower> ();
        _currentSpawnedTower.ToggleOrderInLayer (true);
        shrink();
    }

    private void shrink()
    {
        currentSize = rectTransform.sizeDelta;
        this.gameObject.transform.localScale = Vector3.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }

    private void Unshrink()
    {
        this.gameObject.transform.localScale = Vector3.one;
        rectTransform.sizeDelta = currentSize;
    }

    // Implementasi dari Interface IDragHandler
    // Fungsi ini terpanggil selama men-drag UI
    public void OnDrag (PointerEventData eventData)
    {

        Camera mainCamera = Camera.main;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint (mousePosition);

        _currentSpawnedTower.transform.position = targetPosition;
    }

    // Implementasi dari Interface IEndDragHandler
    // Fungsi ini terpanggil sekali ketika men-drop UI ini
    public void OnEndDrag (PointerEventData eventData)
    {
        if (_currentSpawnedTower.PlacePosition == null)
        {
            Destroy (_currentSpawnedTower.gameObject);
            Unshrink();
        }
        else if (TowerPlacement.Instance.toDelete)
        {
            TowerPlacement.Instance.toDelete = false;
            Destroy(_currentSpawnedTower.gameObject);
            LevelManager.Instance.removeUITower(this);

            Destroy(this.gameObject);
        }
        else
        {
            _currentSpawnedTower.LockPlacement ();
            _currentSpawnedTower.ToggleOrderInLayer (false);
            LevelManager.Instance.RegisterSpawnedTower (_currentSpawnedTower);
            LevelManager.Instance.removeUITower(this);
            _currentSpawnedTower = null;

            Destroy(this.gameObject);
        }
    }
}