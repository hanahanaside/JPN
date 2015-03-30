using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class GenerateCoinPower_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/JPN/Resources/Data/GenerateCoinPower.xls";
    private static readonly string[] sheetNames = { "GenerateCoinPower", };
    
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
                    var exportPath = "Assets/JPN/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_GenerateCoinPower)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_GenerateCoinPower));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_GenerateCoinPower>();
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
                        
                        var p = new Entity_GenerateCoinPower.Param();
			
					cell = row.GetCell(0); p.area_id = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.area_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.level_1 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.level_2 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.level_3 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.level_4 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.level_5 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.max = (cell == null ? 0.0 : cell.NumericCellValue);

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
