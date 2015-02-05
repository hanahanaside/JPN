using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface StageDao
{

	List<Stage> SelectAll ();

	Stage SelectById (int id);

	void UpdateRecord (Stage stage);

	void UpdateAllUpdateDate(string updateDate);
}