# SOValue

This is based on the work by Ryan Hipple, which he presented in the Unity 2017 conference.
Creating ScriptableObjects for each variable allows them to be referenced by multiple scripts quite easily.

The downside is, that SOs cannot retain any changes to data you provide in in a Build of the program, once you change scene. That is a bit of a pity, since then a variable changed in Scene 1 will be reset to it’s original value once you go to Scene 2. To circumvent this, you can store all Scriptable Object variables in a DontDestroyOnLoad script which is present in the first Scene. This then makes sure the data is retained for the duration of the program. 

