# BuildHelpers

- By: Maarten R. Struijk Wilbrink
- For: Leiden University SOSXR
- Fully open source: Feel free to add to, or modify, anything you see fit.

## Installation
1. Open the Unity project you want to install this package in.
2. Open the Package Manager window.
3. Click on the `+` button and select `Add package from git URL...`.
4. Paste the URL of this repo into the text field and press `Add`.


## Usage
### Validation
Add the IValidate interface to any class that you want to validate. Implement the OnValidate method to return a list of validation errors. The errors will be displayed in the console when the game is being built. Example:

```csharp
public class ThisIsAValidationClass : MonoBehaviour, IValidate
{
    public GameObject AnotherGameObject;
    
    public bool IsValid { get; private set; }


    public void OnValidate()
    {
        IsValid = AnotherGameObject != null;
    }
}
```
This will display an error in the console when the game is built if `AnotherGameObject` is not set. The error will not allow you to build it until the issue is resolved. This can be useful for enforcing certain rules in your project, such as requiring certain components to be set, or certain values to be within a certain range.

You can mass add the IValidate to your existing project (__proceed at your own risk__). The `AddValidateInterfaceToMonoBehaviours` has a Menu Item under the SOSXR menu that will add the IValidate interface to all MonoBehaviours in your project. 

The `SceneBuildValidation` is the one that actually does the heavy lifting. By Warped Imagination.

### Destroy / Disable in (production) Build
Add these scripts to any GameObject that you want to be destroyed or disabled in a production build. The `DestroyInBuild` script will destroy the GameObject, while the `DisableInBuild` script will disable it. The `DisableInProductionBuild` script will be useful to disable the GameObject in a production build, but keep it enabled in the editor, _and_ in development builds.



