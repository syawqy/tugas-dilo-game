using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float AutoCollectPercentage = 0.1f;
    public ResourceConfig[] ResourcesConfigs;
    public Transform ResourcesParent;
    public ResourceController ResourcePrefab;
    public Text GoldInfo;
    public Text AutoCollectInfo;
    public TapText TapTextPrefab;
    public Transform CoinIcon;

    private List<ResourceController> _activeResources = new List<ResourceController>();
    private List<TapText> _tapTextPool = new List<TapText>();
    private float _collectSecond;
    private static GameManager _instance = null;
    private double _totalGold;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddAllResources();
    }

    // Update is called once per frame
    void Update()
    {
        _collectSecond += Time.unscaledDeltaTime;
        if (_collectSecond >= 1f)
        {
            CollectPerSecond();
            _collectSecond = 0f;
        }

        CoinIcon.transform.localScale = Vector3.LerpUnclamped(CoinIcon.transform.localScale, Vector3.one * 2f, 0.15f);
        CoinIcon.transform.Rotate(0f, 0f, Time.deltaTime * -100f);
    }

    private void AddAllResources()
    {
        foreach (ResourceConfig config in ResourcesConfigs)
        {
            GameObject obj = Instantiate(ResourcePrefab.gameObject, ResourcesParent, false);
            ResourceController resource = obj.GetComponent<ResourceController>();
            resource.SetConfig(config);
            _activeResources.Add(resource);
        }
    }

    private void CollectPerSecond()
    {
        double output = 0;
        foreach (ResourceController resource in _activeResources)
        {
            output += resource.GetOutput();
        }
        output *= AutoCollectPercentage;
        AutoCollectInfo.text = $"Auto Collect: { output.ToString("F1") } / second";
        AddGold(output);
    }

    private void AddGold(double value)
    {
        _totalGold += value;
        GoldInfo.text = $"Gold: { _totalGold.ToString("0") }";
    }

    public void CollectByTap(Vector3 tapPosition, Transform parent)
    {
        double output = 0;
        foreach (ResourceController resource in _activeResources)
        {
            output += resource.GetOutput();
        }

        TapText tapText = GetOrCreateTapText();
        tapText.transform.SetParent(parent, false);
        tapText.transform.position = tapPosition;
        tapText.Text.text = $"+{ output.ToString("0") }";
        tapText.gameObject.SetActive(true);

        CoinIcon.transform.localScale = Vector3.one * 1.75f;
        AddGold(output);
    }

    private TapText GetOrCreateTapText()
    {
        TapText tapText = _tapTextPool.Find(t => !t.gameObject.activeSelf);
        if (tapText == null)
        {
            tapText = Instantiate(TapTextPrefab).GetComponent<TapText>();
            _tapTextPool.Add(tapText);
        }
        return tapText;
    }

    [System.Serializable]
    public struct ResourceConfig
    {
        public string Name;
        public double UnlockCost;
        public double UpgradeCost;
        public double Output;
    }
}
