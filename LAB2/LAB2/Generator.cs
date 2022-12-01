using LAB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    // Класс "Генератор".
    class Generator
    {
        SyntaxTreeNode treeRoot; // Корень синтаксического дерева.
        List<string> outputText; // Выходной текст - список строк.  

        // Конструктор генератора.
        // В качестве параметра поступает корень синтаксического дерева.
        public Generator(SyntaxTreeNode treeRoot)
        {
            this.treeRoot = treeRoot;
            outputText = new List<string>();
        }

        // Выходной текст - свойство только для чтения.
        public List<string> OutputText
        {
            get { return outputText; }
        }

        // Сгенерировать структурированный текст.
        public void GenerateStructuredText()
        {
            outputText.Clear(); // Очищаем выходной текст.
            RecurTraverseTree(treeRoot, 0); // Рекурсивно обходим дерево и формируем выходной текст.
        }

        // Рекурсивно обойти дерево, формируя выходной текст.
        // node - узел дерева.
        // indent - отступ.
        private void RecurTraverseTree(SyntaxTreeNode node, int indent)
        {
            if (node.SubNodes.Count() > 0) // Если текущий узел - нетерминал.
            {
                foreach (SyntaxTreeNode item in node.SubNodes) // Цикл по всем подчиненным узлам.
                {
                    if ((node != treeRoot) && (node.Name == "P")) // Если текущий узел - не корневой и с именем E.
                        RecurTraverseTree(item, indent + 10); // Значит, это выражение в скобках. Выведем его с дополнительным отступом.
                    else
                        RecurTraverseTree(item, indent); // Выведем с текущим отступом.
                }
            }
            else
            {
                // Знаки операций выводим с дополнительным отступом, а остальные терминалы - с текущим отступом.
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

                // Генерируем отступ.       
                for (int i = 0; i < indent + k; i++)
                {
                    s += " ";
                }

                // Добавляем имя узла.
                s += GetDecimalRepresentation(node.Name);

                // Добавляем созданную строку в результат.
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
