using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCounter : MonoBehaviour
{
    public int exitTappedTimes = 0;

    // Start is called before the first frame update

    private static ExitCounter exitCounter;

    void Awake()
    {
        if (exitCounter == null)
        {

            exitCounter = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this);
        }
    }

    public void exitTapped()
    {
        exitTappedTimes++;
    }
}
