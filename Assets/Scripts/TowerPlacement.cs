using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public Tower _placedTower;
    [SerializeField]private bool isRecyle;
    public static bool toDelete;
    public bool isAvailable;

    // Fungsi Singleton
    private static TowerPlacement _instance = null;
    public static TowerPlacement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TowerPlacement>();
            }
            return _instance;
        }
    }

    // Fungsi yang terpanggil sekali ketika ada object Rigidbody yang menyentuh area collider
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (isRecyle)
        {
            Tower towa = collision.GetComponent<Tower>();
            if (towa != null)
            {
                towa.SetPlacePosition(transform.position);
                _placedTower = towa;
                toDelete = true;
            }
            return;
        }
        if (_placedTower != null)
        {
            return;
        }


        Tower tower = collision.GetComponent<Tower> ();
        if (tower != null & !isAvailable)
        {
            tower.SetPlacePosition (transform.position);
            tower.SetTowerPos(this);
            isAvailable = true;
            _placedTower = tower;
        }
    }

    // Kebalikan dari OnTriggerEnter2D, fungsi ini terpanggil sekali ketika object tersebut meninggalkan area collider
    private void OnTriggerExit2D (Collider2D collision)
    {
        if (_placedTower == null)
        {
            return;
        }
        _placedTower.SetPlacePosition(null);
        _placedTower = null;
    }
}