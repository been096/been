#if UNITY_EDITOR
using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic; // List ���
using System.IO;                  // ���/Ȯ����
using System.Text;                // StringBuilder
using UnityEditor;                // ������ API
using UnityEngine;                // Unity �⺻

/// <summary>
/// [������ ����] monsters.csv�� �о� MonsterDatabaseSO�� ä�� �ִ� ������
/// - CSV�� ��ǥ ����, ū����ǥ�� ���� �ʵ�� �̽�������("" �� ") ����
/// - ū����ǥ ���� �ٹٲ޵� ó��(�� ���� ����)
/// - ����� ���� �̸�(id,name,maxHP,moveSpeed,prefabPath)��� ����(���� ������ ����)
/// </summary>
public class MonsterDB_CsvImporter : EditorWindow
{
    // �Է�: ������Ʈ�� �־�� CSV(TextAsset)
    public TextAsset monstersCsv; // ��: Assets/DataTables/monsters.csv
    public TextAsset itemsCsv;

    // ���: SO ���� ��ġ/�̸�
    public string outputFolderAssets = "Assets/GameData";
    public string outputAssetName = "MonsterDatabase.asset";
    public string outputAssetName2 = "ItemDatabase.asset";

    [MenuItem("Tools/GameDataDB/Import from CSV")]
    public static void ShowWindow()
    {
        MonsterDB_CsvImporter win = GetWindow<MonsterDB_CsvImporter>("GameDataDB CSV Importer");
        win.minSize = new Vector2(560, 200);
    }

    void OnGUI()
    {
        GUILayout.Label("Input (CSV Only)", EditorStyles.boldLabel);
        monstersCsv = (TextAsset)EditorGUILayout.ObjectField("monsters.csv", monstersCsv, typeof(TextAsset), false);

        GUILayout.Space(8);
        GUILayout.Label("Output", EditorStyles.boldLabel);
        outputFolderAssets = EditorGUILayout.TextField("Folder (Assets/...)", outputFolderAssets);
        outputAssetName = EditorGUILayout.TextField("Asset Name", outputAssetName);

        GUILayout.Space(10);
        if (GUILayout.Button("Import Now") == true)
        {
            ImportNow();
        }

        GUILayout.Space(8);
        GUILayout.Label("Input (CSV Only)", EditorStyles.boldLabel);
        itemsCsv = (TextAsset)EditorGUILayout.ObjectField("items.csv", itemsCsv, typeof(TextAsset), false);

        GUILayout.Space(8);
        GUILayout.Label("Output", EditorStyles.boldLabel);
        outputFolderAssets = EditorGUILayout.TextField("Folder (Assets/...)", outputFolderAssets);
        outputAssetName2 = EditorGUILayout.TextField("Asset Name", outputAssetName2);

        GUILayout.Space(10);
        if (GUILayout.Button("Import Now") == true)
        {
            ImportItem();
        }
    }

    void ImportNow()
    {
        // 1) �Է� Ȯ��
        if (monstersCsv == null)
        {
            EditorUtility.DisplayDialog("Error", "monsters.csv(TextAsset)�� �����ϼ���.", "OK");
            return;
        }

        // 2) ��� ���� �غ�
        if (string.IsNullOrEmpty(outputFolderAssets) == true)
        {
            EditorUtility.DisplayDialog("Error", "Output Folder�� ������ϴ�.", "OK");
            return;
        }
        if (Directory.Exists(outputFolderAssets) == false)
        {
            Directory.CreateDirectory(outputFolderAssets);
            AssetDatabase.Refresh();
        }

        // 3) SO �ε� �Ǵ� ����
        string dbPath = Path.Combine(outputFolderAssets, outputAssetName).Replace("\\", "/");
        MonsterDatabaseSO db = AssetDatabase.LoadAssetAtPath<MonsterDatabaseSO>(dbPath);
        if (db == null)
        {
            db = ScriptableObject.CreateInstance<MonsterDatabaseSO>();
            AssetDatabase.CreateAsset(db, dbPath);
        }

        // 4) CSV �Ľ� �� ����Ʈ ä���
        db.monsters.Clear();
        ImportMonstersCSV(monstersCsv.text, db.monsters);

        // 5) ���� + ��Ÿ�� �� ����
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
        db.BuildMap();

        EditorUtility.DisplayDialog("Done", "monsters.csv �� MonsterDatabase ����Ʈ �Ϸ�!", "OK");
    }

