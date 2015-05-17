using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

// статический класс с методами расширения для таблицы очков
public static class TableScore
{
	// сортировка по очкам, жизням и имени
	public static IOrderedEnumerable<Record> SortByScoreLifeName(this Record[] records)
	{
		IOrderedEnumerable<Record> request = from n in records
											 orderby n.name
											 orderby n.score descending, n.life descending
											 select n;

		return request;
	}

	//// установить позицию скроллинга в пределах видимости рейтинга текущего игрока
	public static float StartShowVerticalPosition(this ScrollRect scroll, int curIndex, float heightRec)
	{
		int countVisibleRec = Mathf.FloorToInt (scroll.content.rect.height / heightRec);
		return Mathf.Clamp01(1 - (float)(curIndex - (countVisibleRec - 1))/(scroll.content.childCount - countVisibleRec));
	}
}
