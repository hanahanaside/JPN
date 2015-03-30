using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class UntilSleepTime_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/JPN/Resources/Data/UntilSleepTime.xls";
    private static readonly string[] sheetNames = { "UntilSleepTime", };
    
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
                    var data = (Entity_UntilSleepTime)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_UntilSleepTime));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_UntilSleepTime>();
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
                        
                        var p = new Entity_UntilSleepTime.Param();
			
					cell = row.GetCell(0); p.area_id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.area_name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.level_1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.level_2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.level_3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.level_4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.level_5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.max = (int)(cell == null ? 0 : cell.NumericCellValue);

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
