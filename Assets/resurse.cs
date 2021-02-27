using UnityEngine;
using TMPro;

public class resurse : MonoBehaviour
{
    public TextMeshProUGUI noobomium;
    public TextMeshProUGUI naturalium;
    public TextMeshProUGUI taranium;
    public TextMeshProUGUI weed;
    public int _noobomium = 5670;
    public int _naturalium = 1432;
    public int _taranium = 765;
    public int _weed = 7893;

    void Update()
    {
        noobomium.text = _noobomium.ToString();
        naturalium.text = _naturalium.ToString();
        taranium.text = _taranium.ToString();
        weed.text = _weed.ToString();
    }
}
