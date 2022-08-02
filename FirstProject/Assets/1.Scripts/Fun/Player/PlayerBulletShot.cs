using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBulletShot : MonoBehaviour
{
    public static PlayerBulletShot Instance;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletSpwanPoint;
    //public List<GameObject> blocks = new List<GameObject>();
    public Queue<GameObject> blocksQueue;
    public int maxTargetBlock;


    [Header("Stack")]
    [SerializeField]
    private float _stack;

    [SerializeField]
    private float _stack_Max;
    public float stack_Max
    {
        get
        {
            return _stack_Max;
        }
        set
        {
            _stack_Max = value;
            _stackSlider.maxValue = _stack_Max;
        }
    }

    [SerializeField]
    private int _stackNesting;
    [SerializeField]
    private int _stackNesting_Max;
    [SerializeField]
    [ColorUsage(true)]
    private Color[] stackColors;
    [SerializeField]
    private Slider _stackSlider;

    [SerializeField]
    private float finalTime;
    private bool final;

    public float stack
    {
        get
        {
            return _stack;
        }
        set
        {
            _stack = value < 0 ? 0 : value;

            _stackSlider.value = _stack;


        }
    }

    private void Awake()
    {
        Instance = this;
        stack_Max = _stack_Max;
    }

    private void Start()
    {
        blocksQueue = new Queue<GameObject>();
        StartCoroutine(BulletShot());
    }

    private void Update()
    {
        StackUpdata();
    }

    public void TargetQueue(GameObject target)
    {
        if (!blocksQueue.Contains(target))
        {
            target.GetComponent<Box_Script>().Targeting(true);
            blocksQueue.Enqueue(target);
            if (blocksQueue.Count > maxTargetBlock)
                blocksQueue.Dequeue().GetComponent<Box_Script>().Targeting(false);

        }
    }
    public IEnumerator BulletShot()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //for (int i = 0; i < blocks.Count; i++)
                //{
                //    GameObject shotbullet = Instantiate(bullet, transform.position, Quaternion.identity);
                //    shotbullet.GetComponent<Bullet>().target = blocks[i].transform;
                //    yield return new WaitForSeconds(0.05f);
                //}
                //blocks.Clear();

                while (blocksQueue.Count != 0)
                {
                    GameObject shotbullet = Instantiate(bullet, bulletSpwanPoint.position, bulletSpwanPoint.rotation);
                    shotbullet.transform.Rotate(Vector3.up * Random.Range(-80, 80));
                    shotbullet.GetComponent<Bullet>().target = blocksQueue.Dequeue().transform;
                    yield return new WaitForSeconds(0.1f);
                }


            }
            yield return null;
        }
    }

    void StackUpdata()
    {
        if (final) return;

        stack -= Time.deltaTime * (_stackNesting + 1) * 1.5f;

        if (stack >= stack_Max)
        {
            if (_stackNesting < _stackNesting_Max - 1)
            {
                stack -= stack_Max;
                stack_Max *= 2;
                maxTargetBlock *= 2;
                _stackNesting++;

                SliderColorSet(_stackNesting, _stackNesting + 1);
            }
            else // max stack, max nest
            {
                stack = stack_Max;
                StartCoroutine(FinalShot());
            }
        }
        else if (stack <= 0)
        {
            if (_stackNesting >= 1)
            {
                stack_Max /= 2;
                maxTargetBlock /= 2;
                _stackNesting--;
                stack = stack_Max;

                SliderColorSet(_stackNesting, _stackNesting + 1);
            }
        }
    }

    public IEnumerator FinalShot()
    {
        final = true;
        SliderColorSet(0, _stackNesting_Max + 1);
        _stackSlider.maxValue = finalTime;
        _stackSlider.value = finalTime;
        while (finalTime > 0)
        {
            _stackSlider.value = finalTime;

            if (Input.GetKeyDown(KeyCode.F))
            {
                // Â÷Áö ¹× ¼¦
            }


            finalTime -= Time.deltaTime;
            yield return null;
        }
        final = false;
        _stackNesting = 0;
        stack = 0;
        SliderColorSet(_stackNesting, _stackNesting + 1);
    }

    private void SliderColorSet(int _back, int _fward)
    {
        _stackSlider.transform.GetChild(0).GetComponent<Image>().color = stackColors[_back];
        _stackSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stackColors[_fward];

    }

}
