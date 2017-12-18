using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWAshingText
{
    class Program
    {
        static void Main(string[] args)
        {
            string cwd      = System.AppDomain.CurrentDomain.BaseDirectory;
            string filename = "DM Database V101 Prospects______QL_PROSPECT";

            string file     = cwd + "\\data\\"+filename+".txt";
            string output   = cwd + "\\output\\"+filename+".output.txt";
            string dupOut   = cwd + "\\output\\"+filename+".duplicate.txt";



            string sup = cwd + "\\SUPRESSION_FILE_26_04-16.txt";

            var lines       = File.ReadLines(file);
            var suplines    = File.ReadLines(sup);
            StreamWriter dw = new StreamWriter(dupOut, false);

            using (var tw = new StreamWriter(output, false))
            {
                int ctr = 0;
                int found = 0;
                Dictionary<string, string> dupFirst   = new Dictionary<string, string>();
                Dictionary<string, string> dupLast    = new Dictionary<string, string>();
                Dictionary<string, string> dupAddress = new Dictionary<string, string>();
                Dictionary<string, string> dupName    = new Dictionary<string, string>();


                foreach (var line in lines)
                {
                    string[] cline = line.Split('\t');
                    string name = cline[3].Trim().ToUpper();                
                    string first = cline[13].Trim().ToUpper();
                    string last  = cline[14].Trim().ToUpper();
                    string add   = cline[4].Trim().ToUpper();

                    bool flag = false;
                    foreach (var line2 in suplines)
                    {
                        string[] sline   = line2.Split('\t');
                        string sup_name = sline[6].Trim().ToUpper();                    
                        string sup_add   = sline[7].Trim().ToUpper();
                        string sup_first = sline[16].Trim().ToUpper();
                        string sup_last  = sline[17].Trim().ToUpper();


                        if (ctr==0) {
                            Console.WriteLine("{0} == {1}", "FILE", "SUPP");
                            Console.WriteLine("{0} == {1}", name, sup_name);
                            Console.WriteLine("{0} == {1}", first, sup_first);
                            Console.WriteLine("{0} == {1}", last, sup_last);
                            Console.WriteLine("{0} == {1}", add, sup_add);

                            Console.ReadKey();
                            break;
                        }

                        if (first == sup_first && last == sup_last && add == sup_add && name==sup_name)
                        {
                            found++;
                            flag = true;
                            break;

                        }
                    }
                    if (dupFirst.ContainsKey(first) && dupLast.ContainsKey(last) && dupAddress.ContainsKey(add) && dupName.ContainsKey(name))
                    {
                        string val = line;
                        dw.WriteLine(val);
                        flag = true;
                    }


                    if (flag) continue;
                    string val2 = line;
                    tw.WriteLine(val2);

                    dupFirst[first] = "1";
                    dupFirst[last] = "1";
                    dupFirst[add] = "1";
                    dupName[name] = "1";

                    
                    ctr++;
                    Console.Write("\rProcessing {0}",ctr);

                }

                Console.WriteLine("\nFinished: {0} found. ",found);
                Console.ReadKey();
            }

            dw.Close();
        }
    }
}