    void ImportItem()
    {
        // 1) �Է� Ȯ��
        if (itemsCsv == null)
        {
            EditorUtility.DisplayDialog("Error", "items.csv(TextAsset)�� �����ϼ���.", "OK");
            return;
        }

        // 2) ��� ���� �غ�
        if (string.IsNullOrEmpty(outputFolderAssets) == true)
        {
            EditorUtility.DisplayDialog("Error", "Output Folder�� ������ϴ�.", "OK");
            return;
        }
        if (Directory.Exists(outputFolderAssets) == false)
        {
            Directory.CreateDirectory(outputFolderAssets);
            AssetDatabase.Refresh();
        }

        // 3) SO �ε� �Ǵ� ����
        string dbPath = Path.Combine(outputFolderAssets, outputAssetName2).Replace("\\", "/");
        ItemDatabaseSO db = AssetDatabase.LoadAssetAtPath<ItemDatabaseSO>(dbPath);
        if (db == null)
        {
            db = ScriptableObject.CreateInstance<ItemDatabaseSO>();
            AssetDatabase.CreateAsset(db, dbPath);
        }

        // 4) CSV �Ľ� �� ����Ʈ ä���
        db.items.Clear();
        ImportItemsCSV(itemsCsv.text, db.items);

        // 5) ���� + ��Ÿ�� �� ����
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
        db.BuildMap();

        EditorUtility.DisplayDialog("Done", "items.csv �� ItemDatabase ����Ʈ �Ϸ�!", "OK");
    }

    // ================= CSV �ļ�(�߰�����) =================
    // - ū����ǥ ���� ��ǥ���ٹٲ� ó��
    // - "" �� " �̽������� ó��
    // - ������ ���� ���� ���� ������ ó��

    List<string[]> ParseCsvStrict(string text)
    {
        List<string[]> rows = new List<string[]>();
        List<string> cols = new List<string>();
        StringBuilder cur = new StringBuilder();

        bool inQuote = false;
        int i = 0;
        while (i < text.Length)
        {
            char ch = text[i];

            if (inQuote == true)
            {
                // ����ǥ ���
                if (ch == '"')
                {
                    // "" �� " (�̽�������)
                    if (i + 1 < text.Length && text[i + 1] == '"')
                    {
                        cur.Append('"');
                        i = i + 1; // ���� " �Һ�
                    }
                    else
                    {
                        inQuote = false; // ����ǥ ����
                    }
                }
                else
                {
                    cur.Append(ch); // �״�� ����(��ǥ/���� ����)
                }
            }
            else
            {
                // ���� ���
                if (ch == '"')
                {
                    inQuote = true; // ����ǥ ����
                }
                else if (ch == ',')
                {
                    // �÷� �ϳ� ����
                    cols.Add(cur.ToString());
                    cur.Length = 0;
                }
                else if (ch == '\r' || ch == '\n')
                {
                    // �ٹٲ�(�� ����)
                    cols.Add(cur.ToString());
                    cur.Length = 0;

                    rows.Add(cols.ToArray());
                    cols.Clear();

                    // CRLF ó��: \r\n�̸� \n �� �� �� ��ŵ
                    if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        i = i + 1;
                    }
                }
                else
                {
                    cur.Append(ch);
                }
            }

            i = i + 1;
        }

        // ���� �� ó��(������ �࿡ ������ ������ ���)
        if (inQuote == false)
        {
            cols.Add(cur.ToString());
            if (cols.Count > 1 || (cols.Count == 1 && string.IsNullOrEmpty(cols[0]) == false))
            {
                rows.Add(cols.ToArray());
            }
        }
        else
        {
            // ����ǥ�� ������ ���� ���: �������̶� ���
            Debug.LogWarning("CSV �ļ�: ������ ������ ����ǥ�� ������ �ʾҽ��ϴ�.");
            cols.Add(cur.ToString());
            rows.Add(cols.ToArray());
        }

