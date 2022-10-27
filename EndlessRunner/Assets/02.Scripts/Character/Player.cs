using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player instance;

    private int _hp;
    public int hp
    {
        get
        {
            return _hp; 
        }
        set
        {
            if (value < 0)
            {
                value = 0;
                _character.ChangeMachineState(PlayerStateTypes.Die);
            }

            _hp = value;

            for (int i = 0; i < _hpMax; i++)
            {
                _hpIcons[i].SetActive(i < (value-1));
            }
        }
    }
    [SerializeField] private int _hpInit =3;
    private int _hpMax;

    [SerializeField] private Transform _hpIconContent;
    private List<GameObject> _hpIcons = new List<GameObject>();
    private CharacterPlayer _character;

    [SerializeField] private LayerMask _itemLayer;

    private void Awake()
    {
        instance= this;

        _character = transform.root.GetComponent<CharacterPlayer>();

        for (int i = 0; i < _hpIconContent.childCount; i++)
        {
            _hpIcons.Add(_hpIconContent.GetChild(i).gameObject);
        }

        _hpMax = _hpIconContent.childCount;
        hp = _hpInit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(1<<other.gameObject.layer == _itemLayer)
        {
            other.GetComponent<Item>().OnEarn();
            Destroy(other.gameObject);
        }
    }
}
