using LAB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    // ����� "���������".
    class Generator
    {
        SyntaxTreeNode treeRoot; // ������ ��������������� ������.
        List<string> outputText; // �������� ����� - ������ �����.  

        // ����������� ����������.
        // � �������� ��������� ��������� ������ ��������������� ������.
        public Generator(SyntaxTreeNode treeRoot)
        {
            this.treeRoot = treeRoot;
            outputText = new List<string>();
        }

        // �������� ����� - �������� ������ ��� ������.
        public List<string> OutputText
        {
            get { return outputText; }
        }

        // ������������� ����������������� �����.
        public void GenerateStructuredText()
        {
            outputText.Clear(); // ������� �������� �����.
            RecurTraverseTree(treeRoot, 0); // ���������� ������� ������ � ��������� �������� �����.
        }

        // ���������� ������ ������, �������� �������� �����.
        // node - ���� ������.
        // indent - ������.
        private void RecurTraverseTree(SyntaxTreeNode node, int indent)
        {
            if (node.SubNodes.Count() > 0) // ���� ������� ���� - ����������.
            {
                foreach (SyntaxTreeNode item in node.SubNodes) // ���� �� ���� ����������� �����.
                {
                    if ((node != treeRoot) && (node.Name == "P")) // ���� ������� ���� - �� �������� � � ������ E.
                        RecurTraverseTree(item, indent + 10); // ������, ��� ��������� � �������. ������� ��� � �������������� ��������.
                    else
                        RecurTraverseTree(item, indent); // ������� � ������� ��������.
                }
            }
            else
            {
                // ����� �������� ������� � �������������� ��������, � ��������� ��������� - � ������� ��������.
                int k;
                if ((node.Token.Type == TokenKind.Comma) ||
                    (node.Token.Type == TokenKind.Semicolon)) 
                {
                    k = 10;
                }
                else
                {
                    k = 0;
                }

                string s = "";

                // ���������� ������.       
                for (int i = 0; i < indent + k; i++)
                {
                    s += " ";
                }

                // ��������� ��� ����.
                s += GetDecimalRepresentation(node.Name);

                // ��������� ��������� ������ � ���������.
                outputText.Add(s);
            }
        }

        private string GetDecimalRepresentation(string name)
        {
            if(!name.All(x => char.IsDigit(x)))
            {
                return name;
            }

            long numb = long.Parse(name);
            int index = 0;
            double result = 0;
            while (numb != 0)
            {
                int n = int.Parse((numb % 10).ToString());
                result += n * Math.Pow(2, index);
                index++;
                numb = numb / 10;
            }
            return result.ToString();
        }
    }
}
