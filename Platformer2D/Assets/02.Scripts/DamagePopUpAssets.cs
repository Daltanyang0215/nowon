using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpAssets : MonoBehaviour
{
    private static DamagePopUpAssets _instance;
    public static DamagePopUpAssets Instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate( Resources.Load<DamagePopUpAssets>("Assets/DamagePopUpAssets"));
            return _instance;
        }
    }

    [SerializeField] private DamagePopUp _damagePopUp1;
    [SerializeField] private DamagePopUp _damagePopUp2;

    public DamagePopUp GetDagamePopUp( int layer)
    {
        DamagePopUp TmpDamagePopUp = _damagePopUp1;
        if(layer == LayerMask.NameToLayer("Enemy"))
        {
            TmpDamagePopUp = _damagePopUp2;
        }
        return TmpDamagePopUp;
    }
}
