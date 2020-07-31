using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snow
{
    public class Lira
    {
        private string text;
        private int one, two, three;
        private double Cr1;

        private List<int[]> elements = new List<int[]>();
        private List<double[]> points = new List<double[]>();
        private List<int[]> document6 = new List<int[]>();
        private List<double[]> document7 = new List<double[]>();

        public double fd { get; set; }
        public double Ce { get; set; }
        public double Ct { get; set; }
        public double Sg { get; set; }
        public double R { get; set; }
        public double Gamma { get; set; }
        public int Variant { get; set; }
        public int Number { get; set; }
        public double[] Center { get; set; }
        public int[] Axes { get { return new int[] { one, two, three }; } set { one = value[0]; two = value[1]; three = value[2]; } }

        public Lira(string path)
        {
            text = File.ReadAllText(path, Encoding.Default);
            LoadElements();
            LoadGlobalPoints();
            LoadDocument6();
            LoadDocument7();
            Center = new double[3];
        }

        private string GetDocument(int number)
        {
            int a = text.IndexOf("( " + number + "/") + 6;
            int b = text.IndexOf(")", a) - 3;
            return text.Substring(a, b - a);
        }

        private void LoadElements()
        {
            int num = 0;
            string temp = GetDocument(1).Replace("\n", "").Replace("\r", "");
            foreach (string te in temp.Split(new string[] { " /" }, StringSplitOptions.RemoveEmptyEntries))
                elements.Add(Array.ConvertAll((++num + " " + te).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), x => Convert.ToInt32(x)));
        }

        private void LoadGlobalPoints()
        {
            string temp = GetDocument(4).Replace("\n", "").Replace("\r", "");
            foreach (string te in temp.Split(new string[] { " /" }, StringSplitOptions.RemoveEmptyEntries))
                points.Add(Array.ConvertAll(te.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), x => Convert.ToDouble(x)));
        }

        private void LoadDocument6()
        {
            string temp = GetDocument(6).Replace("\n", "").Replace("\r", "");
            foreach (string te in temp.Split(new string[] { " /" }, StringSplitOptions.RemoveEmptyEntries))
                document6.Add(Array.ConvertAll(te.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), x => Convert.ToInt32(x)));
        }

        private void LoadDocument7()
        {
            string temp = GetDocument(7).Replace("\n", "").Replace("\r", "");
            foreach (string te in temp.Split(new string[] { " /" }, StringSplitOptions.RemoveEmptyEntries))
                document7.Add(Array.ConvertAll(te.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), x => Convert.ToDouble(x)));
        }

        public void Run(int filterByType, int filterByEI)
        {
            List<int[]> filteredElements = new List<int[]>();
            if (filterByType != 0)
                filteredElements.AddRange(elements.Where(x => x[1] == filterByType));
            if (filterByEI != 0)
                filteredElements.AddRange(elements.Where(x => x[2] == filterByEI));
            Cr1 = 2.55 - Math.Exp(0.8 - 14 * fd);

            foreach (var i in filteredElements)
            {
                AddForce(ForcesInElement(i), i[0]);
            };
        }

        private double[] ForcesInElement(int[] element)
        {
            double[] temp = new double[4];

            for (int j = 3; j < element.Length; j++)
                temp[j - 3] = ForceInPoint(element[j]);
            return temp;
        }

        private double ForceInPoint(int point)
        {
            double x = points[point - 1][one] - Center[one];
            double y = points[point - 1][two] - Center[two];
            double z = points[point - 1][three] - Center[three];
            double polar = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            R = Math.Sqrt(Math.Pow(polar, 2) + Math.Pow(z, 2));
            double alfa = Math.Asin(polar / R) * 180 / Math.PI;//Math.Acos(z / R) * 180 / Math.PI; ///sin([play_z)
            double beta = Math.Atan2(y, x) * 180 / Math.PI - Gamma;

            if (y < 0) beta += 360;
            beta -= Math.Truncate(beta / 360) * 360;
            if ((0 < beta && beta < 180 || Variant == 1) && alfa < 60) return Force(alfa, beta);
            else return 0;
        }

        public void Save(string Path)
        {
            string doc_6 = " ";
            string doc_7 = "";
            int i = 0;

            foreach (var item in document6)
            {
                i++;
                if (i < 4) doc_6 += String.Join(" ", Array.ConvertAll(item, x => x.ToString())) + " / ";
                else { doc_6 += String.Join(" ", Array.ConvertAll(item, x => x.ToString())) + " /\n "; i = 0; }
            }
            if (i != 0) doc_6 += "\n";

            foreach (var item in document7)
                doc_7 += String.Join(" ", Array.ConvertAll(item, x => x.ToString())) + " /\n";

            text = text.Replace(GetDocument(6), doc_6);
            text = text.Replace(GetDocument(7), doc_7);
            File.WriteAllText(Path, text, Encoding.Default);
        }

        private double AddForce(double[] value, int num_elem)
        {
            if (value.Count(x => x != 0) != 0)
            {
                double indexForce = GetIndexForce(value);
                if (indexForce < 0)
                {
                    indexForce = document7.Count + 1;
                    document7.Add(new double[] { indexForce }.Concat(value).ToArray());
                }
                document6.Add(new int[] { num_elem, 17, three + 1, (int)indexForce, Number });
            }
            return -1;
        }

        private double GetIndexForce(double[] value)
        {
            foreach (var i in document7)
            {
                if (i.Length - 1 == value.Length)
                    for (int j = 1; j < i.Length; j++)
                    {
                        if (!value[j - 1].Equals(i[j])) break;
                        if (j == i.Length - 1) return i[0];
                    }
            }
            return -1;
        }

        private double Force(double alfa, double beta)
        {
            double polar_z = R * Math.Sin(alfa * Math.PI / 180);
            double nu;
            double force;

            switch (Variant)
            {
                case 1:
                    if (alfa <= 30) nu = 1;
                    else nu = 2 - alfa / 30;
                    break;
                case 2:
                    if (alfa <= 30)
                        nu = Cr1 * Math.Pow(polar_z / (R * 0.5), 2) * Math.Sin(beta * Math.PI / 180);
                    else if (30 < alfa && alfa < 45)
                        nu = Cr1 * Math.Sin(beta * Math.PI / 180) * (3 - alfa / 15)
                            + 1.5 * Math.Sin(beta * Math.PI / 180) * (alfa / 15 - 2);
                    else nu = 1.5 * Math.Sin(beta * Math.PI / 180) * (4 - alfa / 15);
                    break;
                case 3:
                    nu = 3 * Math.Sqrt(2 * fd * Math.Sin(3 * alfa * Math.PI / 180) * Math.Sin(beta * Math.PI / 180));
                    break;
                default:
                    nu = 999;
                    break;
            }
            force = Math.Round(0.7 * Ce * Ct * nu * Sg, 3);
            return force;
        }
    }
}