        return rows;
    }

    // ���� CSV �� ����Ʈ ä���
    void ImportMonstersCSV(string csv, List<MonsterDatabaseSO.MonsterDef> outList)
    {
        List<string[]> rows = ParseCsvStrict(csv);
        if (rows.Count <= 1)
        {
            Debug.LogWarning("monsters.csv: ������ ���� �����ϴ�.");
            return;
        }

        // --- ��� �ľ�(�̸��� �´��� Ȯ��) ---
        string[] header = rows[0];
        int idCol = IndexOf(header, "id");
        int nameCol = IndexOf(header, "name");
        int hpCol = IndexOf(header, "maxHP");
        int speedCol = IndexOf(header, "moveSpeed");
        int prefabCol = IndexOf(header, "prefabPath");
        int iconCol = IndexOf(header, "iconPath");

        if (idCol < 0 || nameCol < 0 || hpCol < 0 || speedCol < 0 || prefabCol < 0 || iconCol < 0)
        {
            Debug.LogWarning("monsters.csv: ������� �ùٸ��� �ʽ��ϴ�. (id,name,maxHP,moveSpeed,prefabPath)");
            return;
        }

        // --- ������ �� ó�� ---
        int r = 1;
        while (r < rows.Count)
        {
            string[] c = rows[r];
            if (c == null || c.Length == 0)
            {
                r = r + 1;
                continue;
            }

            // �� �� ��ȣ
            if (AllEmpty(c) == true)
            {
                r = r + 1;
                continue;
            }

            // �÷� ���� ����
            string id = SafeLower(GetCell(c, idCol));
            string name = GetCell(c, nameCol).Trim();

            // ���� �Ľ�(���� �� �⺻��)
            int maxHP = ParseInt(GetCell(c, hpCol), 1);
            float moveSpeed = ParseFloat(GetCell(c, speedCol), 1f);

            // ������ ���� �ε�
            string prefabPath = GetCell(c, prefabCol).Trim();
            GameObject pf = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (pf == null)
            {
                Debug.LogWarning("monsters.csv: prefab �ε� ����: " + prefabPath + " (row " + r.ToString() + ")");
            }

            string iconPath = GetCell(c, iconCol).Trim();
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);

            // �׸� ����
            MonsterDatabaseSO.MonsterDef def = new MonsterDatabaseSO.MonsterDef();
            def.id = id;
            def.displayName = name;
            def.maxHP = maxHP;
            def.moveSpeed = moveSpeed;
            def.prefab = pf;
            def.icon = icon;

            outList.Add(def);

            r = r + 1;
        }
    }

    void ImportItemsCSV(string csv, List<ItemDatabaseSO.ItemDef> outList)
    {
        List<string[]> rows = ParseCsvStrict(csv);
        if (rows.Count <= 1)
        {
            Debug.LogWarning("items.csv: ������ ���� �����ϴ�.");
            return;
        }

        // --- ��� �ľ�(�̸��� �´��� Ȯ��) ---
        string[] header = rows[0];
        int idCol = IndexOf(header, "id");
        int nameCol = IndexOf(header, "name");
        int itemTypeCol = IndexOf(header, "itemType");
        int priceCol = IndexOf(header, "price");

        if (idCol < 0 || nameCol < 0 || itemTypeCol < 0 || priceCol < 0)
        {
            Debug.LogWarning("items.csv: ������� �ùٸ��� �ʽ��ϴ�. (id,name,itemType,price)");
            return;
        }

        // --- ������ �� ó�� ---
        int r = 1;
        while (r < rows.Count)
        {
            string[] c = rows[r];
            if (c == null || c.Length == 0)
            {
                r = r + 1;
                continue;
            }

            // �� �� ��ȣ
            if (AllEmpty(c) == true)
            {
                r = r + 1;
                continue;
            }

            // �÷� ���� ����
            string id = SafeLower(GetCell(c, idCol));
            string name = GetCell(c, nameCol).Trim();

            // ���� �Ľ�(���� �� �⺻��)
            string itemType = GetCell(c, itemTypeCol).Trim();
            int price = ParseInt(GetCell(c, priceCol), 1);

            // �׸� ����
            ItemDatabaseSO.ItemDef def = new ItemDatabaseSO.ItemDef();
            def.id = id;
            def.name = name;

            switch (itemType)
            {
                case "Weapon":
                    {
                        def.itemType = ItemType.Weapon;
                    }
                    break;

                case "Potion":
                    {
                        def.itemType = ItemType.Potion;
                    }
                    break;

                case "Scroll":
                    {
                        def.itemType = ItemType.Scroll;
                    }
                    break;

                default:
                    {
                        def.itemType = ItemType.None;
                    }
                    break;
            }
            def.price = price;

            outList.Add(def);

            r = r + 1;
        }
    }

    // -------- �ҵ���(������ ���� �и�) --------
    int IndexOf(string[] header, string name)
    {
        int i = 0;
        while (i < header.Length)
        {
            // ����� ��ҹ��� ���� ���� ��(Trim)
            string h = header[i] != null ? header[i].Trim() : "";
            if (string.Equals(h, name, System.StringComparison.OrdinalIgnoreCase) == true)
            {
                return i;
            }
            i = i + 1;
        }
        return -1;
    }

    string GetCell(string[] row, int index)
    {
        if (row == null)
        {
            return "";
        }
        if (index < 0 || index >= row.Length)
        {
            return "";
        }
        string v = row[index];
        if (v == null)
        {
            return "";
        }
        return v;
    }

    bool AllEmpty(string[] row)
    {
        if (row == null)
        {
            return true;
        }
        int i = 0;
        while (i < row.Length)
        {
            string v = row[i];
            if (string.IsNullOrWhiteSpace(v) == false)
            {
                return false;
            }
            i = i + 1;
        }
        return true;
    }

    string SafeLower(string raw)
    {
        if (string.IsNullOrEmpty(raw) == true)
        {
            return "";
        }
        return raw.Trim().ToLower();
    }

    int ParseInt(string s, int fallback)
    {
        int v;
        bool ok = int.TryParse(s, out v);
        if (ok == true)
        {
            return v;
        }
        return fallback;
    }

    float ParseFloat(string s, float fallback)
    {
        float v;
        bool ok = float.TryParse(s, out v);
        if (ok == true)
        {
            return v;
        }
        return fallback;
    }
}
#endif
