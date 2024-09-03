using System.ComponentModel;
using System.Diagnostics;

namespace Ceros_Polinomios
{
    public partial class Form1 : Form
    {
        // Properties
        TextBox[] Coeff {  get; set; }              // TextBox de coeficientes 
        TextBox[] Interval { get; set; }            // TextBox de intervalos
        List<double> Zeros { get; set; }            // Listado de las raíces del polinomio
        List<double> Critical_points {  get; set; } // Listado de los puntos críticos del polinomio
        Label[] Zeros_labels {  get; set; }         // Etiquetas para imprimir las raíces
        Label[] First_order_labels { get; set; }    // Etiquetas para imprimir los puntos críticos

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
        public double Evaluate(double[] poly, double num)
        {
            double result = 0;
            for (int i = 0; i < poly.Length; i++)
                result += (double)poly[i] * Math.Pow(num, poly.Length - 1 - i);

            return result;
        }                    // Evalúa un numero dado en el especificado polinomio
        public void Bisection(double[] poly, double inf, double sup, List<double> zero)
        {
            if (sup - inf < 0000.1)
            {
                double zeroCurrent = Math.Round((inf + sup) / 2, 4);
                if (Math.Round(zeroCurrent) - zeroCurrent < 0.1)
                    zero.Add(Math.Round(zeroCurrent));
                else
                    zero.Add(Math.Round(zeroCurrent, 4));
            }

            else
            {
                double medium = Math.Round((inf + sup) / 2, 5);

                if (Bolzano(poly, inf, medium))
                    Bisection(poly, inf, medium, zero);

                if (Bolzano(poly, medium, sup))
                    Bisection(poly, medium, sup, zero);
            }
        }    // Método principal 
        public bool Bolzano(double[] pol, double inf, double sup)
        {
            double a_evaluated = Evaluate(pol, inf);
            double half = (inf + sup) / 2;
            double halfCurrent = half;

            while (sup >= half || halfCurrent >= inf)
            {
                if (sup >= inf)
                {
                    if (a_evaluated * Evaluate(pol, sup) <= 0)
                        return true;
                    else
                        sup -= 0.00002;
                }

                if (halfCurrent >= inf)
                {
                    if (a_evaluated * Evaluate(pol, halfCurrent) <= 0)
                        return true;
                    else
                        halfCurrent -= 0.00002;
                }
            }
            return false;
        }            // Verifica si existe raíces en el intervalo dado
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
        }          // Chequea los valores de entrada
        public bool IsDigit(string num)
        {
            if (string.IsNullOrWhiteSpace(num))
                return false;

            return num.All(c => c == '-' || Char.IsDigit(c));
        }            // Chequea los valores de entrada (si es dígito o no)
        public double[] GetPoly(TextBox[] coeff)
        {
            double[] poly = new double[4];

            for (int i = 0; i < coeff.Length; i++)
                poly[i] = Convert.ToDouble(coeff[i].Text);

            return poly;
        }   // Retorna un arreglo del polinomio dado los TexBox (coeficientes)
        public void PrintZeros()
        {
            for (int i = 0; i < Zeros.Count; i++)
                Zeros_labels[i].Text = Convert.ToString(Zeros[i]);
        }                   // Imprime las raíces en las etiquetas
        public void PrintCriticalPoints(double[] first_order)
        {
            for (int i = 0; i < Critical_points.Count; i++) 
                First_order_labels[i].Text = Convert.ToString(Critical_points[i]) +" "+ MaxOrMin(first_order, Critical_points[i]);
        }   // Imprime los puntos críticos en las etiquetas
        public double[] GetFirstOrder(double[] poly)
        {
            double[] first_order = new double[4];
            first_order[0] = 0;

            for (int i = 0; i < poly.Length - 1; i++)
                first_order[i + 1] = poly[i] * (poly.Length - (1+i));

            return first_order;
        }            // Retorna la primera derivada del polinomio especificado
        public string MaxOrMin(double[] first_order, double critical_point)
        {
            double left = Evaluate(first_order, critical_point - 0.5);
            double right = Evaluate(first_order, critical_point + 0.5);

            if (left > 0 && right < 0)
                return "(Max)";

            else if (left < 0 && right > 0)
                return "(Min)";

            else if (left * right > 0)
                return "(Const)";

            return "";
        }  // Determina si un valor dado es máximo, mínimo o constante
        public void ClearEverything()
        {
            Zeros.Clear();
            Critical_points.Clear();

            foreach (Label item in Zeros_labels)
                item.Text = "";

            foreach (Label item in First_order_labels)
                item.Text = "";
        }              // Restaura todos los valores y etiquetas

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

                Bisection(poly, intvl1, intvl2, Zeros);
                Bisection(first_order, intvl1, intvl2, Critical_points);

                PrintZeros();
                PrintCriticalPoints(first_order);
            }
        }  // LLeva a cabo todo el proceso

        // Labels_Click
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox1
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox2
        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox3
        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox4
        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox5
        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Clear(); ClearEverything();
        } // Acciona al cliquear el TextBox6
    }
}
