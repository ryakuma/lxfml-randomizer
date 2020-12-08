using System;
namespace lxfml_randomizer
{
    class Program
    {
        static void Main(string[] args)
        {
						Console.WriteLine("Amount of blocks you want in your randomly generated structure \n(keep it under 10000 if you care about loading times)");
						int blockamount = Convert.ToInt32(Console.ReadLine());
						Console.WriteLine("How big should be the field in which blocks are placed?");
						int blockposmax = Convert.ToInt32(Console.ReadLine());

						int i, i2;
						decimal xpos, ypos, zpos;
						decimal[] xposlast = new decimal[blockamount], yposlast = new decimal[blockamount], zposlast = new decimal[blockamount];
						string brick;
						string bricks_file = "";

						decimal horizontalunit = 0.8M;
						decimal verticalunit = 0.96M;

						blockposmax = blockposmax/2;
						int blockposmin = -blockposmax;
						int blockheightmax = 3;

						string b1x1 = "3005";
						string b1x2 = "3004";
						string b1x3 = "3622";
						string b1x4 = "3010";
						string b1x6 = "3009";
						string b2x2 = "3003";
						string b2x3 = "3002";
						string b2x4 = "3001";
						string[] bricks={b1x1, b1x2, b1x3, b1x4, b1x6, b2x2, b2x3, b2x4};

						string mwhite = "1";
						string mblack = "26";
						string mdarkGray = "199";
						string mlightGray = "194";
						string mred = "21";
						string myellow = "24";
						string mgreen = "28";
						string mblue = "23";
						string[] materials = {mwhite, mblack, mdarkGray, mlightGray, mred, myellow, mgreen, mblue};

						string documentstart = System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\documentstart.txt");
						string documentend = System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\documentend.txt");

						Console.WriteLine("lxfml-randomizer\nPlease be patient...");
						var r = new Random();
						xposlast[0]=blockposmax+1;
						zposlast[0]=blockposmax+1;
						for(i=0; i < blockamount; i++){
							int refid = i;
							xpos = r.Next(blockposmin, blockposmax);
							zpos = r.Next(blockposmin, blockposmax);
							ypos = 0;//r.Next(0, blockheightmax);

							xpos = (xpos*horizontalunit)-0.4M;
							zpos = (zpos*horizontalunit)-0.4M;

							for(i2=0; i2 < i; i2++){
								if (xposlast[i2] == xpos && (zposlast[i2] == zpos))
								ypos++;
							}

							ypos = ypos*verticalunit;

							//if (xpos == xposlast && zpos == zposlast) ypos+=verticalunit;

							xposlast[i] = xpos;
							zposlast[i] = zpos;
							yposlast[i] = ypos;

							int bricksmaxrand = bricks.Length;
							int materialsmaxrand = materials.Length;
							string designid = bricks[r.Next(0, bricksmaxrand)];
							string material = materials[r.Next(0, materialsmaxrand)];
							brick = (System.IO.File.ReadAllText(@".\lxfml-randomizer\lxfml-randomizer\data\bricktemplate.txt")).Replace("!REFID", refid.ToString()).
							Replace("!DESIGNID", designid).
							Replace("!MATERIALS", material).
							Replace("!XPOS", (xpos.ToString()).Replace(",",".")).
							Replace("!YPOS", (ypos.ToString()).Replace(",",".")).
							Replace("!ZPOS", (zpos.ToString()).Replace(",","."));
							bricks_file = bricks_file+brick;
							Console.WriteLine("Block no. " + (i+1) + " generated.	|	" + DateTime.Now.TimeOfDay);
						}
						string fileoutput = documentstart + bricks_file + documentend;
						System.IO.File.WriteAllText(@".\randomized.lxfml", fileoutput);
						Console.WriteLine("\n============================\nFile generated!	|	" + DateTime.Now.TimeOfDay);
						System.Threading.Thread.Sleep(1000);
        }
    }
}
