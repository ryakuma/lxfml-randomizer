using System;
namespace lxfml_randomizer
{
	class randomizer
	{
		static void Main(string[] args)
		{
			//reading block amount and field size from user
			Console.WriteLine("How many blocks do you want in your file?\nkeep it under 5000 if you care about loading times");
			int blockamount = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("How big should be the field in which blocks are placed?\nmax X&Z is input/2, min X&Z is -(input/2)");
			int blockposmax = Convert.ToInt32(Console.ReadLine());

			//declaring some variables used later
			int i, i2;
			decimal xpos, ypos, zpos;
			decimal[] xposlast = new decimal[blockamount],
			yposlast = new decimal[blockamount],
			zposlast = new decimal[blockamount];
			string brick;
			string bricks_file = "";

			//DD units, do not change unless you know why you're doing it
			decimal horizontalunit = 0.8M;
			decimal verticalunit = 0.96M;

			blockposmax = blockposmax/2;
			int blockposmin = -blockposmax;
			//int blockheightmax = 3; //not used for now

			//bricks | b - brick, 1x1 - 1 by 1 block, etc., value is an ID of that brick
			string b1x1 = "3005";
			string b1x2 = "3004";
			string b1x3 = "3622";
			string b1x4 = "3010";
			string b1x6 = "3009";
			string b2x2 = "3003";
			string b2x3 = "3002";
			string b2x4 = "3001";
			string[] bricks={b1x1, b1x2, b1x3, b1x4, b1x6, b2x2, b2x3, b2x4}; // array with all the bricks to randomize from

			//materials | m - material, name is color name, value is an ID of that material
			string mwhite = "1";
			string mblack = "26";
			string mdarkGray = "199";
			string mlightGray = "194";
			string mred = "21";
			string myellow = "24";
			string mgreen = "28";
			string mblue = "23";
			string[] materials = {mwhite, mblack, mdarkGray, mlightGray, mred, myellow, mgreen, mblue}; //array with all the materials to randomize from

			//how should generated file start and end, stored in txt files so you can change it
			string documentstart = System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\documentstart.txt");
			string documentend = System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\documentend.txt");

			Console.WriteLine("lxfml-randomizer\nPlease be patient...");
			var r = new Random();

			//add a new block until there are blockamount blocks in the file
			for(i=0; i < blockamount; i++){
				int refid = i;
				xpos = r.Next(blockposmin, blockposmax);
				zpos = r.Next(blockposmin, blockposmax);
				ypos = 0;

				//converting horizontal block positions to DD units and making blocks stick to DD's mesh
				xpos = (xpos*horizontalunit)-0.4M;
				zpos = (zpos*horizontalunit)-0.4M;

				//increase height of a block if it's origin is the same as any of the previous blocks
				for(i2=0; i2 < i; i2++){
					if (xposlast[i2] == xpos && (zposlast[i2] == zpos))
					ypos++;
				}

				//convert block height to DD units
				ypos = ypos*verticalunit;

				//save current block origin to an array, used in for loop a bit up
				xposlast[i] = xpos;
				zposlast[i] = zpos;
				yposlast[i] = ypos;

				//max random value is length of arrays which contain bricks and materials
				int bricksmaxrand = bricks.Length;
				int materialsmaxrand = materials.Length;

				//select random brick and material
				string designid = bricks[r.Next(0, bricksmaxrand)];
				string material = materials[r.Next(0, materialsmaxrand)];

				//load brick template and insert current brick values
				brick = (System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\bricktemplate.txt")).Replace("!REFID", refid.ToString()).
				Replace("!DESIGNID", designid).
				Replace("!MATERIALS", material).
				Replace("!XPOS", (xpos.ToString()).Replace(",",".")).
				Replace("!YPOS", (ypos.ToString()).Replace(",",".")).
				Replace("!ZPOS", (zpos.ToString()).Replace(",","."));

				//keep current bricks in memory
				bricks_file = bricks_file+brick;
				Console.WriteLine("Block no. " + (i+1) + " generated.	|	" + DateTime.Now.TimeOfDay);
			}
			string fileoutput = documentstart + bricks_file + documentend; //whole output to a single variable
			System.IO.File.WriteAllText(@".\randomized.lxfml", fileoutput); //output to a file
			Console.WriteLine("\n============================\nFile generated!	|	" + DateTime.Now.TimeOfDay);
			System.Threading.Thread.Sleep(1000);
		}
	}
}
