using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text blockCountText;
    [SerializeField] private TMP_Text moneyCountText;
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private GameObject moneyImage;
    [SerializeField] private Vector3 moneySpawnPosition = new Vector3(0, 0, 0);
    [SerializeField] private float moneyAnimationTime = 1f;
    [SerializeField] private AnimationCurve moneyAnimationCurve = AnimationCurve.Linear(0, 0, 1 , 1);

    private Animator _moneyAnimator;
    private Animator _blockAnimator;
    private Camera _camera;

    public void BlockCountChanged(int count)
    {
        blockCountText.text = count.ToString();
        _blockAnimator.SetTrigger("Shake");
    }

    public void MoneyCountChanged(int count)
    {
        StartCoroutine(ChangeMoney(count));
    }

    private void Start()
    {
        _camera = Camera.main;

        _moneyAnimator = moneyCountText.GetComponentInParent<Animator>();
        _blockAnimator = blockCountText.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        fpsText.text = ((int) (1f / Time.deltaTime)).ToString();
    }

    private IEnumerator ChangeMoney(int count)
    {
        RectTransform moneyTransform = Instantiate(moneyImage, transform).GetComponent<RectTransform>();
        moneyTransform.transform.SetAsFirstSibling();
        moneyTransform.transform.position = _camera.WorldToScreenPoint(moneySpawnPosition);

        float passedTime = 0;
        Vector2 startPosition = moneyTransform.anchoredPosition;

        while (passedTime < moneyAnimationTime)
        {
            passedTime += Time.deltaTime;
            Vector2 passedPath = (Vector2.zero - startPosition) *
                                 moneyAnimationCurve.Evaluate(passedTime / moneyAnimationTime);
            moneyTransform.anchoredPosition = startPosition + passedPath;

            yield return null;
        }
        
        Destroy(moneyTransform.gameObject);
        moneyCountText.text = count.ToString();
        _moneyAnimator.SetTrigger("Shake");
    }
}
