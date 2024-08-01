using NUnit.Framework;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using Pendu.LoginPage;
using Pendu.GameStart;

public class LoginManagerTests
{
    private string testUsername = "TestUser123";
    private bool isLoginSuccess = false;
    private bool isLoginFailure = false;

    [UnityTest]
    public IEnumerator LoginWithCustomID_ShouldSucceed()
    {
        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 0);
        GameObject loginManagerGameObject = GameObject.Find("LoginManager");
        LoginManager loginManager = loginManagerGameObject.GetComponent<LoginManager>();
        loginManager.LoginWithCustomID(testUsername, 
            onSuccess: () => { isLoginSuccess = true; },
            onError: (error) => { isLoginFailure = true; });

        // Wait until login attempt completes
        yield return new WaitUntil(() => isLoginSuccess || isLoginFailure);

        Assert.IsTrue(isLoginSuccess, "Login should succeed");
        Assert.IsFalse(isLoginFailure, "Login should not fail");
    }

    [UnityTest]
    public IEnumerator OnPlayButtonClicked_ShouldLoadScene()
    {
        SceneManager.LoadScene(0);
        // Set PlayerPrefs login status to simulate a logged-in user
        PlayerPrefs.SetInt("IsLoggedIn", 1);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 0);
        GameObject sceneControllerGameObject = GameObject.Find("LoginPage");
        SceneController sceneController = sceneControllerGameObject.GetComponent<SceneController>();

        // Simulate clicking the play button
        sceneController.OnPlayButtonClicked();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);
        Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex, "Scene 1 should be loaded");

        // Clean up PlayerPrefs
        PlayerPrefs.DeleteKey("IsLoggedIn");
    }
    
    // [UnityTest]
    // public IEnumerator OnButtonClick_ShouldMatchCorrectLettersAndUpdateWordLabel()
    // {
    //     SceneManager.LoadScene(1);
    //     yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

    //     GameObject go = new GameObject("LoginManager");
    //     LoginManager loginManager = go.AddComponent<LoginManager>();
    //     yield return null;

    //     GameObject gameObjectGameStart = GameObject.Find("GameStart");

    //     KeyboardGenerator keyboardGenerator = gameObjectGameStart.GetComponent<KeyboardGenerator>();
    //     WordMatcher wordMatcherTest = gameObjectGameStart.GetComponent<WordMatcher>();

    //     Assert.IsTrue(wordMatcherTest.OnButtonClick('w'), "The letter 'w' should be matched");
    //     Assert.IsTrue(wordMatcherTest.OnButtonClick('o'), "The letter 'o' should be matched");
    //     Assert.IsTrue(wordMatcherTest.OnButtonClick('r'), "The letter 'r' should be matched");
    //     Assert.IsTrue(wordMatcherTest.OnButtonClick('d'), "The letter 'd' should be matched");

    //     wordMatcherTest.UpdateWordLabel();

    //     yield return null;

    //     var uiDocument = gameObjectGameStart.GetComponent<UIDocument>();
    //     Assert.IsNotNull(uiDocument, "UIDocument should be present on the GameStart GameObject.");

    //     Label wordLabel = uiDocument.rootVisualElement.Q<Label>("WordLabel");
    //     Assert.IsNotNull(wordLabel, "WordLabel should be present in the UI document.");
    // }
}