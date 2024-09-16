#if UNITY_EDITOR
using SOSXR.EnhancedLogger;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;


public class SceneBuildValidation : IProcessSceneWithReport
{
    private static readonly bool RunOnlyOnBuilding = true;

    /// <summary>
    ///     The order in relationship to other IProcessSceneWithReport callbacks. Lower numbers are called first.
    /// </summary>
    public int callbackOrder => 0;


    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if (!BuildPipeline.isBuildingPlayer && RunOnlyOnBuilding)
        {
            return;
        }

        if (!AreAllSceneValidationComponentsValid(scene))
        {
            throw new BuildFailedException("Scene " + scene.name + " has validation errors");
        }
    }


    private static bool AreAllSceneValidationComponentsValid(Scene scene)
    {
        var isValid = true;

        foreach (var gameObject in scene.GetRootGameObjects())
        {
            var validators = gameObject.GetComponentsInChildren<IValidate>();

            if (validators is not {Length: > 0})
            {
                continue;
            }

            foreach (var validator in validators)
            {
                if (validator.IsValid)
                {
                    continue;
                }

                isValid = false;
                Log.Error("SceneBuildValidation", "Detected Validation issue with", validator.GetType().Name);
            }
        }

        return isValid;
    }
}
#endif