using UnityEngine;
using UnityEditor;
using TMPro;

public class KoreanFontBuilder
{
    [MenuItem("Tools/Build Korean TMP Font")]
    public static void Build()
    {
        var fontPath = "Assets/Fonts/MalgunGothic.ttf";
        var font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
        if (font == null) { Debug.LogError("폰트 없음: " + fontPath); return; }

        // TMP Font Asset Creator 에디터 타입을 통한 폰트 에셋 생성
        var creatorType = System.Type.GetType(
            "TMPro.EditorUtilities.TMP_FontAssetCreatorWindow, Unity.TextMeshPro.Editor");

        if (creatorType != null)
        {
            // Font Asset Creator 창을 열고 폰트 설정
            var window = EditorWindow.GetWindow(creatorType);
            var fontProp = creatorType.GetField("m_SourceFontFile",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fontProp != null) fontProp.SetValue(window, font);
            Debug.Log("Font Asset Creator 창 열림 - 직접 Generate 후 Save 해주세요");
        }
        else
        {
            // Font Asset Creator가 없으면 TMP_FontAsset.CreateFontAsset 사용
            CreateFontAssetFallback(font);
        }
    }

    static void CreateFontAssetFallback(Font font)
    {
        // GlyphRenderMode, AtlasPopulationMode 타입 가져오기
        var glyphRenderModeType = typeof(UnityEngine.TextCore.LowLevel.GlyphRenderMode);
        var atlasPopModeType = System.Type.GetType("TMPro.AtlasPopulationMode, Unity.TextMeshPro");

        var createMethod = typeof(TMP_FontAsset).GetMethod("CreateFontAsset", new System.Type[] {
            typeof(Font), typeof(int), typeof(int),
            glyphRenderModeType, typeof(int), typeof(int),
            atlasPopModeType, typeof(bool)
        });

        // Dynamic 모드 생성 (Atlas Population = Dynamic)
        var renderMode = System.Enum.ToObject(glyphRenderModeType, 4168); // SDFAA
        var populationMode = System.Enum.ToObject(atlasPopModeType, 1);   // Dynamic

        var fontAsset = createMethod.Invoke(null,
            new object[] { font, 90, 9, renderMode, 2048, 2048, populationMode, false })
            as TMP_FontAsset;

        // 저장 경로
        var savePath = "Assets/Fonts/MalgunGothic_TMP.asset";
        AssetDatabase.DeleteAsset(savePath);
        AssetDatabase.CreateAsset(fontAsset, savePath);

        // 서브에셋 저장
        foreach (var tex in fontAsset.atlasTextures)
        {
            if (tex != null)
            {
                tex.name = "MalgunGothic_TMP Atlas";
                AssetDatabase.AddObjectToAsset(tex, savePath);
            }
        }
        if (fontAsset.material != null)
        {
            fontAsset.material.name = "MalgunGothic_TMP Material";
            AssetDatabase.AddObjectToAsset(fontAsset.material, savePath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(savePath);

        // 씬의 TMP 컴포넌트에 적용
        ApplyToScene(savePath);
        Debug.Log("폰트 에셋 생성 완료 (Dynamic). 플레이 시 한글이 자동으로 렌더링됩니다.");
    }

    static void ApplyToScene(string savePath)
    {
        var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(savePath);
        var allSubs = AssetDatabase.LoadAllAssetsAtPath(savePath);
        Material mat = null;
        foreach (var s in allSubs) if (s is Material) { mat = s as Material; break; }

        var all = Object.FindObjectsByType<TMPro.TextMeshProUGUI>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var tmp in all)
        {
            tmp.font = fontAsset;
            if (mat != null) tmp.fontSharedMaterial = mat;
            tmp.enabled = true;
            tmp.gameObject.SetActive(true);
            EditorUtility.SetDirty(tmp.gameObject);
        }

        // TMP Settings 기본 폰트
        var settings = TMP_Settings.instance;
        if (settings != null)
        {
            var field = typeof(TMP_Settings).GetField("m_defaultFontAsset",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null) { field.SetValue(settings, fontAsset); EditorUtility.SetDirty(settings); }
        }
        AssetDatabase.SaveAssets();
    }
}
