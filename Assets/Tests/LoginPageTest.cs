using NUnit.Framework;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using Pendu.LoginPage;

public class LoginManagerTests : MonoBehaviour
{
    private string testUsername = "TestUser123";
    private bool isLoginSuccess = false;
    private bool isLoginFailure = false;

    [UnityTest]
    public IEnumerator LoginWithCustomID_ShouldSucceed()
    {
        LoginManager loginManager = new LoginManager();

        loginManager.LoginWithCustomID(testUsername, 
            onSuccess: () => { isLoginSuccess = true; },
            onError: (error) => { isLoginFailure = true; });

        // Wait until login attempt completes
        yield return new WaitUntil(() => isLoginSuccess || isLoginFailure);

        Assert.IsTrue(isLoginSuccess, "Login should succeed");
        Assert.IsFalse(isLoginFailure, "Login should not fail");
    }

[UnityTest]
    public IEnumerator OnPlayButtonClicked_ShouldLoadSceneAndDisplayUIElement()
    {
        // Set PlayerPrefs login status to simulate a logged-in user
        PlayerPrefs.SetInt("IsLoggedIn", 1);

        // Create a GameObject and add SceneController component
        var sceneControllerGameObject = new GameObject("SceneController");
        var sceneController = sceneControllerGameObject.AddComponent<SceneController>();
        
        // Simulate clicking the play button
        sceneController.OnPlayButtonClicked();

        // Wait for the scene to load
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

        // Validate the scene is loaded correctly
        Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex, "Scene 1 should be loaded");

        // Find UI element in the loaded scene
        var uiDocument = FindObjectOfType<UIDocument>();
        Assert.IsNotNull(uiDocument, "UIDocument should be present in the scene");

        var root = uiDocument.rootVisualElement;
        var userNameLabel = root.Q<Label>("UserName");
        Assert.IsNotNull(userNameLabel, "UserName label should be found in the loaded scene");

        // Clean up PlayerPrefs
        PlayerPrefs.DeleteKey("IsLoggedIn");
    }

    
}