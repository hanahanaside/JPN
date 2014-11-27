using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface StageDao {

	List<Stage> SelectAll ();

	void UpdateById(Stage stage);
}