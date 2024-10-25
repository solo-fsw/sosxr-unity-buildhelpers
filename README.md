# BuildHelpers

- **By:** Maarten R. Struijk Wilbrink
- **For:** Leiden University SOSXR
- **Fully open source:** Feel free to add to or modify anything you see fit.

## Installation
1. Open the Unity project you want to install this package in.
2. Open the Package Manager window.
3. Click on the `+` button and select `Add package from git URL...`.
4. Paste the URL of this repo into the text field and press `Add`.

## Usage

### Version Management
This package increments the last digit of the SemVer build number (found in Project Settings > Player > Version Number). This feature is useful for tracking builds and debugging. The build number will be incremented with each successful build and reset to 0 on the first build. It also updates the Android Bundle/Gradle version code, used in platforms like ArborXR.

You can use TextMeshPro alongside the `ShowBuildInfo` script to display the build number in the game. Combine this with a [DestroyInProductionBuild script](https://github.com/mrstruijk/BuildHelpers/blob/main/Runtime/DestroyInProductionBuild.cs) to remove the build number from production builds.

The build number is logged to a CSV file stored at `Assets/Resources/build_info.csv`, including timestamps and whether the build is production. It runs automatically at build start and increments the build number after a successful build.

You must manually increment the first and second positions of the SemVer numbers. The package only increments the final ('patch') position:
- **Automatically incremented:**
    - 1.0.0 → 1.0.1
    - 1.0.1 → 1.0.2
    - 1.0.2 → 1.0.3
- **Manually incremented:**
    - 1.0.3 → 1.1.0
    - 1.1.0 → 1.2.0
    - 1.2.0 → 2.0.0

### Validation
To validate any class, add the `IValidate` interface and implement the `OnValidate` method to return a list of validation errors. Errors are displayed in the console during the build process. For example:

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

This setup will show an error if `AnotherGameObject` is not set, preventing the build until resolved. You can mass-add the `IValidate` interface to all MonoBehaviours in your project using the `AddValidateInterfaceToMonoBehaviours` menu item under the SOSXR menu. The `SceneBuildValidation` script performs the validation checks.

### Destroy / Disable in (Production) Build
Use the `DestroyInBuild` script to destroy GameObjects or the `DisableInBuild` script to disable them during production builds. The `DisableInProductionBuild` script keeps GameObjects enabled in the editor and development builds but disables them in production.

--- 

Feel free to modify any part of this combined version as needed!