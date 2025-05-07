using UnityEngine;
using System.Collections;

public class PostProcessingRandomizer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ChangeProfileEveryFewSeconds());
    }

    private IEnumerator ChangeProfileEveryFewSeconds()
    {
        while (true)
        {
            var dataList = PPController.Instance.postProcessingDataList;

            if (dataList != null && dataList.Count > 0)
            {
                int randomIndex = Random.Range(0, dataList.Count);
                var randomStyle = dataList[randomIndex].style;

                PPController.Instance.SetPostProcessingStyle(randomStyle);

            }

            yield return new WaitForSeconds(3f);
        }
    }
}
