using System.Collections;
using TMPro;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    private static CurrencySystem Instance;

    [SerializeField] private int balance;
    public static int Balance=>  Instance==null? 0 : Instance.balance;

    [SerializeField] TextMeshProUGUI balanceText;
    [SerializeField] TextMeshProUGUI notEnoughCashText;
    private Vector2 prePos;

    private Coroutine notEnoughCashCoroutine;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if(notEnoughCashText != null)
        {
            prePos = notEnoughCashText.rectTransform.anchoredPosition;
            notEnoughCashText.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        TryAddAmount(500);
    }

    [Button]
    public static bool TryDeductAmount(int amount)
    {
        if(Instance == null)
        {
            Debug.Log("[Currency System] Instance Not Found");
            return false;
        }
        if(amount < 0)
        {
            Debug.Log("[Currency System] Amount must be non Negative");
            return false;
        }
        if(amount > Instance.balance)
        {
            Debug.Log("[Currency System] Not enough balance");
            Instance.Shake();
            return false;
        }

        Instance.balance -= amount;
        UpdateBalanceText();
        return true;
    }


    [Button]
    public static bool TryAddAmount(int amount)
    {
        if (Instance == null)
        {
            Debug.Log("[Currency System] Instance Not Found");
            return false;
        }
        if (amount < 0)
        {
            Debug.Log("[Currency System] Amount must be non Negative");
            return false;
        }
        Instance.balance += amount;
        UpdateBalanceText();
        return true;
    }

    private static void UpdateBalanceText() 
    {
        if (Instance.balanceText != null)
        {
            Instance.balanceText.SetText(Instance.balance.ToString());
        }
    }

    [Button]
    private void Shake()
    {
        if (notEnoughCashText==null)
        {
            Debug.Log("[Currency System] <color = red> notEnoughCashText </color> is not assigned in inspector");
            return;
        }
        if (notEnoughCashCoroutine != null)
        {
            StopCoroutine(notEnoughCashCoroutine);
        }
        notEnoughCashCoroutine = StartCoroutine(ShakeIEnumerator());
    }

    IEnumerator ShakeIEnumerator()
    {
        notEnoughCashText.gameObject.SetActive(true);

        RectTransform rect = notEnoughCashText.rectTransform;
        float elapsed = 0f;
        float duration = 1f;
        float strength = 10f;
        float frequency = 50f;

        while (elapsed < duration)
        {
            if (elapsed <= 0.4f)
            {
                float offset = Mathf.Sin(elapsed * frequency) * strength;
                rect.anchoredPosition = prePos + new Vector2(offset, 0f);
            }
            else
            {
                rect.anchoredPosition = prePos;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = prePos;
        notEnoughCashText.gameObject.SetActive(false);
    }
}
