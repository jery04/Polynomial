using System.ComponentModel;
using System.Diagnostics;

namespace Ceros_Polinomios
{

    public partial class Form1 : Form
    {
        // Properties
        TextBox[] Coeff {  get; set; }
        TextBox[] Interval { get; set; }
        List<double> Zeros { get; set; }
        List<double> Critical_points {  get; set; }
        Label[] Zeros_labels {  get; set; }
        Label[] First_order_labels { get; set; }

        public Form1()
        {
            InitializeComponent();
            this.Coeff = new TextBox[4]
            {
                textBox1, textBox2, textBox3, textBox4,
            };
            this.Interval = new TextBox[2]
            {
                textBox5, textBox6
            };
            this.Zeros_labels = new Label[3]
            {
                label16, label17, label18
            };
            this.First_order_labels = new Label[2]
            {
                label19, label20
            };
            this.Critical_points = new List<double>();
            this.Zeros = new List<double>();
        }

        // Methods
        public double Evaluar(double[] poly, double num)
        {
            double result = 0;
            for (int i = 0; i < poly.Length; i++)
                result += (double)poly[i] * Math.Pow(num, poly.Length - 1 - i);

            return result;
        }
        public void Bisec(double[] poly, double a, double b, List<double> zero)
        {
            if (b - a < 0000.1)
            {
                double zeroCurrent = Math.Round((a + b) / 2, 4);
                if (Math.Round(zeroCurrent) - zeroCurrent < 0.1)
                    zero.Add(Math.Round(zeroCurrent));
                else
                    zero.Add(Math.Round(zeroCurrent, 4));
            }

            else
            {
                double medium = Math.Round((a + b) / 2, 5);

                if (Bolzano(poly, a, medium))
                    Bisec(poly, a, medium, zero);

                if (Bolzano(poly, medium, b))
                    Bisec(poly, medium, b, zero);
            }
        }
        public bool Bolzano(double[] pol, double inf, double sup)
        {
            double a_evaluated = Evaluar(pol, inf);
            double half = (inf + sup) / 2;
            double halfCurrent = half;

            while (sup >= half || halfCurrent >= inf)
            {
                if (sup >= inf)
                {
                    if (a_evaluated * Evaluar(pol, sup) <= 0)
                        return true;
                    else
                        sup -= 0.00002;
                }

                if (halfCurrent >= inf)
                {
                    if (a_evaluated * Evaluar(pol, halfCurrent) <= 0)
                        return true;
                    else
                        halfCurrent -= 0.00002;
                }
            }
            return false;
        }
        public bool CheckInput(TextBox[] coeff, TextBox[] interval)
        {
            bool check = true;

            foreach (TextBox input in coeff)
            {
                if (!IsDigit(input.Text))
                { input.BackColor = Color.Red; check = false; }
                else 
                    input.BackColor = Color.White;
            }
            foreach (TextBox input in interval)
            {
                if (!IsDigit(input.Text))
                { input.BackColor = Color.Red; check = false; }
                else 
                    input.BackColor = Color.White;
            }
            return check;
        }
        public bool IsDigit(string num)
        {
            if (string.IsNullOrWhiteSpace(num))
                return false;

            return num.All(c => c == '-' || Char.IsDigit(c));
        }
        public double[] GetPoly(TextBox[] coeff)
        {
            double[] poly = new double[4];

            for (int i = 0; i < coeff.Length; i++)
                poly[i] = Convert.ToDouble(coeff[i].Text);

            return poly;
        }
        public void PrintZeros()
        {
            for (int i = 0; i < Zeros.Count; i++)
                Zeros_labels[i].Text = Convert.ToString(Zeros[i]);
        }
        public void PrintCriticalPoints(double[] first_order)
        {
            for (int i = 0; i < Critical_points.Count; i++) 
                First_order_labels[i].Text = Convert.ToString(Critical_points[i]) +" "+ MaxOrMin(first_order, Critical_points[i]);
        }
        public double[] GetFirstOrder(double[] poly)
        {
            double[] first_order = new double[4];
            first_order[0] = 0;

            for (int i = 0; i < poly.Length - 1; i++)
                first_order[i + 1] = poly[i] * (poly.Length - (1+i));

            return first_order;
        } 
        public string MaxOrMin(double[] first_order, double x)
        {
            double left = Evaluar(first_order, x - 0.5);
            double right = Evaluar(first_order, x + 0.5);

            if (left > 0 && right < 0)
                return "(Max)";

            else if (left < 0 && right > 0)
                return "(Min)";

            else if (left * right > 0)
                return "(Const)";

            return "";
        }
        public void ClearEverything()
        {
            Zeros.Clear();
            Critical_points.Clear();

            foreach (Label item in Zeros_labels)
                item.Text = "";

            foreach (Label item in First_order_labels)
                item.Text = "";
        }

        // Botton_Calculate
        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckInput(Coeff, Interval))
            {
                ClearEverything();
                double intvl1 = Convert.ToDouble(Interval[0].Text);
                double intvl2 = Convert.ToDouble(Interval[1].Text);
                double[] poly = GetPoly(Coeff);
                double[] first_order = GetFirstOrder(poly);

                Bisec(poly, intvl1, intvl2, Zeros);
                Bisec(first_order, intvl1, intvl2, Critical_points);

                PrintZeros();
                PrintCriticalPoints(first_order);
            }
        }

        // Labels
        // Click
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); ClearEverything();
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); ClearEverything();
        }
        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear(); ClearEverything();
        }
        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear(); ClearEverything();
        }
        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear(); ClearEverything();
        }
        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Clear(); ClearEverything();
        }
    }
}
