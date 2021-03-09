using UnityEngine;

public class RewardTextManager : MonoBehaviour
{
    public static RewardTextManager instance;
    public GameObject rewardTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void ShowExpText(float expToShow)
    {
        int exp = Mathf.RoundToInt(expToShow);

        GameObject reward = Instantiate(rewardTextPrefab);
        reward.transform.SetParent(transform);
        reward.transform.localPosition = Vector3.zero;
        reward.GetComponent<FloatingText>().SetExpText(exp);
    }
}
