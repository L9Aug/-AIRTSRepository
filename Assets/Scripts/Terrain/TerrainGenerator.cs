using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour
{
    int H;
    int W;
    int[,] Wmap;
    DPathfinding D = new DPathfinding();

    void Setup()
    {
        W = MapGenerator.Map.GetLength(0);
        H = MapGenerator.Map.GetLength(1);
    }

    public void StartGen()
    {
        D = new DPathfinding();
        Setup();
        WightMap();
        D.setup();
        CoastLines();
        WmapUpdate();
        FillPlains();
        Montain();
        WmapUpdate();
        Lake();
        WmapUpdate();
        Rivers();
        WmapUpdate();
        Grassland();
        WmapUpdate();
        Forest();
        WmapUpdate();
        Hills();
        WmapUpdate();
        Desert();
        //cleanup
        HoleFix();
        
        WmapUpdate();
        TextureCorrector(); //Set tile to there correct texture
    }

    void WightMap()
    {
        Wmap = new int[W, H];
        //int b = 0;
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if ((i >= 0 && !(i > 5)) || (i > H - 5) || (b >= 0 && !(b > 5) || (b >= W - 5)))
                {
                    MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
                    Wmap[i, b] = 0;
                }
            }
        }
    }
    
    void CoastLines()
    {

        List<DPathfinding.cord> marklist = new List<DPathfinding.cord>();
   
        marklist.Add(D.map[10, 10]);
        marklist.Add(D.map[10, W - 15]);
        marklist.Add(D.map[H - 15, W -15]);
        marklist.Add(D.map[H- 15, 10]);
        marklist.Add(D.map[10, 10]);

        for (int i = 0; i < marklist.Count; ++i)
        {
            if (((i + 1) < marklist.Count))
            {
                D.run(marklist[i], marklist[i + 1]);
            }
        }
    }

    void TextureCorrector()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                MapGenerator.Map[i, b].SetTexture(MapGenerator.Map[i, b].TerrainType);
            }
        }
    }

    void WmapUpdate()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {

                switch (MapGenerator.Map[i, b].TerrainType)
                {
                    case TerrainTypes.Plains:
                        Wmap[i, b] = 1;
                        break;

                    case TerrainTypes.Grassland:
                        Wmap[i, b] = 2;
                        break;

                    case TerrainTypes.Coast:
                        Wmap[i, b] = 3;
                        break;

                    case TerrainTypes.Hills:
                        Wmap[i, b] = 4;
                        break;

                    case TerrainTypes.Forest:
                        Wmap[i, b] = 5;
                        break;

                    case TerrainTypes.Mountains:
                        Wmap[i, b] = 6;
                        break;

                    case TerrainTypes.Desert:
                        Wmap[i, b] = 7;
                        break;

                    case TerrainTypes.River:
                        Wmap[i, b] = 8;
                        break;

                    case TerrainTypes.Lake:
                        Wmap[i, b] = 9;
                        break;

                    case TerrainTypes.Sea:
                        Wmap[i, b] = 10;
                        break;

                }
            }

        }
    }

    void Montain()
    {
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Plains)
                {
                    if (UnityEngine.Random.Range(0, 20) > 17 && D.map[i, b].cost > 17)
                    {
                        var ML = MapGenerator.Map[b, i].GetHexArea(UnityEngine.Random.Range(2, 4));
                        bool GoodToGo = true;
                        foreach (HexTile h in ML)
                        {
                            if ((h.TerrainType == TerrainTypes.Coast || h.TerrainType == TerrainTypes.Sea))
                            {
                                GoodToGo = false;
                            }
                        }


                        if (GoodToGo)
                        {
                            foreach (HexTile h in ML)
                            {

                                h.TerrainType = TerrainTypes.Mountains;
                            }
                        }

                    }
                }
            }
        }



    }

    void Lake()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Plains)
                {
                    if (UnityEngine.Random.Range(0, 20) > 18 && D.map[i, b].cost < 2)
                    {
                        var ML = MapGenerator.Map[b, i].GetHexArea(UnityEngine.Random.Range(2, 6));
                        bool GoodToGo = true;
                        foreach (HexTile h in ML)
                        {
                            if ((h.TerrainType == TerrainTypes.Coast || h.TerrainType == TerrainTypes.Sea))
                            {
                                GoodToGo = false;
                            }
                        }


                        if (GoodToGo)
                        {
                            D.ChangeOnsearch = true;
                            D.T = TerrainTypes.Lake;
                            D.OnlyChange = TerrainTypes.Plains;
                            D.runX(D.map[i, b], UnityEngine.Random.Range(5, 25));
                        }

                    }
                }
            }

        }
    }

    void Rivers()
    {
        List<DPathfinding.cord> targetlist = new List<DPathfinding.cord>();
        List<DPathfinding.cord> Closed = new List<DPathfinding.cord>();
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                if (Wmap[i, b] == 3 || Wmap[i, b] == 9)
                {
                    targetlist.Add(D.map[i, b]);
                }
            }
        }

        print(targetlist.Count);
       
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Mountains && !(Closed.Contains(D.map[i, b])))
                {

                    if (UnityEngine.Random.Range(0, 30) > 27)
                    {                 
                        var l = MapGenerator.Map[i, b].GetHexArea(4);
                        foreach (HexTile h in l)
                        {
                            Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                        }

                        D.ChangeOnsearch = false;
                        D.ChangeTerrainOnTrail = true;
                        D.T = TerrainTypes.River;
                        D.runToHit(D.map[i, b], targetlist);
                    }

                }
            }
        }

    }

    void Grassland()
    {
        List<DPathfinding.cord> targets = new List<DPathfinding.cord>();
        List<DPathfinding.cord> Closed = new List<DPathfinding.cord>();
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Lake || MapGenerator.Map[i, b].TerrainType == TerrainTypes.River && !(Closed.Contains(D.map[i, b])))
                {
                    targets.Add(D.map[i, b]);
                    var l = MapGenerator.Map[i, b].GetHexArea(3);
                    foreach (HexTile h in l)
                    {
                        Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                    }

                }

            }
        }


        /*
        foreach(DPathfinding.cord c in targets)
        {
            var l =MapGenerator.Map[c.X, c.Y].GetHexArea(UnityEngine.Random.Range(2, 6));
            foreach(HexTile h in l)
            {
                if(h.TerrainType == TerrainTypes.Plains)
                {
                    h.TerrainType = TerrainTypes.Grassland;
                }
            }


        }
        */

        foreach (DPathfinding.cord c in targets)
        {
            var l = MapGenerator.Map[c.X, c.Y].GetHexArea(UnityEngine.Random.Range(2, 6));
            foreach (HexTile h in l)
            {
                if (h.TerrainType == TerrainTypes.Plains)
                {
                    D.ChangeOnsearch = true;
                    D.T = TerrainTypes.Grassland;
                    D.OnlyChange = TerrainTypes.Plains;
                    D.runX(D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y], 5);
                }
            }


        }



    }

    void Forest()
    {
        List<DPathfinding.cord> targets = new List<DPathfinding.cord>();
        List<DPathfinding.cord> Closed = new List<DPathfinding.cord>();
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Plains && Surrounded(MapGenerator.Map[i, b], TerrainTypes.Grassland) && !(Closed.Contains(D.map[i, b])))
                {

                    targets.Add(D.map[i, b]);
                    var l = MapGenerator.Map[i, b].GetHexArea(5);
                    foreach (HexTile h in l)
                    {
                        Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                    }

                }
            }

        }

       
        foreach (DPathfinding.cord c in targets)
        {
            var l = MapGenerator.Map[c.X, c.Y].GetHexArea(UnityEngine.Random.Range(0, 2));
            foreach (HexTile h in l)
            {
                    D.ChangeOnsearch = true;
                    D.ChangeAll = true;
                    D.T = TerrainTypes.Forest;
                    D.OnlyChange = TerrainTypes.Grassland;
                    D.runX(D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y], 3);
                }
            }


        }

    void HoleFix()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Plains && (Surrounded(MapGenerator.Map[i, b],TerrainTypes.Grassland) || Surrounded(MapGenerator.Map[i, b], TerrainTypes.Forest)))
                {
                    MapGenerator.Map[i, b].TerrainType = TerrainTypes.Forest;
                }
            }
        }
    }

    void Hills()
    {
        List<DPathfinding.cord> targets = new List<DPathfinding.cord>();
        List<DPathfinding.cord> Closed = new List<DPathfinding.cord>();
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Mountains && !(Closed.Contains(D.map[i, b])))
                {

                    targets.Add(D.map[i, b]);
                    var l = MapGenerator.Map[i, b].GetHexArea(10);
                    foreach (HexTile h in l)
                    {
                        Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                    }

                }
            }

        }


        foreach (DPathfinding.cord c in targets)
        {
            var l = MapGenerator.Map[c.X, c.Y].GetHexArea(UnityEngine.Random.Range(0, 4));
            foreach (HexTile h in l)
            {
                if (UnityEngine.Random.Range(0, 20) < 10)
                {
                    D.ChangeOnsearch = true;
                    D.ChangeAll = false;
                    D.T = TerrainTypes.Hills;
                    D.OnlyChange = TerrainTypes.Plains;
                    D.runX(D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y], 3);
                }
            }
        }
    }

    void Desert()
    {

        List<DPathfinding.cord> targets = new List<DPathfinding.cord>();
        List<DPathfinding.cord> Closed = new List<DPathfinding.cord>();
        for (int i = 0; i < W; i++)
        {
            // MapGenerator.Map[b, i].SetTexture(TerrainTypes.Sea);
            for (int b = 0; b < H; b++)
            {
                if (MapGenerator.Map[i, b].TerrainType == TerrainTypes.Plains && !(Closed.Contains(D.map[i, b])))
                {


                    var Area = MapGenerator.Map[i, b].GetHexArea(5);
                    bool AreaGood = true;
                    foreach (HexTile h in Area)
                    {
                        if (h.TerrainType != TerrainTypes.Plains)
                        {
                            AreaGood = false;
                            //Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                        }
                    }


                    if(AreaGood)
                    {
                        targets.Add(D.map[i, b]);
                        var l = MapGenerator.Map[i, b].GetHexArea(4);
                        foreach (HexTile h in l)
                        {
                            Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                        }
                    }
                    else
                    {
                        var l = MapGenerator.Map[i, b].GetHexArea(4);
                        foreach (HexTile h in l)
                        {
                            Closed.Add((D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y]));
                        }
                    }
                   
                

                }
            }

        }


        foreach (DPathfinding.cord c in targets)
        {
            var l = MapGenerator.Map[c.X, c.Y].GetHexArea(UnityEngine.Random.Range(0, 4));
            foreach (HexTile h in l)
            {
                if (UnityEngine.Random.Range(0, 20) < 10)
                {
                    D.ChangeOnsearch = true;
                    D.ChangeAll = false;
                    D.T = TerrainTypes.Desert;
                    D.OnlyChange = TerrainTypes.Plains;
                    D.runX(D.map[(int)h.hexTransform.RowColumn.x, (int)h.hexTransform.RowColumn.y], 4);
                }
            }
        }
    }
     
    void FillPlains()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                if (Wmap[i, b] == 3)
                {
                    D.map[i, b].cost = 1000;
                }
            }
        }

        D.DontChange = TerrainTypes.Coast;
        D.ChangeOnsearch = true;
        D.OnlyChange = TerrainTypes.Sea;
        D.T = TerrainTypes.Plains;
        D.run(D.map[H / 2, W / 2], D.map[W - 1, H - 1]);
        MapGenerator.Map[H / 2, W / 2].TerrainType = TerrainTypes.Plains;
        MapGenerator.Map[H / 2, W / 2].SetTexture(TerrainTypes.Plains);

    }

    bool Surrounded(HexTile h, TerrainTypes x)
    {
        var l = h.GetHexRing(2);

        bool Surrounded = true;
        foreach (HexTile H1 in l)
        {
            if (H1.TerrainType != x)
            {
                Surrounded = false;
            }
        }
        return Surrounded;
    }

}

