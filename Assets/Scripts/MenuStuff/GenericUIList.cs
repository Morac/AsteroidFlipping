using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UIGrid))]
public abstract class GenericUIList : MonoBehaviour {

	public ListItem ListItemPrefab;

	List<ListItem> listitems = new List<ListItem>();

	UIGrid _grid;
	protected UIGrid Grid
	{
		get
		{
			if (_grid == null)
				_grid = GetComponent<UIGrid>();
			return _grid;
		}
	}
	protected abstract List<object> ListData();
	protected virtual int SortMethod(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public virtual List<string> LabelNames()
	{
		return null;
	}

	public virtual List<string> GroupNames()
	{
		return null;
	}

	void OnEnable()
	{
		foreach(var data in ListData())
		{
			AddListItem(data);
		}
		Grid.sorting = UIGrid.Sorting.Custom;
		Grid.onCustomSort = SortMethod;
		Grid.Reposition();
	}

	protected void AddListItem(object data)
	{
		var item = Instantiate(ListItemPrefab, transform.position, transform.rotation) as ListItem;
		item.transform.parent = transform;
		item.transform.localScale = Vector3.one;
		listitems.Add(item);
		item.Data = data;

		OnListItemCreate(item);
	}

	protected abstract void OnListItemCreate(ListItem item);

	void OnDisable()
	{
		if (listitems == null)
			return;
		foreach(var item in listitems)
		{
			if (item == null)
				continue;
			Destroy(item.gameObject);
		}
	}

}
