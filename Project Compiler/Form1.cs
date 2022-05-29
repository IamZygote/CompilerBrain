using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Project_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            string x = textBox1.Text;
            List<string> Code = new List<string>();
            List<string> Scanner = new List<string>();
            Scan(Scanner, Code, x, textBox4);
            Analyze(Scanner, Code,textBox2,textBox3);
        }

        public void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public static void Scan(List<string> Scanner, List<string> Code, string x,TextBox textBox)
        {
            string[] identifiers = { "int", "char", "float", "string", "double", "bool" };
            string[] symbols = { "+", "-", "%", "/", "*", "(", ")", "{", "}", ",", ";", "&&", "||", "<", ">", "=", "!", "|", "&" };
            string[] reservedWords = { "for", "while", "if", "do", "return", "break", "continue", "end" };
            char[] seperators = { ' ', '(', ')', '=', '{', '}', ';', ',', '<', '>', '&', '|', '=', '!', '+', '-', '*', '/', '%' };
            string substring = "";
            int seperatorFlag = 0;
            // this loop for seperating the input word.
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < seperators.Length; j++)
                {
                    if (x[i] == seperators[j])
                    {
                        if (substring != "")
                            Code.Add(substring);
                        if (seperators[j] != ' ')
                        {
                            substring = "";
                            substring += seperators[j];
                            Code.Add(substring);
                        }
                        substring = "";
                        seperatorFlag = 0;
                        break;
                    }
                    else
                    {
                        seperatorFlag = 1;
                    }
                }
                if (seperatorFlag == 1)
                {
                    substring += x[i];
                    if (i == x.Length - 1 && seperatorFlag == 1)
                    {
                        Code.Add(substring);
                    }
                    seperatorFlag = 0;
                }
            }
            // this loops for output the seperated words.
            int variableFlag = 0;
            for (int i = 0; i < Code.Count; i++)
            {
                variableFlag = 0;
                for (int j = 0; j < identifiers.Length; j++)
                {
                    if (Code[i] == identifiers[j])
                    {
                        Scanner.Add("identifier");
                        //textBox.Text += Code[i] + " is an identifier\n";
                        textBox.AppendText(Code[i] + " is a identifier\n");
                        textBox.AppendText(Environment.NewLine);
                        break;
                    }
                    if (j == identifiers.Length - 1)
                        variableFlag++;
                }

                for (int j = 0; j < reservedWords.Length; j++)
                {
                    if (Code[i] == reservedWords[j])
                    {
                        Scanner.Add("reservedWord");
                        //textBox.Text += Code[i] + " is a reservedWord.";
                        textBox.AppendText(Code[i] + " is a reservedWord\n");
                        textBox.AppendText(Environment.NewLine);
                        break;
                    }
                    if (j == reservedWords.Length - 1)
                        variableFlag++;
                }

                for (int j = 0; j < symbols.Length; j++)
                {
                    if (Code[i] == symbols[j])
                    {
                        Scanner.Add("symbol");
                        //textBox.Text += Code[i] + " is a symbol";
                        textBox.AppendText(Code[i] + " is a symbol\n");
                        textBox.AppendText(Environment.NewLine);
                        break;
                    }
                    if (j == symbols.Length - 1)
                        variableFlag++;
                }
                if (variableFlag == 3)
                {
                    Scanner.Add("variable");
                    //textBox.Text += Code[i] + " is a variable";
                    textBox.AppendText(Code[i] + " is a variable\n");
                    textBox.AppendText(Environment.NewLine);
                }
            }
        }
        public static bool Analyze(List<string> Scanner, List<string> Code, TextBox textBox, TextBox textbox2)
        {
            int idF = 0;
            int varF = 0;
            int equF = 0;
            int semF = 1;
            int resF = 0;
            int openBF = 0;
            int simpF = 0;
            int andF = 0;
            int clseBF = 0;
            int curlyF = 0;
            string temp = "";
            List<string> Scanner2 = new List<string>();
            List<string> Code2 = new List<string>();
            int index=0;
            for (int j = 0; j < Code.Count; j++)
            {
                if (Scanner[j] == "reservedWord" && resF == 0 && varF == 0 && idF == 0)
                {
                    resF = 1;
                }
                else if (resF == 1)
                {
                    if (Code[j] == "(")
                    {
                        openBF = 1;
                    }
                    else if (openBF == 1)
                    {
                        if (Scanner[j] == "variable" && varF == 0)
                        {
                            varF = 1;
                            simpF = 0;
                        }
                        else if (Scanner[j] == "variable" && varF == 1 && (temp == "<" || temp == ">" || temp == "<=" || temp == ">=" || temp == "!=" || temp == "=="))
                        {
                            temp = "";
                            simpF = 0;
                            varF = 2;
                        }
                        else if (varF == 1)
                        {
                            if ((Code[j] == "<" || Code[j] == ">" || Code[j] == "=" || Code[j] == "!") && simpF == 0)
                            {
                                simpF = 1;
                                temp = Code[j];
                            }
                            else if (simpF == 1)
                            {
                                if ((temp == "!" || temp == "=") && Code[j] == "=")
                                {
                                    temp += "=";
                                }
                                else if ((temp == "!" || temp == "=") && Code[j] != "=")
                                {
                                    textBox.AppendText("= missing\n");
                                    textBox.AppendText(Environment.NewLine);
                                    return false;
                                }
                                else if ((temp == ">" || temp == "<") && Code[j] == "=")
                                {
                                    temp += "=";
                                }
                                else
                                {
                                    textBox.AppendText("variable missing\n");
                                    textBox.AppendText(Environment.NewLine);
                                    return false;
                                }
                            }
                        }
                        else if (varF == 2)
                        {
                            if ((Code[j] == "&" || Code[j] == "|") && andF == 0)
                            {
                                andF = 1;
                                temp = Code[j];
                            }
                            else if (andF == 1)
                            {
                                if (temp == Code[j])
                                {
                                    andF = 0;
                                    varF = 0;
                                    temp = "";
                                }
                                else
                                {
                                    textBox.AppendText(temp + " missing\n");
                                    textBox.AppendText(Environment.NewLine);
                                    return false;
                                }
                            }
                            else if (Code[j] == ")")
                            {
                                openBF = 0;
                                resF = 0;
                                clseBF = 1;
                                varF = 0;
                            }
                        }
                        else
                        {
                            textBox.AppendText("variable missing\n");
                            textBox.AppendText(Environment.NewLine);
                        }
                    }

                }
                else if (clseBF == 1 && curlyF == 0 && Code[j] == "{")
                {
                    curlyF = 1;
                }
                else if (Scanner[j] == "identifier" && varF == 0 && idF == 0 && (clseBF == 0 || curlyF == 1))
                {
                    idF = 1;
                    semF = 0;
                }
                else if (Scanner[j] == "variable" && varF == 0 && (clseBF == 0 || curlyF == 1))
                {
                    varF = 1;
                    semF = 0;
                }
                else if (varF == 1 && equF == 0)
                {
                    if (Code[j] == ";")
                    {
                            for(int p=0;p<j ;p++)
                            {
                                Scanner2.Add(Scanner[p]);
                                Code2.Add(Code[p]);
                            }
                            index = j;
                            Memory(Scanner2, Code2, textbox2);
                            Scanner2.Clear();
                            Code2.Clear();
                        
                        idF = 0;
                        varF = 0;
                        semF = 1;
                    }
                    else if (Code[j] == "=")
                    {
                        equF = 1;
                        varF = 0;
                    }
                    else
                    {
                        textBox.AppendText("semicolon missing\n");
                        textBox.AppendText(Environment.NewLine);
                        return false;
                    }
                }
                else if (equF == 1)
                {
                    if (Scanner[j] == "variable" && varF == 0)
                    {
                        varF = 1;
                    }
                    else if (varF == 1)
                    {
                        if (Code[j] == ";")
                        {
                                for (int p = 0; p < j; p++)
                                {
                                    Scanner2.Add(Scanner[p]);
                                    Code2.Add(Code[p]);
                                }
                                index = j;
                                Memory(Scanner2, Code2, textbox2);
                                Scanner2.Clear();
                                Code2.Clear();
                            
                            equF = 0;
                            idF = 0;
                            varF = 0;
                            semF = 1;
                        }
                        else if (Code[j] == "+" || Code[j] == "-" || Code[j] == "%" || Code[j] == "/" || Code[j] == "*")
                        {
                            varF = 0;
                        }
                        else
                        {
                            textBox.AppendText("semicolon missing\n");
                            textBox.AppendText(Environment.NewLine);
                            return false;
                        }
                    }
                    else
                    {
                        textBox.AppendText("variable missing\n");
                        textBox.AppendText(Environment.NewLine);
                        return false;
                    }
                }
                else if (curlyF == 1 && clseBF == 1 && Code[j] == "}")
                {
                    curlyF = 0;
                    clseBF = 0;
                }
                else
                {
                    textBox.AppendText("variable missing\n");
                    textBox.AppendText(Environment.NewLine);
                    return false;
                }
            }
            if (resF == 1 && varF == 0 && openBF == 0)
            {
                textBox.AppendText("( missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            //if(x==X) {int x=0}
            else if (openBF == 1 && (varF == 0 || temp == "<" || temp == ">" || temp == "<=" || temp == ">=" || temp == "!=" || temp == "=="))
            {
                textBox.AppendText("variable missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (openBF == 1 && varF == 1)
            {
                textBox.AppendText("symbol missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (openBF == 1 && varF == 2 && andF == 0)
            {
                textBox.AppendText(") missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (openBF == 1 && varF == 2 && andF == 1)
            {
                textBox.AppendText(temp + " missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (curlyF == 0 && clseBF == 1)
            {
                textBox.AppendText("{ missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (idF == 1 && varF == 0)
            {
                textBox.AppendText("variable missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (equF == 1 && varF == 0)
            {
                textBox.AppendText("variable missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (semF == 0)
            {
                textBox.AppendText("semicolon missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            else if (curlyF == 1 && clseBF == 1)
            {
                textBox.AppendText("} missing\n");
                textBox.AppendText(Environment.NewLine);
                return false;
            }
            return true;
        }
        
        public static void Memory(List<string> Scanner, List<string> Code, TextBox textBox)
        {
            textBox.Clear();
            List<string> variables = new List<string>();
            List<int> numbers = new List<int>();
            int checkFlag = 0;
            int checkOperator = 0;
            char currentOperator = ' ';
            int addVariable = 1;
            for (int i = 0; i < Code.Count; i++)
            {
                if (Scanner[i] == "identifier" && Scanner[i + 1] == "variable")
                {
                    //check if the variables are numbers
                    if (Regex.IsMatch(Code[i], @"^\d+$"))
                    {
                        Console.WriteLine("syntax Error ");
                        break;
                    }
                    else
                    {
                        variables.Add(Code[i + 1]);
                        addVariable = 0;
                    }
                }
                if (Scanner[i] == "variable" && i + 1 < Code.Count && Code[i + 1] == "=" && checkFlag == 0)
                {
                    int isThisVariableDefined = 0;
                    for (int k = 0; k < variables.Count; k++)
                    {
                        if (Code[i] == variables[k])
                        {
                            if (addVariable == 1)
                            {
                                variables.Add(Code[i]);
                            }
                            checkFlag = 1;
                            if (i + 2 > Code.Count - 1)
                            {
                                Console.WriteLine("syntax Error ");
                                break;
                            }
                            else
                            { i += 2; }
                            isThisVariableDefined = 1;
                            break;
                        }
                    }
                    if (isThisVariableDefined == 0)
                    {
                        Console.WriteLine("syntax error: variable is not defined");
                        break;
                    }
                    addVariable = 1;
                }
                else if (Scanner[i] == "variable" && i + 1 < Code.Count && Code[i + 1] == ";" && currentOperator == ' ')
                {
                    int isThisVariableDefined = 0;
                    for (int k = 0; k < variables.Count; k++)
                    {
                        if (Code[i] == variables[k])
                        {
                            numbers.Add(-5345234);//el rakam da lw el variable = NULL
                            isThisVariableDefined = 1;
                        }
                    }
                    if (isThisVariableDefined == 0)
                    {
                        Console.WriteLine("variable is not defined");
                        break;
                    }
                    addVariable = 1;
                }
                if (checkFlag == 1)
                {
                    if (Code[i] == ";")
                    {
                        checkFlag = 0;
                    }
                    if (Scanner[i] == "variable")
                    {
                        //check if the variables are numbers
                        if (Regex.IsMatch(Code[i], @"^\d+$"))
                        {
                            numbers.Add(Convert.ToInt32(Code[i]));
                            if (currentOperator != ' ')
                            {
                                equation(currentOperator, numbers);
                                currentOperator = ' ';
                            }
                            checkOperator = 1;
                        }
                        else
                        {
                            int posOfLastSameVariable = -1;
                            for (int j = 0; j < variables.Count; j++)
                            {
                                if (variables[j] == Code[i])
                                {
                                    posOfLastSameVariable = j;
                                }
                            }
                            numbers.Add(numbers[posOfLastSameVariable]);
                            if (currentOperator != ' ')
                            {
                                equation(currentOperator, numbers);
                                currentOperator = ' ';
                            }
                            checkOperator = 1;
                        }
                    }
                    else if (Scanner[i] == "symbol" && checkOperator == 1)
                    {
                        if (Code[i] == "+")
                        {
                            currentOperator = '+';
                        }
                        if (Code[i] == "-")
                        {
                            currentOperator = '-';
                        }
                        if (Code[i] == "/")
                        {
                            currentOperator = '/';
                        }
                        if (Code[i] == "*")
                        {
                            currentOperator = '*';
                        }
                        if (Code[i] == "%")
                        {
                            currentOperator = '%';
                        }
                        checkOperator = 0;
                    }
                    else
                    {
                        Console.WriteLine("syntax error in memory");
                    }
                }
            }
            for (int h = 0; h < variables.Count; h++)
            {
                if(numbers.Count!=0)
                {
                    if (numbers[h] == -5345234) // bayez int y,  int y=;
                    {
                        textBox.AppendText(variables[h] + " = NULL");
                        textBox.AppendText(Environment.NewLine);
                    }
                    else 
                    {
                        textBox.AppendText(variables[h] + " = " + numbers[h]);
                        textBox.AppendText(Environment.NewLine);
                    }
                }
              
            }
        }
        static void equation(char currentOperator, List<int> numbers)
        {
            if (currentOperator == '+')
            {
                numbers[numbers.Count - 2] += numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
            }
            if (currentOperator == '-')
            {
                numbers[numbers.Count - 2] -= numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
            }
            if (currentOperator == '/')
            {
                numbers[numbers.Count - 2] /= numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
            }
            if (currentOperator == '*')
            {
                numbers[numbers.Count - 2] *= numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
            }
            if (currentOperator == '%')
            {
                numbers[numbers.Count - 2] %= numbers[numbers.Count - 1];
                numbers.RemoveAt(numbers.Count - 1);
            }
        }
    }
}
