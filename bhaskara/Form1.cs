using System;
using System.Linq;
using System.Windows.Forms;

namespace bhaskara {

    public partial class Form1: Form {

        public Form1() {
            InitializeComponent();
            LimpaTexto();
        }

        public void LimpaTexto() {
            lblX1Result.Text = "";
            lblX2Result.Text = "";
            lblAResult.Text = "";
            lblBResult.Text = "";
            lblCResult.Text = "";
            lblRaizesReaisResult.Text = "";
            lblXVResult.Text = "";
            lblYVResult.Text = "";
        }

        private void btnCalcular_Click(object sender, EventArgs e) {
            parseJenifer();
            //parseGiovano();
        }

        private void parseJenifer() {
            string equacaoDigitada = tbEquacao.Text.ToLower().Trim();

            // ax² + bx + c = 0
            // varrer a string
            // a - se o próximo char for x (precisa ser x), os anteriores são numeros
            // a -  pode ser x² e x^2
            // o bloco b começa se o proximo digito for um sinal (+ ou -),
            // um for para cada var?
            // padrões repetidos para b e c
            // c acaba com o sinal de =
            // o último digito deve ser 0

            equacaoDigitada = equacaoDigitada.Replace(" ", "");

            if (equacaoDigitada[0] == 'x') {
                equacaoDigitada = "1" + equacaoDigitada;
            }

            string numeroA = "";
            string numeroB = "";
            string numeroC = "";

            string pedacoA = "";
            string pedacoB = "";
            string pedacoC = "";

            int i = 0;
            bool potencia = false;
            for (i = 0; i < equacaoDigitada.Length; i++) {
                pedacoA += equacaoDigitada[i].ToString();
                if (equacaoDigitada[i] == '^') {
                    potencia = true;
                }
                if (equacaoDigitada[i] != '-' && equacaoDigitada[i] != '+' && equacaoDigitada[i] != '=') {
                    if (Char.IsNumber(equacaoDigitada[i])) {
                        if (equacaoDigitada[i] != '²' && !potencia) {
                            numeroA = numeroA + equacaoDigitada[i];
                        }
                    }
                } else {
                    if (i == 0) {
                        numeroA = numeroA + equacaoDigitada[i];
                    } else {
                        if (numeroA == "-" || numeroA == "+") {
                            numeroA = numeroA + "1";
                        }
                        pedacoA = pedacoA.Remove(pedacoA.Length - 1);
                        break;
                    }
                }
            }
            if (equacaoDigitada.Substring(i).Contains('x')) {
                for (; i < equacaoDigitada.Length; i++) {
                    pedacoB += equacaoDigitada[i].ToString();
                    if (equacaoDigitada[i] != 'x' && equacaoDigitada[i] != '=') {
                        if (equacaoDigitada[i] == '-' || equacaoDigitada[i] == '+' || Char.IsNumber(equacaoDigitada[i])) {
                            numeroB = numeroB + equacaoDigitada[i];
                        }
                    } else {
                        if (numeroB == "+" || numeroB == "-")
                            numeroB = numeroB + "1";
                        if (numeroB == "") {
                            numeroB = "0";
                        }
                        if (pedacoB == "=") {
                            pedacoB = "";
                        }
                        //pedacoB = pedacoB.Remove(pedacoB.Length - 1);
                        break;
                    }
                }
            } else {
                numeroB = "0";
            }
            for (; i < equacaoDigitada.Length; i++) {
                pedacoC += equacaoDigitada[i].ToString();
                if (equacaoDigitada[i] != '=') {
                    if (equacaoDigitada[i] == '-' || equacaoDigitada[i] == '+' || Char.IsNumber(equacaoDigitada[i])) {
                        numeroC = numeroC + equacaoDigitada[i];
                    }
                } else {
                    if (numeroC == "") {
                        numeroC = "0";
                    }
                    if (pedacoC == "=") {
                        pedacoC = "";
                    } else {
                        pedacoC = pedacoC.Substring(1, pedacoC.Length - 2);
                    }
                    break;
                }
            }

            // a deve ter 2 ou 3 caracteres depois dos numeros digitados, mas não precisa ter numeros ( x² ou x^2)
            //b deve começar com um sinal e terminar com um x
            // c deve começar com um sinal e acabar com um numero

            if (!pedacoA.Contains("²") && !pedacoA.Contains("^2")) {
                MessageBox.Show("Equação inválida!, é necessário ter uma potência");
                return;
            }
            if (pedacoB.Length > 0 && !pedacoB.Contains("x")) {
                MessageBox.Show("O valor de b deve ter um x");
                return;
            }

            bool podeSinal = true;
            bool podeNumero = true;
            bool podeIncognita = true;
            bool podePow = false;
            bool podePow2Grande = false;
            bool podeCircunflexo = false;
            bool podeIgual = false;

            for (int j = 0; j < pedacoA.Length; j++) {
                Char atual = pedacoA[j];

                if (atual == '-' || atual == '+') {
                    if (podeSinal) {
                        podeSinal = false;
                        podeNumero = true;
                        podeIncognita = true;
                        podePow2Grande = false;
                        podePow = false;
                        podeCircunflexo = false;
                        podeIgual = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! \n Revise os sinais de a");
                    return;
                }

                if (Char.IsLetter(atual)) {
                    if (podeIncognita && atual == 'x') {
                        podeSinal = false;
                        podeNumero = false;
                        podeIncognita = false;
                        podePow2Grande = false;
                        podePow = true;
                        podeCircunflexo = true;
                        podeIgual = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! \n Revise a incógnita de a, a incógnita deve ser = x");
                    return;
                }

                if (atual == '=') {
                    if (podeIgual) {
                        podeSinal = false;
                        podeNumero = true;
                        podeIncognita = false;
                        podePow2Grande = false;
                        podePow = false;
                        podeCircunflexo = false;
                        podeIgual = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! \n Revise o sinal = de a, ele pode estar no lugar errado");
                    return;
                }

                // circunflexo

                if (Char.IsNumber(atual)) {
                    if ((podePow && atual == '²') || (podePow2Grande && atual == '2')) {
                        podeSinal = false;
                        podeNumero = false;
                        podeIncognita = false;
                        podePow2Grande = false;
                        podePow = false;
                        podeCircunflexo = false;
                        podeIgual = false;
                        continue;
                    }
                    if (podeNumero) {
                        podeSinal = false;
                        podeNumero = true;
                        podeIncognita = true;
                        podePow2Grande = false;
                        podePow = false;
                        podeCircunflexo = false;
                        podeIgual = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! \n Revise os numeros de a, a potência deve ser = 2");
                    return;
                }

                if (atual == '^') {
                    if (podeCircunflexo) {
                        podeSinal = false;
                        podeNumero = false;
                        podeIncognita = false;
                        podePow2Grande = true;
                        podePow = false;
                        podeCircunflexo = false;
                        podeIgual = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! verifique a potência");
                    return;
                }

                MessageBox.Show("Equação inválida!");
            }

            podeSinal = true;
            podeNumero = false;
            podeIncognita = false;

            for (int j = 0; j < pedacoB.Length; j++) {
                char atual = pedacoB[j];
                if (atual == '+' || atual == '-') {
                    if (podeSinal) {
                        podeSinal = false;
                        podeNumero = true;
                        podeIncognita = true;
                        continue;
                    }
                    MessageBox.Show(" Equação inválida! verifique o sinal de b");
                    return;
                }

                if (Char.IsNumber(atual)) {
                    if (podeNumero && atual >= '0' && atual <= '9') {
                        podeNumero = true;
                        podeIncognita = true;
                        podeSinal = false;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! verifique os numeros de b");
                    return;
                }

                if (Char.IsLetter(atual)) {
                    if (podeIncognita && atual == 'x') {
                        podeSinal = false;
                        podeNumero = false;
                        podeIncognita = false;
                        continue;
                    }
                    MessageBox.Show("Equação Inválida! verifique a incógnita de b");
                    return;
                }
                MessageBox.Show("Equação inválida! verifique os valores de b");
                return;
            }
            podeSinal = true;
            podeNumero = false;

            for (int j = 0; j < pedacoC.Length; j++) {
                char atual = pedacoC[j];

                if (atual == '+' || atual == '-') {
                    if (podeSinal) {
                        podeSinal = false;
                        podeNumero = true;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! verifique os sinais de c");
                    return;
                }

                if (Char.IsNumber(atual)) {
                    if (podeNumero && atual >= '0' && atual <= '9') {
                        podeSinal = false;
                        podeNumero = true;
                        continue;
                    }
                    MessageBox.Show("Equação inválida! verifique os números de c");
                    return;
                }
                MessageBox.Show("Equação inválida! verifique os valores de c");
                return;
            }
            
            int a = Convert.ToInt32(numeroA);
            int b = Convert.ToInt32(numeroB);
            int c = Convert.ToInt32(numeroC);

            if (a == 0) {
                MessageBox.Show("O valor de a não pode ser 0");
                return;
            }

            double delta = Math.Pow(b, 2) - 4 * a * c;
            double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
            double x2 = (-b - Math.Sqrt(delta)) / (2 * a);

            double xv = -b / (2 * a);
            double yv = -delta / (4 * a);

            lblX1Result.Text = x1.ToString("F");
            lblX2Result.Text = x2.ToString("F");
            lblAResult.Text = a.ToString();
            lblBResult.Text = b.ToString();
            lblCResult.Text = c.ToString();
            lblXVResult.Text = xv.ToString("F");
            lblYVResult.Text = yv.ToString("F");

            if (delta < 0) {
                lblRaizesReaisResult.Text = "0";
            } else if (delta == 0) {
                lblRaizesReaisResult.Text = "1";
            } else {
                lblRaizesReaisResult.Text = "2";
            }
        }

        private void parseGiovano() {
            tbEquacao.Text = "2x²+3x-5=0";
            tbEquacao.Text = "2x²-5=0";
            tbEquacao.Text = "-x²+3x-5=0";
            tbEquacao.Text = "-2x²+3x-5=0";
            tbEquacao.Text = "x^2+3x-5=0";

            String equacao = tbEquacao.Text.Trim();

            String[] operadores = { "-", "+", "=" };
            String[] partes = new String[3];
            int parte = 0;
            String pedaco = "";
            Boolean potencia = false;

            // 2x²+3x-5=0
            for (int i = 0; i < equacao.Length; i++) {
                String letra = equacao[i].ToString();
                if (String.IsNullOrEmpty(pedaco) || !operadores.Contains(letra)) {
                    if (i < 2 && letra == "x") {
                        letra = $"1{letra}";
                    }
                    if (!potencia) {
                        pedaco += letra;
                    } else {
                        potencia = false;
                    }
                } else {
                    if (letra == "^") {
                        potencia = true;
                    }
                    if (!pedaco.Contains("x") && parte == 1) {
                        parte++;
                    }
                    partes[parte] = pedaco;
                    pedaco = "";
                    parte++;
                    i--;
                }
            }

            String[] charsToRemove = { "^2", "²", "x" };
            for (int i = 0; i < partes.Length; i++) {
                foreach (String letra in charsToRemove) {
                    partes[i] = partes[i].Replace(letra, "");
                }
            }

            int a = Convert.ToInt32(partes[0]);
            int b = Convert.ToInt32(partes[1]);
            int c = Convert.ToInt32(partes[2]);

            double delta = Math.Pow(b, 2) - 4 * a * c;
            double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
            double x2 = (-b - Math.Sqrt(delta)) / (2 * a);

            double xv = -b / (2 * a);
            double yv = -delta / (4 * a);

            lblX1Result.Text = x1.ToString();
            lblX2Result.Text = x2.ToString();
            lblAResult.Text = a.ToString();
            lblBResult.Text = b.ToString();
            lblCResult.Text = c.ToString();
            lblXVResult.Text = xv.ToString();
            lblYVResult.Text = yv.ToString();

            if (delta < 0) {
                lblRaizesReaisResult.Text = "0";
            } else if (delta == 0) {
                lblRaizesReaisResult.Text = "1";
            } else {
                lblRaizesReaisResult.Text = "2";
            }
        }

    }

}