using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * TODO
 */
public class GridGenerator : MonoSingleton<GridGenerator>
{
    public List<GameObject> tiles = new List<GameObject> ();
 
    
    public GameObject Fetch( int x, int y)
    {
        return this.Fetch(new Coord(x,y));   
    }
    public GameObject Fetch(Coord coord)
    {
        foreach(GameObject tile in tiles)
        {
            if(tile.GetComponent<GridModel>().coord.Equals(coord))
            {
                return tile;
            }
        }
        
        return null;
    }
    
    public List<GameObject> FetchArea(Coord coord, int range)
    {
        List<GameObject> gos = new List<GameObject> ();
        /*
         *  - - - - - - -
         * - - 2 2 2 - - 
         *  - 2 1 1 2 - -
         * - 2 1 0 1 2 -
         *  - 2 1 1 2 - -
         * - - 2 2 2 - -
         *  - - - - - - -
         *
         * determine level.
         * 
         * level = number left, number right.
         * 
         * step left X. select.
         * step right X.
         * 
         */
        
        int a = 0;
        int b = 1;
        int nr = 0;
        int nl = 0;
        
        int r = range / 2;
        
        if (coord.y % 2 == 0)
        {
            if(range % 2 == 1)
            {
                b = 0;
                a = 1;
            }
            else
            {
                b = 0;
            }
        }
        else
        {
            if(range % 2 == 0)
            {
                
                b = 0;
            }
            else
            {
            }
        }

        List<Coord> list = new List<Coord>();
        
        list.Add(new Coord(coord.x + range, coord.y));
        list.Add(new Coord(coord.x - range, coord.y));
        
        if(range > 1)
        {
            for(int m = 1; m < range; m++)
            {
                nr = 0;
                nl = 0;
                if(coord.y % 2 == 1 && m % 2 == 1)
                {
                    nr = 1;
                }
                
                if(coord.y % 2 == 0 && m % 2 == 1)
                {
                    nl = 1;
                }
                list.Add(new Coord(coord.x + range - (m/2) - nr, coord.y + m));
                list.Add(new Coord(coord.x - range + (m/2) + nl, coord.y + m));
                list.Add(new Coord(coord.x + range - (m/2) - nr, coord.y - m));
                list.Add(new Coord(coord.x - range + (m/2) + nl, coord.y - m));
            }
        }
        for(int x = -r - b; x <= r + a; x++)
        {
            list.Add(new Coord(coord.x + x, coord.y + range));
        }
        
        for(int x = -r - b; x <= r + a; x++)
        {
            list.Add(new Coord(coord.x + x, coord.y - range));
        }
        
        foreach(Coord c in list)
        {
            GameObject go = Fetch(c);
                
            if(go != null)
            {
                gos.Add(go);
            }
        }
        
        
        
        
        
        if (range > 1)
        {
            List<GameObject> _gos = FetchArea(coord, range - 1);
            
            foreach (GameObject _go in _gos)
            {
                gos.Add(_go);
            }
        }
        
        return gos;
    }
    
    public List<GameObject> FetchCone(Coord coord, int range)
    {
        
        List<GameObject> gos = new List<GameObject> ();
        
        int a = 0;
        int b = 0;
        
        if (coord.y % 2 == 0)
        {
            if (range % 2 == 1)
            {
                a = 1;
            }
        }
        else
        {
            if (range % 2 == 1)
            {
                b = 1;
            }
        }
        
        if (range % 2 == 0)
        {
               // a = 1;
        }
        
        int r = range/2;
        
        for(int x = -r - b; x <= r + a; x++)
        {
            Coord c = new Coord(coord.x + x, coord.y + range);
            GameObject go = Fetch(c);
                
            if(go != null)
            {
                gos.Add(go);
            }
        }
        
        if (range > 1)
        {
            List<GameObject> _gos = FetchCone(coord, range - 1);
            
            foreach (GameObject go in _gos)
            {
                gos.Add(go);
            }
        }
        
        return gos;
    }
 
    public List<GameObject> FetchLine(Coord coord, int range)
    {
        
        List<GameObject> gos = new List<GameObject> ();
        
        //Coord c = new Coord(coord.x, coord.y + range);
        //Coord c = new Coord(coord.x+(range%2), coord.y + range);
        
        //int r = range/2;
        //Coord c = new Coord(coord.x-r, coord.y + range);
        int a = 0;
        
        if (coord.y % 2 == 0)
        {
            if (range % 2 == 1)
            {
                a = 1;
            }
        }
        
        int r = range/2;
        Coord c = new Coord(coord.x + r + a, coord.y + range);
        
        GameObject go = Fetch(c);
                
        if(go != null)
        {
            gos.Add(go);
        }
        
        if (range > 1)
        {
            List<GameObject> _gos = FetchLine(coord, range - 1);
            
            foreach (GameObject _go in _gos)
            {
                gos.Add(_go);
            }
        }
        
        return gos;
    }
    
