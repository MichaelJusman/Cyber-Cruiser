using UnityEngine;
using TMPro;

public class BossUI : GameBehaviour
{
    [SerializeField] private GameObject _bossWarningUI, _bossHealthBarUI;
    [SerializeField] private TMP_Text _bossNameText, _bossWarningText;
    [SerializeField] private UISlider _bossHealthBar;

    public string BossNameText
    {
        set
        {
            _bossNameText.text = value;
            _bossNameText.enabled = true;
        }
    }

    public string BossWarningText
    {
        set
        {
            _bossWarningText.text = value;
        }
    }

    private void OnEnable()
    {
        EnemySpawnerManager.OnBossSelected += EnableBossWarningUI;
        GameManager.OnMissionStart += DisableBossUI;
        EnemySpawner.OnBossSpawned += (e) => { DisableBossWarningUI(); };
        EnemySpawner.OnBossSpawned += EnableBossUI;
        Boss.OnBossDied += (p, v) => { DisableBossUI(); };
        Boss.OnBossDamage += UpdateBossHealthBar;
    }

    private void OnDisable()
    {
        EnemySpawnerManager.OnBossSelected -= EnableBossWarningUI;
        GameManager.OnMissionStart -= DisableBossUI;
        EnemySpawner.OnBossSpawned -= (e) => { DisableBossWarningUI(); }; 
        Boss.OnBossDamage -= UpdateBossHealthBar;
        EnemySpawner.OnBossSpawned -= EnableBossUI;
        Boss.OnBossDied -= (p, v) => { DisableBossUI(); };
    }

    private void Start()
    {
        DisableBossUI();
    }
    public void EnableBossWarningUI(GameObject boss)
    {
        BossWarningText = "Warning!! " + boss.name + " approaching";
        _bossWarningUI.SetActive(true);
    }

    private void DisableBossWarningUI()
    {
        BossWarningText = "";
        _bossWarningUI.SetActive(false);
    }

    private void EnableBossUI(Enemy boss)
    {
        BossNameText = boss.gameObject.name;
        EnableSlider(_bossHealthBar, boss.MaxHealth);
    }

    private void DisableBossUI()
    {
        _bossHealthBarUI.SetActive(false);
        DisableSlider(_bossHealthBar);
        _bossNameText.enabled = false;
        BossNameText = "";
        DisableBossWarningUI();
    }

    private void UpdateBossHealthBar(float value)
    {
        ChangeSliderValue(_bossHealthBar, value);
    }
}
