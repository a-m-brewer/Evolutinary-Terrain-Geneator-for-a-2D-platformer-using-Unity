using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TEST_MapGenDisplay {

	[Test]
	public void TEST_InvokeRepeatingEvolution() {
        // Use the Assert class to test conditions.
        var mapGenDisplay = new GameObject().AddComponent<MapGenDisplay>();
        mapGenDisplay.GetComponent<MapGenDisplay>().InvokeRepeatingEvolution();
        bool test = mapGenDisplay.GetComponent<MapGenDisplay>().IsInvoking();
        mapGenDisplay.GetComponent<MapGenDisplay>().CancelInvokeEvolution();
        Assert.True(test);
    }

    [Test]
    public void TEST_CancelInvokeEvolution()
    {
        // Use the Assert class to test conditions.
        var mapGenDisplay = new GameObject().AddComponent<MapGenDisplay>();
        mapGenDisplay.GetComponent<MapGenDisplay>().InvokeRepeatingEvolution();
        bool invoked = mapGenDisplay.GetComponent<MapGenDisplay>().IsInvoking();
        mapGenDisplay.GetComponent<MapGenDisplay>().CancelInvokeEvolution();
        bool notInvoked = !mapGenDisplay.GetComponent<MapGenDisplay>().IsInvoking();
        Assert.True(invoked && notInvoked);
    }

}
