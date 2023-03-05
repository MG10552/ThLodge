using UnityEngine;
using System.Collections;

public class BattleTextRenderer : MonoBehaviour
{
    Mesh mesh;
    BattleTextFont font;
    BattleTextSentance sentance;

    public float GlyphSpacing = 0.05f;
    public float GlyphSize = 0.5f;
    public bool LockY = true;
    public bool LookAtMainCamera = true;
    public string InitialText = "";
    public TextAsset FontDefinition = null;
    public Material FontMaterial = null;
    public bool Visible = true;
    public bool AnimateInWorld = true;
    public bool IsAnimated = true;
    public float FadeDelay = 0f;
    public Color Color = Color.white;
    public BattleTextFont.DefinitionType FontDefinitionType = BattleTextFont.DefinitionType.BMFont_XML;

    public string Sentance
    {
        get { return sentance.Sentance; }
    }

    public void Init()
    {
        LockY = false;
        LookAtMainCamera = false;
        AnimateInWorld = false;
        IsAnimated = false;

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
    }

    void LateUpdate()
    {
        // Set local scale
        transform.localScale = new Vector3(GlyphSize, GlyphSize, GlyphSize);

        // Set renderer state
        GetComponent<Renderer>().enabled = Visible;

        /*
        if (LookAtMainCamera)
        {
            // Direction from us to look at position
            Vector3 d = BattleTextCamera.Instance.MainCamera.transform.position - transform.position;

            if (LockY)
            {
                // We don't wanna rotate on Y
                d.y = 0;
            }

            // Use the inverse and create a look rotation
            transform.rotation = Quaternion.LookRotation(-d.normalized);
        }
        */
    }

    public void SetText(string text)
    {
        SetText(ActionBarSettings.Instance.FontDefinition, ActionBarSettings.Instance.FontMaterial, text);
    }

    void SetText(TextAsset fontDefinition, Material fontMaterial, string text)
    {
        if (fontDefinition == null)
        {
            Debug.LogError("[BattleTextRenderer] Parameter 'fontDefinition' was null");
            return;
        }

        if (fontMaterial == null)
        {
            Debug.LogError("[BattleTextRenderer] Parameter 'fontMaterial' was null");
            return;
        }

        text = text ?? "";
        font = BattleTextFont.Load(fontDefinition);
        sentance = font.MakeSentance(text);

        MeshFilter filter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        renderer.material = fontMaterial;

        mesh = filter.mesh;
        mesh.Clear();

        Vector3[] vs = new Vector3[sentance.GlyphCount * 4];
        Vector2[] uvs = new Vector2[vs.Length];
        Color[] clr = new Color[vs.Length];
        int[] tris = new int[sentance.GlyphCount * 2 * 3];

        int v = 0;
        int t = 0;
        int u = 0;
        int gc = sentance.GlyphCount;
        float ts = font.TextureSize;

        float r = Color.r;
        float g = Color.g;
        float b = Color.b;
        float time = Time.time + FadeDelay;

        float offset = 0; // -(sentance.CalculateWidth(GlyphSpacing) / 2f);

        for (int gn = 0; gn < gc; ++gn)
        {
            if (gn > 0)
            {
                offset += GlyphSpacing;
            }

            BattleTextGlyph glyph = sentance[gn];

            // Vertices

            float width = glyph.CalculateWidth(font);
            float height = glyph.CalculateHeight(font);
            float glyphOffset = glyph.CalculateOffset(font);

            vs[v + 0] = new Vector3(offset, -glyphOffset);
            vs[v + 1] = new Vector3(offset + width, -glyphOffset);
            vs[v + 2] = new Vector3(offset, -height - glyphOffset);
            vs[v + 3] = new Vector3(offset + width, -height - glyphOffset);

            // Triangles

            tris[t + 0] = v + 0;
            tris[t + 1] = v + 1;
            tris[t + 2] = v + 2;

            tris[t + 5] = v + 2;
            tris[t + 4] = v + 3;
            tris[t + 3] = v + 1;

            // UVs

            float x = glyph.X;
            float y = glyph.Y;
            float w = glyph.Width;
            float h = glyph.Height;

            float top = ts - y;
            float right = x + w;

            if (IsAnimated)
            {
                // Magic :)
                uvs[v + 0] = new Vector2(x / ts, time);
                uvs[v + 1] = new Vector2(right / ts, time);
                uvs[v + 2] = new Vector2(x / ts, time);
                uvs[v + 3] = new Vector2(right / ts, time);

                clr[v + 0] = new Color(r, g, b, top / ts);
                clr[v + 1] = new Color(r, g, b, top / ts);
                clr[v + 2] = new Color(r, g, b, (top - h) / ts);
                clr[v + 3] = new Color(r, g, b, (top - h) / ts);
            }
            else
            {
                uvs[v + 0] = new Vector2(x / ts, top / ts);
                uvs[v + 1] = new Vector2(right / ts, top / ts);
                uvs[v + 2] = new Vector2(x / ts, (top - h) / ts);
                uvs[v + 3] = new Vector2(right / ts, (top - h) / ts);

                clr[v + 0] = Color;
                clr[v + 1] = Color;
                clr[v + 2] = Color;
                clr[v + 3] = Color;
            }

            // Increment for next

            t += 6;
            v += 4;
            u += 4;

            offset += width;
        }

        mesh.vertices = vs;
        mesh.uv = uvs;
        mesh.colors = clr;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}