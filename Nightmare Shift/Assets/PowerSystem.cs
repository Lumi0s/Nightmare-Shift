using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public static PowerSystem Instance { get; private set; }
    public int usage = 0;
    private float power = 100;
    public TextMeshProUGUI powerUI;
    public TextMeshProUGUI usageUI;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        power -= Time.deltaTime * usage * 0.3f;
        power = Mathf.Max(power, 0);

        powerUI.text = "Power: " + power.ToString("N0") + "%";
        usageUI.text = "Usage: " + usage.ToString();
    }
}
