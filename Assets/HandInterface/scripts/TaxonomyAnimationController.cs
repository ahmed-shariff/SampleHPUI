using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxonomyAnimationController : MonoBehaviour
{
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
	StartCoroutine(screenProcess());
    }

    IEnumerator screenProcess()
    {
        while(true)
	{
	    if (currentIndex == transform.childCount)
		currentIndex = 0;
	    for(int i=0; i < transform.childCount; i++)
	    {
		if (i == currentIndex)
		    transform.GetChild(i).gameObject.SetActive(true);
		else
		    transform.GetChild(i).gameObject.SetActive(false);
	    }
	    yield return new WaitForSeconds(5);

	    currentIndex++;
	}
    }
}
