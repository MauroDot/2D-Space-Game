using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldLimit : MonoBehaviour
{
    [SerializeField]
    private int _resistance = 3;
    private int _maxResistance;
    private SpriteRenderer _shieldRenderer;
    private Color _normal, _damage1, _damage2;

    // Start is called before the first frame update
    void Start()
    {
        _maxResistance = _resistance;
        _shieldRenderer = GetComponent<SpriteRenderer>();
        _normal = _shieldRenderer.color;
        _damage1 = new Color32(255, 191, 255, 195);
        _damage1 = new Color32(234, 48, 123, 195);

        if(_shieldRenderer == null)
        {
            Debug.LogError("Shield renderer is NULL");
        }
    }
    
    void SetShieldColor()
    {
        switch(_resistance)
        {
            case 1:
                _shieldRenderer.color = _damage2;
                break;
            case 2:
                _shieldRenderer.color = _damage1;
                break;
                default: _shieldRenderer.color = _normal;
                break;
        }
    }

    public bool DamageShield()
    {
        _resistance--;
        SetShieldColor();

        switch (_resistance)
        { 
            case 1: case 2:
                return true;
            default:
                _resistance = _maxResistance;
                return false;
        }
    }

    public void RestoreShield()
    {
        _resistance = _maxResistance;
        SetShieldColor();
    }
}
