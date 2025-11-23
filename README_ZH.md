# Animator Parameter 
#### [README_EN](https://github.com/Cyi0310/Animator-Parameter/blob/main/README.md)

`AnimatorParameter` 是一個 Unity 自訂屬性，能夠在 Inspector 中自動列出指定 Animator 內部的參數（Parameters）。這能有效避免手動輸入 string 所產生的人為錯誤，減少因打錯參數名稱導致的編譯錯誤或執行期錯誤。

## Demo
> ![](https://github.com/user-attachments/assets/cff8a344-2218-4361-ad31-a73f6cea1345)

## Feature
- 自動列出 Animator 參數名稱
- 支援編輯器內選擇，無須手動輸入
- 預防拼字錯誤與無效參數
- 支援 `場景中的 Animator Component` 以及 `專案中的 AnimatorController Asset 檔案`。

## 使用方式
- 建立一個 `Animator` or `AnimatorController` 欄位
- 建立一個 string 欄位來儲存參數名稱
- 並在 string 欄位上加上 `[AnimatorParameter(nameof(anim))]` 屬性

如此一來，就可以在 Inspector 中以下拉選單方式選擇對應 Animator 的參數。

### 情境一: 能使用場景中的 Animator Component
``` C#
public class UseParameterSimple : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    
    // 將 AnimatorParameter 屬性套用到 string 欄位
    [SerializeField, AnimatorParameter(nameof(anim))]
    private string animParameter;
}
```

### 情境二: 能使用專案中的 AnimatorController Asset 檔案
``` C#
public class UseParameterSimple2 : MonoBehaviour
{
    [SerializeField]
    private AnimatorController controllerAsset;

    // 將 AnimatorParameter 屬性套用到 string 欄位
    [SerializeField, AnimatorParameter(nameof(controllerAsset))]
    private string controllerAssetParam;
}
```

### 情境三: 支援跨階層繼承取得該欄位
``` C#
public class BaseEntity : MonoBehaviour
{
    [SerializeField]
    protected Animator anim;
}

public class ChildEntity : BaseEntity
{
    // 子類別可以直接參考父類別的 anim 欄位
    [SerializeField, AnimatorParameter(nameof(anim))]
    private string jumpParam;
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.