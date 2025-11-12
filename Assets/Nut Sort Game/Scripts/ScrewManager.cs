using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrewManager : MonoBehaviour
{
	private List<Bolt> screwList;

	private void Awake()
	{
		screwList = transform.GetComponentsInChildren<Bolt>().ToList();
	}

	public List<Bolt> GetScrewLists()
	{
		return screwList;
	}
}