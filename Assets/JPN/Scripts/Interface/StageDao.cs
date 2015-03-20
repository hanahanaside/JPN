using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface StageDao
{

	List<StageData> SelectAll ();

	StageData SelectById (int id);

	void UpdateRecord (StageData stage);

	void UpdateAllUpdateDate(string updateDate);

	void InsertData(StageData stageData);
}