    public void Clear ()
    {
        foreach (GameObject go in tiles) {
            Destroy (go);
        }
     
        tiles = new List<GameObject> ();
    }
    
    // Use this for initialization
	public void Generate (string name,GameObject obj, Vector3 origin, int width, int height, Transform parent = null)//box
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3 position = origin + new Vector3 (x, 0, y);
                GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + x + y;
				GridModel model = go.GetComponent<GridModel>();
                model.coord = new Coord(x,y);
                tiles.Add (go);
				go.transform.SetParent(parent);
            }
        }
    }

	public void GenerateV (string name,GameObject obj, Vector3 origin, int vertical,int objCount = 0, Transform parent = null)//vertical
	{
		for (int y = 1; y < vertical; y++) {
			Vector3 position = origin + new Vector3 (0, 0, y);
			GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
			go.name = name + "_VT" + objCount;
			objCount++;
			//go.tag = name;
			GridModel model = go.GetComponent<GridModel> ();
			model.coord = new Coord (y, 0);
			tiles.Add (go);
			go.transform.SetParent(parent);
		}
	}

	public void GenerateH (string name,GameObject obj, Vector3 origin, int horizontal, int objCount = 0, Transform parent = null)//horizontal
	{
		for (int x = 1; x < horizontal; x++) {
			Vector3 position = origin + new Vector3 (x, 0, 0);
			GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
			go.name = name + "_HT" + objCount;
			objCount++;
			//go.tag = name;
			GridModel model = go.GetComponent<GridModel> ();
			model.coord = new Coord (x, 0);
			tiles.Add (go);
			go.transform.SetParent(parent);
		}
	}
	public void GenerateB (string name,GameObject obj, Vector3 origin, int width, int height, Transform parent = null)
	{
		int objectCount = 0;
		GenerateH (name,obj,origin,width, objectCount,parent);//draw 1st horizontal
		GenerateV (name,obj,origin,height, objectCount,parent);//draw 1st vertical
		Vector3 Vertical2 = new Vector3 (width-1, 0, 0);//translation for 2nd vertical
		GenerateV (name,obj,origin+Vertical2,height, objectCount,parent);//draw 2nd vertical
		Vector3 Horizontal2 = new Vector3 (0, 0, height - 1);//translation for 2nd horizontal
		GenerateH (name,obj,origin+Horizontal2,width, objectCount,parent);//draw 2nd horizontal
	}

	public void GenerateSq (string name,GameObject obj,GameObject obj2, Vector3 origin, int width, int height, string[] Obstacles,Transform parent = null, bool once = false,GameObject obj3 = null, int nameLength = 3)//box
	{
		int objCount = 0;
		Vector3 position1 = origin + new Vector3 (0, 0, 0);
		GameObject go1 = Instantiate (obj, position1, Quaternion.identity) as GameObject;
		go1.name = name + "_T" + objCount;
		objCount++;
		//go1.tag = name;
		GridModel model1 = go1.GetComponent<GridModel> ();
		model1.coord = new Coord (0, 0);
		tiles.Add (go1);
		go1.transform.SetParent(parent);
		bool obsHit1 = false;//init obstacle hit 1
		bool Merge = false;
		for (int x = 1; x < width; x++) {
			Vector3 position = origin + new Vector3 (x, 0, x);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location
			
			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit1 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit1 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit1 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit1 = true;
					}
					if(obsHit1)
						break;
				}
				if(obsHit1)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit1 = false;
				Merge = false;
			}
			else if(obsHit1)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit1 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit2 = false;//init obstacle hit 2
		for (int x = 1; x < width; x++) {
			Vector3 position = origin + new Vector3 (-x, 0, x);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location
			
			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit2 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit2 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit2 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit2 = true;
					}
					if(obsHit2)
						break;
				}
				if(obsHit2)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit2 = false;
				Merge = false;
			}
			else if(obsHit2)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit2 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit3 = false;//init obstacle hit 3
		for (int y = 1; y < height; y++) {
			Vector3 position = origin + new Vector3 (y, 0, -y);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location
			
			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit3 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit3 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit3 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit3 = true;
					}
					if(obsHit3)
						break;
				}
				if(obsHit3)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit3 = false;
			}
			else if(obsHit3)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit3 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit4 = false;//init obstacle hit 4
		for (int y = 1; y < height; y++) {
			Vector3 position = origin + new Vector3 (-y, 0, -y);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location
			
			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit4 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit4 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit4 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit4 = true;
					}
					if(obsHit4)
						break;
				}
				if(obsHit4)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit4 = false;
				Merge = false;
			}
			else if(obsHit4)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit4 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
	}

	public void GenerateT (string name,GameObject obj,GameObject obj2, Vector3 origin, int width, int height, string[] Obstacles, Transform parent = null, bool once = false, GameObject obj3 = null,int nameLength = 3)//t shape
	{
		int objCount = 0;
		Vector3 position1 = origin + new Vector3 (0, 0, 0);
		GameObject go1 = Instantiate (obj, position1, Quaternion.identity) as GameObject;
		go1.name = name + "_T" + objCount;
		objCount++;
		//go1.tag = name;
		GridModel model1 = go1.GetComponent<GridModel> ();
		model1.coord = new Coord (0, 0);
		tiles.Add (go1);
		go1.transform.SetParent(parent);
		bool obsHit1 = false;//init obstacle hit 1
		bool Merge = false;
		for (int x = 1; x < width; x++) {
			Vector3 position = origin + new Vector3 (x, 0, 0);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location

			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit1 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit1 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit1 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit1 = true;
					}
					if(obsHit1)
						break;
				}
				if(obsHit1)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit1 = false;
				Merge = false;
			}
			else if(obsHit1)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit1 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit2 = false;//init obstacle hit 2
		for (int x = 1; x < width; x++) {
			Vector3 position = origin + new Vector3 (-x, 0, 0);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location

			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit2 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit2 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit2 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit2 = true;
					}
					if(obsHit2)
						break;
				}
				if(obsHit2)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit2 = false;
				Merge = false;
			}
			else if(obsHit2)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit2 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (-x, 0);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit3 = false;//init obstacle hit 3
		for (int y = 1; y < height; y++) {
			Vector3 position = origin + new Vector3 (0, 0, y);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location

			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit3 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit3 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit3 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit3 = true;
					}
					if(obsHit3)
						break;
				}
				if(obsHit3)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit3 = false;
				Merge = false;
			}
			else if(obsHit3)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit3 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, y);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
		bool obsHit4 = false;//init obstacle hit 4
		for (int y = 1; y < height; y++) {
			Vector3 position = origin + new Vector3 (0, 0, -y);
			Collider[] hitColliders = Physics.OverlapSphere (position, 0);//get object in this location

			for(int i = 0; i < hitColliders.Length; i++)
			{
				for(int j = 0; j < Obstacles.Length; j++)
				{
					if(obj3 != null)
					{
						if(parent.name.Substring (0, nameLength - 1) == hitColliders[i].name.Substring (0, nameLength - 1))//check for obstacle
						{
							obsHit4 = true;
							Merge = true;
						}
						else if(hitColliders[i].transform.tag == parent.transform.tag)
							obsHit4 = true;
					}
					else if(hitColliders[i].transform.tag == parent.transform.tag)
						obsHit4 = true;
					if(hitColliders[i].transform.tag == Obstacles[j])//check for obstacle
					{
						obsHit4 = true;
					}
					if(obsHit4)
						break;
				}
				if(obsHit4)
					break;
			}
			if(Merge)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj3, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit4 = false;
				Merge = false;
			}
			else if(obsHit4)//end loop if hit obstacle
			{
				GameObject go = Instantiate (obj2, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
				if(once)
					obsHit4 = false;
			}
			else
			{
				GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
				go.name = name + "_T" + objCount;
				objCount++;
				//go.tag = name;
				GridModel model = go.GetComponent<GridModel> ();
				model.coord = new Coord (0, -y);
				tiles.Add (go);
				go.transform.SetParent(parent);
			}
		}
	}

    // Use this for initialization
	public void GenerateHex (GameObject obj, Vector3 origin, int width, int height, Transform parent = null)
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3 position = origin + new Vector3 (x * 1.75f, 0, y * 1.5f);
             
                if (y % 2 == 0) {
                    position += new Vector3 ((1.75f / 2f), 0, 0);
                }
             GameObject go = Instantiate (obj, position, Quaternion.identity) as GameObject;
                GridModel model = go.GetComponent<GridModel>();
                model.coord = new Coord(x,y);
                tiles.Add (go);
				go.transform.SetParent(parent);
            }
        }
    }
	public void Delete (string name)
	{
		foreach (GameObject go in tiles ) {
			if(go != null)
			{
				if(go.name == name)
				Destroy (go);
			}
		}
	}
	public bool Check (string name)
	{
		foreach (GameObject go in tiles ) {
			if(go != null)
			{
				if(go.name == name)
					return true;
			}
		}
		return false;
	}
    /**
     * TODO
     * -- Generate by Pattern
     */
}
