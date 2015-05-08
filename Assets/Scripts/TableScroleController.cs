using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections;

// картеж в таблице рейтинга
public class Record
{
	public Record( string _name, int _score, int _life)
	{
		name = _name;
		score = _score;
		life = _life;
	}
	public string name;
	public int score;
	public int life;
}

// таблица рейтинга
public class TableScroleController : MonoBehaviour {

	public Image record;
	public Color[] reward = new Color[4];
		
	Transform recordParent;
	ArrayList table;
	ScrollRect scroll;
	Record cur;
	Record prev;
	int curIndex;
	int rating = 1;
	float heightRec;

	void Awake()
	{
		recordParent = transform.Find ("Panel").Find ("ScrollField").Find ("Content");
		scroll = transform.Find ("Panel").Find ("ScrollField").GetComponent<ScrollRect> ();
		table = new ArrayList ();
		cur = GameManager.Instance.curPlayer;
	}

	void Start () 
	{
		BinaryReader dataIn;
		string name;
		int score;
		int life;

		// считывание из файла
		curIndex = 0;
		dataIn = new BinaryReader(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Open));
		try{
			for(;;)
			{
				name = dataIn.ReadString();
				score = dataIn.ReadInt32();
				life = dataIn.ReadInt32();
				table.Add( new Record( name, score, life ));
			}
		}catch(EndOfStreamException)
		{
			dataIn.Close();
			ShowTable();
		}
	}

	// заполнение таблицы рейтинга
	void AddRecord( Record r, int index )
	{
		Image rec = Instantiate(record) as Image;
		rec.transform.SetParent(recordParent);
		Text recRating = rec.transform.Find ("Rating").GetComponent<Text> ();
		Text recName = rec.transform.Find ("Name").GetComponent<Text> ();
		recName.text = r.name;
		Text recScore = rec.transform.Find ("Score").GetComponent<Text> ();
		recScore.text = r.score.ToString();
		Text recLife = rec.transform.Find ("Life").GetComponent<Text> ();
		recLife.text = r.life.ToString();
		// строка для текущего игрока мигает
		if (cur != null && r.name == cur.name && r.score == cur.score && r.life == cur.life) 
		{
			rec.GetComponent<Animator>().SetTrigger("Animating");
			curIndex = index;
			heightRec = recName.rectTransform.rect.height;
		}
		if (index > 1 && r.score == prev.score && r.life == prev.life)
		{
			recRating.text = rating.ToString() + ".";
		}
		else
		{
			recRating.text = index.ToString() + ".";
			rating = index;
		}
		prev = r;
		rec.color = rating < 4 ? reward [rating - 1] : reward [3]; // цветовая индикация рейтинга
	}

	void ShowTable()
	{
		// сортировка по очкам, жизням и имени
		var request = from n in (Record[])table.ToArray(typeof(Record))
					  orderby n.name
			          orderby n.score descending, n.life descending
				      select n;

		int i = 1;
		foreach (Record rec in request)
		{
			AddRecord( rec, i );
			i++;
		}
		// установить позицию скроллинга в пределах видимости рейтинга текущего игрока
		int countVisibleRec = Mathf.FloorToInt( scroll.content.rect.height / heightRec);
		scroll.verticalNormalizedPosition = Mathf.Clamp01(1 - (float)(curIndex - (countVisibleRec - 1))/(scroll.content.childCount - countVisibleRec));
	}
	

}
