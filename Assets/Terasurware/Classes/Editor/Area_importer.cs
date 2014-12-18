using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class Area_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/Area.xls";
    private static readonly string[] sheetNames = { "Area", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read))
            {
                var book = new HSSFWorkbook(stream);

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_Area)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_Area));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_Area>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_Area.Param();
			
					cell = row.GetCell(0); p.area_id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.area_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.area_open = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.minimum_amount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.cost_start = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.cost_add = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.cost_end = (int)(cell == null ? 0 : cell.NumericCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