public class DPathfinding : MonoBehaviour
{
    public class cord
    {
        public int X, Y;
        public int cost;
        public List<cord> nabours = new List<cord>();
        public int CostSoFar;
        public cord root;
    }

    public List<cord> Open = new List<cord>();
    public List<cord> Closed = new List<cord>();

    public cord Start;
    public cord Target;
    public cord[,] map;

    public TerrainTypes DontChange = TerrainTypes.Coast;
    public TerrainTypes OnlyChange = TerrainTypes.Coast;
    public TerrainTypes T = TerrainTypes.Coast;
    public bool ChangeTerrainOnTrail = true;
    public bool ChangeOnsearch = false;
    public bool ChangeAll = false;

    int H, W;

    public void setup()
    {
        W = MapGenerator.Map.GetLength(0);
        H = MapGenerator.Map.GetLength(1);

        map = new cord[H, W];

        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                map[i, b] = new cord();
                map[i, b].cost = UnityEngine.Random.Range(1, 20);
                map[i, b].X = i;
                map[i, b].Y = b;
            }
        }


        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {

                try
                {
                    map[i, b].nabours.Add(map[i - 1, b]);
                }
                catch { };
                try
                {
                    map[i, b].nabours.Add(map[i + 1, b]);
                }
                catch { };
                //right
                try
                {
                    map[i, b].nabours.Add(map[i, b + 1]);
                }
                catch { };
                //left
                try
                {
                    map[i, b].nabours.Add(map[i, b - 1]);
                }
                catch { };


            }
        }

        //Start = map[50, 50];
        //Start = map[map.GetLength(]
        //Target = map[90, 90];

    }

    public void path(cord c)
    {
        if (c != Start)
        {
            //c.CostSoFar = 1000;
            if (ChangeTerrainOnTrail)
            {
                try
                {
                    MapGenerator.Map[c.X, c.Y].TerrainType = T;
                    MapGenerator.Map[c.X, c.Y].SetTexture(T);
                }
                catch { Debug.Log("ErrorPath"); }
            }
            path(c.root);
        }

    }
    void Startcord(cord c)
    {
        c.CostSoFar = 0;
        Closed.Add(c);
    }
    void AddtoOpen(cord c)
    {

        if (!(c.cost == 1000))
        {
            foreach (cord cc in c.nabours)
            {

                if (c.nabours.Count > 0)
                {

                    if (!Open.Contains(cc) && !Closed.Contains(cc))
                    {
                        Open.Add(cc);
                        cc.CostSoFar = c.CostSoFar + cc.cost;
                        cc.root = c;


                        if (cc != Target)
                        {
                            if (ChangeOnsearch)
                            {
                                if (!(MapGenerator.Map[cc.X, cc.Y].TerrainType == DontChange))
                                {
                                    if (MapGenerator.Map[cc.X, cc.Y].TerrainType == OnlyChange || ChangeAll)
                                    {
                                        MapGenerator.Map[cc.X, cc.Y].TerrainType = T;
                                        MapGenerator.Map[cc.X, cc.Y].SetTexture(T);
                                    }
                                }
                            }
                        }
                    }
                    else if ((cc.CostSoFar) > (c.CostSoFar + cc.cost))
                    {
                        cc.CostSoFar = c.CostSoFar + cc.cost;
                        cc.root = c;
                        if (Closed.Contains(c))
                        {
                            Closed.Remove(cc);
                            Open.Add(cc);
                            if (cc != Target)
                            {
                                // MapGenerator.Map[cc.X, cc.Y].SetTexture(TerrainTypes.D);
                            }
                        }
                    }
                }

            }
        }


    }
    void AddtoClose(cord c)
    {
        Open.Remove(c);
        Closed.Add(c);
    }
    cord Getlowest()
    {
        if (Open.Count != 0)
        {
            cord returncord = Open[0];
            float low = returncord.CostSoFar;

            if (Open.Count > 1)
            {
                for (int i = 1; i < Open.Count; ++i)
                {
                    if (Open[i].CostSoFar < low)
                    {
                        returncord = Open[i];
                        low = returncord.CostSoFar;
                    }
                }

            }

            return returncord;
        }
        else
        {
            return null;
        }

    }
    private void Reset()
    {
        for (int i = 0; i < W; i++)
        {
            for (int b = 0; b < H; b++)
            {
                map[i, b].cost = UnityEngine.Random.Range(1, 20);
                map[i, b].CostSoFar = 0;
                map[i, b].root = null;
            }
        }
    }
    bool IsTarget(List<cord> t, cord c)
    {
        foreach (cord target in t)
        {
            if (c == target)
            {
                Target = target;
                return true;
            }
        }
        return false;
    }

    public void run(cord s, cord t)
    {

        Open.Clear();
        Closed.Clear();

        Start = s;
        Startcord(s);
        Target = t;




        AddtoOpen(Start);
        bool NoPath = false;
        while (true)
        {
            cord c = Getlowest();
            if (c != Target && c != null)
            {
                AddtoOpen(c);
                AddtoClose(c);
            }
            else if (c == null)
            {
                NoPath = true;
                Debug.Log("NoPathFound");
                break;
            }
            else
            {
                //MapGenerator.Map[c.X, c.Y].SetTexture(TerrainTypes.Coast);
                Debug.Log("done");
                path(Target);
                break;
            }


        }


    }
    public void runX(cord s, int x)
    {

        Open.Clear();
        Closed.Clear();

        Start = s;
        Startcord(s);
        Target = null;

        AddtoOpen(Start);
        bool NoPath = false;
        for (int i = 0; i < x; i++)
        {
            cord c = Getlowest();
            if (c != Target)
            {
                AddtoOpen(c);
                AddtoClose(c);
            }
            else
            {
                //MapGenerator.Map[c.X, c.Y].SetTexture(TerrainTypes.Coast);
                Debug.Log("done");
                path(Target);
                break;
            }


        }


    }
    public void runToHit(cord s, List<cord> Tlist)
    {
        Reset();
        Open.Clear();
        Closed.Clear();

        Start = s;
        Startcord(s);
        List<cord> targets = Tlist;
        //Target = t;




        AddtoOpen(Start);
        bool NoPath = false;
        while (true)
        {
            cord c = Getlowest();
            if (!IsTarget(Tlist, c) && c != null)
            {
                AddtoOpen(c);
                AddtoClose(c);
            }
            else if (c == null)
            {
                NoPath = true;
                Debug.Log("NoPathFound");
                break;
            }
            else
            {
                //MapGenerator.Map[c.X, c.Y].SetTexture(TerrainTypes.Coast);

                path(Target);
                break;
            }

        }
    }
    public IEnumerator Run()
    {
        Startcord(Start);
        AddtoOpen(Start);
        yield return null;

        bool ReachedDestination = false;
        bool noValidPath = false;

        HexTile DestinaitionReached = null;

        while (ReachedDestination == false)
        {
            cord c = Getlowest();
            if (c != Target)
            {
                AddtoOpen(c);
                AddtoClose(c);
            }
            else if (c == Target)
            {
                foreach (cord b in Open)
                {
                    if (b == Target)
                    {
                        Debug.Log("1done");
                        break;

                    }

                }
                Debug.Log("2done");
                break;

            }
            else
            {
                Debug.Log("3done");
            }

            yield return null;
        }



    }

}