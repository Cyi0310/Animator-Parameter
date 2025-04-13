# Animator Parameter <!-- omit in toc -->

`AnimatorParameter` 是一個 Unity 自訂屬性，能夠在 Inspector 中自動列出指定 Animator 內部的參數（Parameters）。這能有效避免手動輸入 string 所產生的人為錯誤，減少因打錯參數名稱導致的編譯錯誤或執行期錯誤。

## Demo
> ![](https://private-user-images.githubusercontent.com/62579114/433122992-cff8a344-2218-4361-ad31-a73f6cea1345.gif?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDQ1Mzk1MDcsIm5iZiI6MTc0NDUzOTIwNywicGF0aCI6Ii82MjU3OTExNC80MzMxMjI5OTItY2ZmOGEzNDQtMjIxOC00MzYxLWFkMzEtYTczZjZjZWExMzQ1LmdpZj9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTA0MTMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUwNDEzVDEwMTMyN1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTc3MzExMTI4M2ZlMWUwYjkzNzc2OWIwZjJhNTA4NDRlMWQzM2QwZTU1YzRjOTQyZTBiYjc2YzIwNjEyMTYzYWEmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.0WKlqfMB53Eg3eFrcfEmMn7YEeG-fAcb0sewYXhOPdM)

## Feature
- 自動列出 Animator 參數名稱
- 支援編輯器內選擇，無須手動輸入
- 預防拼字錯誤與無效參數

## 使用方式
- 建立一個 Animator 欄位
- 建立一個 string 欄位來儲存參數名稱
- 並在 string 欄位上加上 `[AnimatorParameter(nameof(anim))]` 屬性

如此一來，就可以在 Inspector 中以下拉選單方式選擇對應 Animator 的參數。

``` C#
public class UseParameterSimple : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    // 將 AnimatorParameter 屬性套用到 string 欄位
    [SerializeField, AnimatorParameter(nameof(anim))]
    private string parameter;
}
```