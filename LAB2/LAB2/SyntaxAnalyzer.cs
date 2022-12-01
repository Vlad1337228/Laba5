using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2
{
    // Класс исключительных ситуаций синтаксического анализа.
    class SynAnException : Exception
    {
        // Позиция возникновения исключительной ситуации в анализируемом тексте.
        private int lineIndex; // Индекс строки.
        private int symIndex;  // Индекс символа.

        // Индекс строки, где возникла исключительная ситуация - свойство только для чтения.
        public int LineIndex
        {
            get { return lineIndex; }
        }

        // Индекс символа, на котором возникла исключительная ситуация - свойство только для чтения.
        public int SymIndex
        {
            get { return symIndex; }
        }

        // Конструктор исключительной ситуации.
        // message - описание исключительной ситуации.
        // lineIndex и symIndex - позиция возникновения исключительной ситуации в анализируемом тексте.
        public SynAnException(string message, int lineIndex, int symIndex)
            : base(message)
        {
            this.lineIndex = lineIndex;
            this.symIndex = symIndex;
        }
    }

    // Класс "Синтаксический анализатор".
    // При обнаружении ошибки в исходном тексте он генерирует исключительную ситуацию SynAnException или LexAnException.
    class SyntaxAnalyzer
    {
        private LexicalAnalyzer lexAn; // Лексический анализатор.

        // Конструктор синтаксического анализатора. 
        // В качестве параметра передается исходный текст.
        public SyntaxAnalyzer(string[] inputLines)
        {
            // Создаем лексический анализатор.
            // Передаем ему текст.
            lexAn = new LexicalAnalyzer(inputLines);
        }

        // Обработать синтаксическую ошибку.
        // msg - описание ошибки.
        private void SyntaxError(string msg)
        {
            // Генерируем исключительную ситуацию, тем самым полностью прерывая процесс анализа текста.
            throw new SynAnException(msg, lexAn.CurLineIndex, lexAn.CurSymIndex);
        }

        private void GetAttribyteError(string msg, string value)
        {
            throw new AttribyteException(msg, lexAn.CurLineIndex, lexAn.CurSymIndex, value );
        }

        // Проверить, что тип текущего распознанного токена совпадает с заданным.
        // Если совпадает, то распознать следующий токен, иначе синтаксическая ошибка.
        private void Match(TokenKind tkn)
        {
            if (lexAn.Token.Type == tkn) // Сравниваем.
            {
                lexAn.RecognizeNextToken(); // Распознаем следующий токен.
            }
            else 
            {
                SyntaxError("Ожидалось " + tkn.ToString()); // Обнаружена синтаксическая ошибка.
            }
        }


        // Провести синтаксический анализ текста.
        public void ParseText(out SyntaxTreeNode treeRoot)
        {
            P(out treeRoot);

            if (lexAn.Token.Type != TokenKind.EndOfText) // Если текущий токен не является концом текста.
            {
                SyntaxError("После арифметического выражения идет еще какой-то текст"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void P(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("P");

            SyntaxTreeNode nodeK;
            lexAn.RecognizeNextToken(); // Распознаем первый токен в тексте.
            K(out nodeK);
            node.AddSubNode(nodeK);
            CheckNumberInList(node, nodeK.ListNumbers);

            if (lexAn.Token.Type != TokenKind.EndOfText) // Если текущий токен не является концом текста.
            {
                SyntaxTreeNode nodeS;
                S(out nodeS);
                node.AddSubNode(nodeS);
                CheckNumberInList(node, nodeS.ListNumbers);

            }
        }


        private void K(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("K"); // Создаем узел с именем "T".

            SyntaxTreeNode nodeO;
            O(out nodeO);
            node.AddSubNode(nodeO);
            CheckNumberInList(node, nodeO.ListNumbers);

            SyntaxTreeNode nodeR;
            R(out nodeR);
            node.AddSubNode(nodeR);
            CheckNumberInList(node, nodeR.ListNumbers);

            if (lexAn.Token.Type == TokenKind.Comma)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
            }
            else
            {
                SyntaxError("Ожидалась запятая");
            }

            SyntaxTreeNode nodeA;
            A(out nodeA);
            node.AddSubNode(nodeA);
            CheckNumberInList(node, nodeA.ListNumbers);

        }

        private void S(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("S");

            if (lexAn.Token.Type == TokenKind.Semicolon)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));

                SyntaxTreeNode nodeP;
                P(out nodeP);
                node.AddSubNode(nodeP);
                CheckNumberInList(node, nodeP.ListNumbers);
            }
            else
            {
                SyntaxError("После арифметического выражения идет еще какой-то текст"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void O(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("O");

            if (lexAn.Token.Type == TokenKind.Identifier) // Если текущий токен - идентификатор.
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));

                lexAn.RecognizeNextToken(); // Пропускаем этот идентификатор.
            }
            else
            {
                SyntaxError("Ожидались буквы"); // Обнаружена синтаксическая ошибка.
            }

            
        }

        private void R(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("R");

            if (lexAn.Token.Type == TokenKind.Number) // Если текущий токен - число.
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));

                if (lexAn.Token.Value.All(x => char.IsDigit(x)))
                {
                    CheckNumberInList(node, new List<string>() { lexAn.Token.Value });
                }

                lexAn.RecognizeNextToken(); // Пропускаем этот идентификатор.
            }
            else
            {
                SyntaxError("Ожидались цифры"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void A(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("A");

            if (lexAn.Token.Type == TokenKind.Number)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));

                if (lexAn.Token.Value.All(x => char.IsDigit(x)))
                {

                    CheckNumberInList(node, new List<string>() { lexAn.Token.Value });
                }

                lexAn.RecognizeNextToken();
            }
            else if (lexAn.Token.Type == TokenKind.Identifier)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
            }
            else
            {
                SyntaxError("Ожидались буква (a,b,c,d) или цифра (1,0)"); // Обнаружена синтаксическая ошибка.
            }
        }

        private void CheckNumberInList(SyntaxTreeNode node, List<string> listNimbers)
        {
            var result = node.AddNumbersToList(listNimbers);
            if (!string.IsNullOrEmpty(result))
            {
                GetAttribyteError("Такое число уже существует в данном контексте ", result);
            }
        }

    }
}
