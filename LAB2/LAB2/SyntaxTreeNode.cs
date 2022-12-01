using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    // Класс "Узел синтаксического дерева".
    public class SyntaxTreeNode
    {
        private string name; // Наименование узла.
        private List<SyntaxTreeNode> subNodes; // Подчиненные узлы.
        private Token token; // Токен (если узел - листьевой).
        private List<string> listNumbers { get; set; } = new List<string>();




        // Конструктор узла.
        public SyntaxTreeNode(string name)
        {
            this.name = name;
            this.token = null;
            subNodes = new List<SyntaxTreeNode>();
        }


        // Конструктор листьевого узла.
        public SyntaxTreeNode(Token tkn)
        {
            this.name = tkn.Value;
            this.token = tkn;
            subNodes = new List<SyntaxTreeNode>();
        }





        // Добавить узел в список подчиненных узлов.
        public void AddSubNode(SyntaxTreeNode subNode)
        {
            this.subNodes.Add(subNode);
        }

        // Наименование узла - свойство только для чтения.
        public string Name
        {
            get { return name; }
        }

        public Token Token
        {
            get { return token; }
        }

        // Список подчиненных узлов - свойство только для чтения.
        public List<SyntaxTreeNode> SubNodes
        {
            get { return subNodes; }
        }

        public List<string> ListNumbers
        {
            get { return listNumbers; }
        }

        public string AddNumbersToList(List<string > strings)
        {
            if (strings == null || strings.Count == 0)
                return null;

            foreach (var item in strings)
            {
                if(listNumbers.Contains(item))
                {
                    return item;
                }
            }

            listNumbers.AddRange(strings);
            return null;

        }


    }


    public class AttribyteException : Exception
    {

        public int LineIndex { get;  }
        public int SymIndex { get;  }
        public string Value { get; set; }


        public AttribyteException(string message, int lineIndex, int symIndex, string value)
            : base(message)
        {
            this.LineIndex = lineIndex;
            this.SymIndex = symIndex;
            this.Value = value;
        }

    } 

}
