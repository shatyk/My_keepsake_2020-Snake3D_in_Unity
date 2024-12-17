// "Converted" to Unity by http://unitycoder.com/blog/
// Original source: http://roy-t.nl/index.php/2011/09/24/another-faster-version-of-a-2d3d-in-c/

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.PlayerLoop;

//using Microsoft.Xna.Framework;

namespace Tests
{
    /// <summary>
    /// Author: Roy Triesscheijn (http://www.roy-t.nl)
    /// Small program that demonstrates the use of the PathFinder class
    /// </summary>
    class Program : MonoBehaviour
    {
        [SerializeField] private SnakeController snakeController;
        [SerializeField] private SnakeController_AI snakeController_AI;
        [SerializeField] private ManagerGame managerGame;

        public World world;

        private List<GameObject> blocksObj;

        public Vector3 target;
        public Vector3 startPos;
		//private UnityEngine.Texture2D tex;

        void Start()
        {
            target = new Vector3(6,6,6);
            startPos = new Vector3(0,0,0);
        }


		/// <summary>
		/// Small benchmark that creates a 10x10x10 world and then tries to find a path from
		/// 0,0,0 to 9,5,8. Benchmark is run 100 times after which benchmark results and the path are shown        
		/// 1 out 3 nodes in the world are blocked
		/// </summary>        
		//public static void Main(string[] args)
		public Vector3[] Main(Texture2D tex, Transform prefab, LineRenderer lineRenderer)
        {

            double[] bench = new double[1000];            
            world = new World(7, 7, 7);
			
			Vector3 offset = new Vector3(0.5f,0.5f,0.5f);
			offset = Vector3.zero;

			// create level
			//System.Random random = new System.Random();
            var partsSnakeList = new List<PartSnakeLogic>();
            partsSnakeList.AddRange(snakeController.partsSnakeList);
            partsSnakeList.AddRange(snakeController_AI.partsSnakeList);

            world = new World(7,7,7);
            if (blocksObj != null)
            {
                foreach (var block in blocksObj)
                {
					Destroy(block);
                }

			}
            blocksObj = new List<GameObject>();

			foreach (var part in partsSnakeList)
            {
                var x = (int)part.transform.position.x + managerGame.sizeMapOffset.X;
                var y = (int)part.transform.position.y + managerGame.sizeMapOffset.Y;
                var z = (int)part.transform.position.z + managerGame.sizeMapOffset.Z;

                world.MarkPosition(new Point3D(x, y, z), true);
                blocksObj.Add(Instantiate(prefab, new Vector3(x, y, z) + offset, Quaternion.identity).gameObject);
			}

      //      for (int x = 0; x < world.Right; x++)
      //      {
      //          for (int y = 0; y < world.Top; y++)
      //          {
      //              for (int z = 0; z < world.Back; z++)
      //              {
      //                  //prevent the starting square from being blocked
      //                  //if ((x + y + z) % 3 == 0 && (x + y + z) != 0)
      //                  if ((UnityEngine.Random.value>0.70f) || (z==5 && UnityEngine.Random.value>0.30f))
      //                  {
						//	//if (z==0)
						//	//{
								
						//		world.MarkPosition(new Point3D(x, y, z), true);
						//		//tex.SetPixel(x,y,Color.gray);
						//		Instantiate(prefab,new Vector3(x,y,z)+offset,Quaternion.identity);
						//	//}
      //                  }else{
						//	//tex.SetPixel(x,y,Color.white);
						//}
      //              }
      //          }
      //      }
/*
            for (int i = 0; i < bench.Length; i++)
            {
                DateTime start = DateTime.Now;
                PathFinder.FindPath(world, Point3D.Zero, new Point3D(9, 9, 0));
                TimeSpan ts = DateTime.Now - start;
                bench[i] = ts.TotalMilliseconds;
			}

			*/
			/*
			UnityEngine.Debug.Log(1);
            UnityEngine.Debug.Log("Total time: " + bench.Sum().ToString() + "ms");
            UnityEngine.Debug.Log("Average time: " + bench.Sum() / bench.Length + "ms");
            UnityEngine.Debug.Log("Max: " + bench.Max() + "ms");
            UnityEngine.Debug.Log("Min: " + bench.Min() + "ms");
*/
//            UnityEngine.Debug.Log("Output: ");
            SearchNode crumb2 = PathFinder.FindPath( world, new Point3D((int)startPos.x, (int)startPos.y, (int)startPos.z), new Point3D((int)target.x, (int)target.y, (int)target.z));                                   
			
        //    Console.Out.WriteLine("Start: " + crumb2.position.ToString());
			// get start pos
            //UnityEngine.Debug.Log("Start: " + crumb2.position.ToString());
			//tex.SetPixel(0,0,Color.blue);
			
			//List arr = new List
			List<Vector3> myroute = new List<Vector3>();
			
			int verts = 1;
			lineRenderer.SetVertexCount(verts);
			
			//offset = Vector3.zero;
			offset = new Vector3(0.5f,0.5f,0.5f);
			lineRenderer.SetPosition(verts-1, startPos);
            //myroute.Add(startPos + offset);

            do
            {
                //string p = crumb2.next.position.ToString();
                int[] p = crumb2.next.position.ToArray();
                Debug.Log("x:" + p[0] + " y:" + p[1] + " z:" + p[2]);
                //UnityEngine.Debug.Log("Route: " + p);
                crumb2 = crumb2.next;
                int xx = p[0];
                int yy = p[1];
                int zz = p[2];
                //if (zz==0)	tex.SetPixel(xx,yy,Color.green);
                verts++;
                lineRenderer.SetVertexCount(verts);
                lineRenderer.SetPosition(verts - 1, new Vector3(xx, yy, zz) + offset * 0.5f);
                myroute.Add(new Vector3(xx, yy, zz) + offset);
            } while (crumb2.next != null);


			//UnityEngine.Debug.Log("Finished at: " + crumb2.position.ToString());            
			int[] p2 = crumb2.position.ToArray();
			int xx2=p2[0];
			int yy2=p2[1];
			int zz2=p2[2];
			//tex.SetPixel(xx2,yy2,Color.red);
			
			verts++;
			lineRenderer.SetVertexCount(verts);
			lineRenderer.SetPosition(verts-1, new Vector3(xx2,yy2,zz2)+offset);
			myroute.Add(new Vector3(xx2,yy2,zz2)+offset);			
			myroute.Add(new Vector3(xx2,yy2,zz2)+offset*2);		// extra	
			myroute.Add(new Vector3(xx2,yy2,zz2)+offset*3);		// extra	
			
			// we are done..now fly the route?
			
			
			//tex.Apply(false);
            //Console.ReadLine();
			
			return myroute.ToArray();
			
        }

        void Update()
        {

        }

    }
}
