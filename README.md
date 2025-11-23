# Animator Parameter 
#### [README_ZH](https://github.com/Cyi0310/Animator-Parameter/blob/main/README_ZH.md)

`AnimatorParameter` is a custom Unity attribute that automatically lists available parameters from the specified Animator directly in the Inspector. This helps prevent human errors caused by manually typing strings and reduces the chances of compilation or runtime issues due to incorrect parameter names.

## Demo
> ![](https://github.com/user-attachments/assets/cff8a344-2218-4361-ad31-a73f6cea1345)

## Feature
- Automatically lists Animator parameter names
- Selectable in the Inspector without manual input
- Prevents typos and invalid parameter references
- Supports both `Animator Component in the scene` and `AnimatorController Asset file in the project`.

## How to use
- Create an `Animator` or `AnimatorController` field
- Create a string field to store the parameter name
- Apply [AnimatorParameter(nameof(anim))] to the string field

This allows you to select the corresponding parameter from a dropdown in the Inspector.

### Scenario 1: Using the Animator Component in the scene
```csharp
public class UseParameterSimple : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    
    // Apply the AnimatorParameter attribute to the string field
    [SerializeField, AnimatorParameter(nameof(anim))]
    private string animParameter;
}
```

### Scenario 2: Using the AnimatorController Asset file in the project
``` C#
public class UseParameterSimple2 : MonoBehaviour
{
    [SerializeField]
    private AnimatorController controllerAsset;

    // Apply the AnimatorParameter attribute to the string field
    [SerializeField, AnimatorParameter(nameof(controllerAsset))]
    private string controllerAssetParam;
}
```

### Scenario 3: Supports cross-hierarchy inheritance to retrieve the field
``` C#
public class BaseEntity : MonoBehaviour
{
    [SerializeField]
    protected Animator anim;
}

public class ChildEntity : BaseEntity
{
    // The child class can directly reference the parent class's anim field
    [SerializeField, AnimatorParameter(nameof(anim))]
    private string jumpParam;
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.