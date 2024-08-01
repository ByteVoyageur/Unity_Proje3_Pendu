using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Pendu.LoginPage;
using Pendu.GameStart;
using System.Collections;

public class GameStartTest
{
    [UnityTest]
    public IEnumerator ShouldInitializeLoginManagerCorrectlyInScene1()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

        GameObject go = new GameObject("LoginManager");
        LoginManager loginManager = go.AddComponent<LoginManager>();

        yield return null;

        Assert.IsNotNull(LoginManager.instance, "LoginManager.instance should not be null after creation");

        Assert.IsNotNull(LoginManager.instance, "LoginManager.instance should be properly set after retries");

        yield return null;
    }

    [UnityTest]
        public IEnumerator OnButtonClick_ShouldMatchCorrectLettersAndUpdateWordLabel()
    {
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

        GameObject go = new GameObject("LoginManager");
        LoginManager loginManager = go.AddComponent<LoginManager>();
        
        yield return null;

        Assert.IsNotNull(LoginManager.instance, "LoginManager.instance should not be null after creation");

        GameObject gameObjectGameStart = GameObject.Find("GameStart");

        KeyboardGenerator keyboardGenerator = gameObjectGameStart.GetComponent<KeyboardGenerator>();
        WordMatcher wordMatcherTest = gameObjectGameStart.GetComponent<WordMatcher>();

        Assert.IsTrue(wordMatcherTest.OnButtonClick('w'), "The letter 'w' should be matched");
        Assert.IsTrue(wordMatcherTest.OnButtonClick('o'), "The letter 'o' should be matched");
        Assert.IsTrue(wordMatcherTest.OnButtonClick('r'), "The letter 'r' should be matched");
        Assert.IsTrue(wordMatcherTest.OnButtonClick('d'), "The letter 'd' should be matched");

        wordMatcherTest.UpdateWordLabel();

        yield return null;

        var uiDocument = gameObjectGameStart.GetComponent<UIDocument>();
        Assert.IsNotNull(uiDocument, "UIDocument should be present on the GameStart GameObject.");

        Label wordLabel = uiDocument.rootVisualElement.Q<Label>("WordLabel");
        Assert.IsNotNull(wordLabel, "WordLabel should be present in the UI document.");
    }
}