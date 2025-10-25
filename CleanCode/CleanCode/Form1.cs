using System;
using System.Windows.Forms;
using NetSpell.SpellChecker.Dictionary;
using NetSpell.SpellChecker;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using edu.stanford.nlp.ling;
using Console = System.Console;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CleanCode
{
    public partial class Form1 : Form
    {
        FormCheckVariableName formCheckVariableName = new FormCheckVariableName();
        FormCheckMethodName formCheckMethodName = new FormCheckMethodName();
        FormCheckMethodParameters formCheckMethodParameters = new FormCheckMethodParameters();
        FormCheckMethodSize formCheckMethodSize = new FormCheckMethodSize();
        FormCheckMethodSingleResponsibility formCheckMethodSingleResponsibility = new FormCheckMethodSingleResponsibility();
        FormCheckIfBlock formCheckIfBlock = new FormCheckIfBlock();
        FormCheckElseBlock formCheckElseBlock = new FormCheckElseBlock();
        FormCheckForBlock formCheckForBlock = new FormCheckForBlock();
        FormCheckWhileBlock formCheckWhileBlock = new FormCheckWhileBlock();
        FormCheckNestedIf formCheckNestedIf = new FormCheckNestedIf();
        FormCheckNestedFor formCheckNestedFor = new FormCheckNestedFor();
        FormCheckNestedWhile formCheckNestedWhile = new FormCheckNestedWhile();
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                textBox.Text = fileName;
            }
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            CheckVariableName();
            CheckMethodName();
            CheckMethodParameters();
            CheckMethodSize();
            CheckMethodSingleResponsibility();
            CheckIfBlock();
            CheckElseBlock();
            CheckForBlock();
            CheckWhileBlock();
            CheckNestedIf();
            CheckNestedFor();
            CheckNestedWhile();
        }
        private void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (dataGridView.SelectedRows[0].Cells[0].RowIndex)
            {
                case 0:
                    formCheckVariableName.Show();
                    break;
                case 1:
                    formCheckMethodName.Show();
                    break;
                case 2:
                    formCheckMethodParameters.Show();
                    break;
                case 3:
                    formCheckMethodSize.Show();
                    break;
                case 4:
                    formCheckMethodSingleResponsibility.Show();
                    break;
                case 5:
                    formCheckIfBlock.Show();
                    break;
                case 6:
                    formCheckElseBlock.Show();
                    break;
                case 7:
                    formCheckForBlock.Show();
                    break;
                case 8:
                    formCheckWhileBlock.Show();
                    break;
                case 9:
                    formCheckNestedIf.Show();
                    break;
                case 10:
                    formCheckNestedFor.Show();
                    break;
                case 11:
                    formCheckNestedWhile.Show();
                    break;
                default:
                    break;
            }
        }
        public void Show(string x, string y)
        {
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Clean Code Principle";
            dataGridView.Columns[0].MinimumWidth = 595;
            dataGridView.Columns[1].Name = "Count";
            dataGridView.Columns[1].MinimumWidth = 45;
            string[] row = new string[] { x, y };
            dataGridView.Rows.Add(row);
        }
        public string SplitCamelCase(string x)
        {
            return System.Text.RegularExpressions.Regex.Replace(x, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        public bool IsEnglishWord(string x)
        {
            WordDictionary dictionary = new WordDictionary();
            dictionary.DictionaryFile = @"..\..\NewFolder\en-US.dic";
            dictionary.Initialize();
            Spelling spell = new Spelling();
            spell.Dictionary = dictionary;
            if (spell.TestWord(x))
            {
                return true;
            }
            return false;
        }
        public string Tag(string x)
        {
            var model = new MaxentTagger(@"..\..\NewFolder\wsj-0-18-bidirectional-nodistsim.tagger", null, false);
            var sentence = MaxentTagger.tokenizeText(new StringReader(x)).toArray();
            foreach (ArrayList i in sentence)
            {
                var taggedSentence = model.tagSentence(i);
                return SentenceUtils.listToString(taggedSentence, false);
            }
            return null;
        }
        public void CleanVariableName(string x, string y)
        {
            string text1 = SplitCamelCase(x);
            string[] text2 = text1.Split(' ');
            for (int i = 0; i < text2.Length; i++)
            {
                if (IsEnglishWord(text2[i]) != true)
                {
                    formCheckVariableName.Show(x, y, "It contains a non-English word.");
                    break;
                }
            }
            if (Tag(text2[0]) != text2[0].ToString() + "/NN")
            {
                formCheckVariableName.Show(x, y, "Its first word is not Noun.");
            }
        }
        public void CleanMethodName(string x, string y)
        {
            string text1 = SplitCamelCase(x);
            string[] text2 = text1.Split(' ');
            for (int i = 0; i < text2.Length; i++)
            {
                if (IsEnglishWord(text2[i]) != true)
                {
                    formCheckMethodName.Show(x, y, "It contains a non-English word.");
                    break;
                }
            }
            if (Tag(text2[0]) != text2[0].ToString() + "/VB")
            {
                formCheckMethodName.Show(x, y, "Its first word is not Verb.");
            }
        }
        public void CheckVariableName()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var variableDeclarator = root.DescendantNodes().OfType<VariableDeclaratorSyntax>();
            var forStatement = root.DescendantNodes().OfType<ForStatementSyntax>();
            ArrayList arrayList = new ArrayList();
            foreach (var i in forStatement)
            {
                arrayList.Add(i.DescendantNodes().OfType<IdentifierNameSyntax>().First().Identifier.ToString());
            }
            foreach (var i in variableDeclarator)
            {
                if (!arrayList.contains(i.Identifier.ToString()))
                {
                    count = count + 1;
                    CleanVariableName(i.Identifier.ToString(), tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("All words forming the variable name should be meaningful (English word). The first word forming the variable name should be Noun. Clean Code book. Page 17.", count.ToString());
        }
        public void CheckMethodName()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var i in methodDeclaration)
            {
                count = count + 1;
                CleanMethodName(i.Identifier.ToString(), tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
            }
            Show("All words forming the method name should be meaningful (English word). The first word forming the method name should be Verb. Clean Code book. Page 25.", count.ToString());
        }
        public void CheckMethodParameters()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var i in methodDeclaration)
            {
                if (i.ParameterList.Parameters.Count > 4)
                {
                    count = count + 1;
                    formCheckMethodParameters.Show(i.Identifier.ToString(), tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("The number of the method parameters should not exceed 4 cases. Clean Code book. Page 40.", count.ToString());
        }
        public void CheckMethodSize()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var i in methodDeclaration)
            {
                var startLinePosition = tree.GetLineSpan(i.Span).StartLinePosition.Line;
                var endLinePosition = tree.GetLineSpan(i.Span).EndLinePosition.Line;
                if (endLinePosition - startLinePosition + 1 > 24)
                {
                    count = count + 1;
                    formCheckMethodSize.Show(i.Identifier.ToString(), tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("The size of the method should not exceed 24 lines. Clean Code book. Page 34.", count.ToString());
        }
        public void CheckMethodSingleResponsibility()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var i in methodDeclaration)
            {
                var returnStatement = i.DescendantNodes().OfType<ReturnStatementSyntax>();
                if (returnStatement.Count() > 1)
                {
                    ArrayList arrayList = new ArrayList();
                    foreach (var j in returnStatement)
                    {
                        arrayList.add(j.Expression);
                    }
                    for (int k = 1; k < arrayList.size(); k++)
                    {
                        if (arrayList.toArray()[0].ToString() != arrayList.toArray()[k].ToString())
                        {
                            count = count + 1;
                            formCheckMethodSingleResponsibility.Show(i.Identifier.ToString(), tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                            break;
                        }
                    }
                }
            }
            Show("The method should have one responsibility (single responsibility principle). Clean Code book. Page 35.", count.ToString());
        }
        public void CheckIfBlock ()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var ifStatement = root.DescendantNodes().OfType<IfStatementSyntax>();
            foreach (var i in ifStatement)
            {
                var elseClause = i.DescendantNodes().OfType<ElseClauseSyntax>().First();
                var startLinePosition = tree.GetLineSpan(i.Span).StartLinePosition.Line;
                var endLinePosition = tree.GetLineSpan(elseClause.Span).StartLinePosition.Line - 1;
                if (endLinePosition - startLinePosition + 1 != 4)
                {
                    count = count + 1;
                    formCheckIfBlock.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("The block within If statement should be 1 line. Probably that line should be a function call. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckElseBlock()
        {
            int count = 0;
            ArrayList arrayList = new ArrayList();
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var ifStatement = root.DescendantNodes().OfType<IfStatementSyntax>();
            foreach (var i in ifStatement)
            {
                var elseClause = i.DescendantNodes().OfType<ElseClauseSyntax>().Last();
                var startLinePosition = tree.GetLineSpan(elseClause.Span).StartLinePosition.Line;
                var endLinePosition = tree.GetLineSpan(elseClause.Span).EndLinePosition.Line;
                if (endLinePosition - startLinePosition + 1 != 4)
                {
                    if (arrayList.contains(tree.GetLineSpan(elseClause.Span).StartLinePosition.Line.ToString()) != true)
                    {
                        count = count + 1;
                        arrayList.add(tree.GetLineSpan(elseClause.Span).StartLinePosition.Line.ToString());
                        formCheckElseBlock.Show(tree.GetLineSpan(elseClause.Span).StartLinePosition.Line.ToString());
                    }
                }
            }
            Show("The block within Else statement should be 1 line. Probably that line should be a function call. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckForBlock()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var forStatement = root.DescendantNodes().OfType<ForStatementSyntax>();
            foreach (var i in forStatement)
            {
                var startLinePosition = tree.GetLineSpan(i.Span).StartLinePosition.Line;
                var endLinePosition = tree.GetLineSpan(i.Span).EndLinePosition.Line;
                if (endLinePosition - startLinePosition + 1 != 4)
                {
                    count = count + 1;
                    formCheckForBlock.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("The block within For statement should be 1 line. Probably that line should be a function call. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckWhileBlock()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var whileStatement = root.DescendantNodes().OfType<WhileStatementSyntax>();
            foreach (var i in whileStatement)
            {
                var startLinePosition = tree.GetLineSpan(i.Span).StartLinePosition.Line;
                var endLinePosition = tree.GetLineSpan(i.Span).EndLinePosition.Line;
                if (endLinePosition - startLinePosition + 1 != 4)
                {
                    count = count + 1;
                    formCheckWhileBlock.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString());
                }
            }
            Show("The block within While statement should be 1 line. Probably that line should be a function call. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckNestedIf()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var ifStatement1 = root.DescendantNodes().OfType<IfStatementSyntax>();
            foreach (var i in ifStatement1)
            {
                var ifStatement2 = i.DescendantNodes().OfType<IfStatementSyntax>();
                foreach (var j in ifStatement2)
                {
                    var ifStatement3 = j.DescendantNodes().OfType<IfStatementSyntax>();
                    if (ifStatement3.Count() > 0)
                    {
                        count = count + 1;
                        formCheckNestedIf.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(j.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(ifStatement3.First().Span).StartLinePosition.Line.ToString());
                    }
                }
            }
            Show("The nested If statements should not exceed 2 levels. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckNestedFor()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var forStatement1 = root.DescendantNodes().OfType<ForStatementSyntax>();
            foreach (var i in forStatement1)
            {
                var forStatement2 = i.DescendantNodes().OfType<ForStatementSyntax>();
                foreach (var j in forStatement2)
                {
                    var forStatement3 = j.DescendantNodes().OfType<ForStatementSyntax>();
                    if (forStatement3.Count() > 0)
                    {
                        count = count + 1;
                        formCheckNestedFor.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(j.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(forStatement3.First().Span).StartLinePosition.Line.ToString());
                    }
                }
            }
            Show("The nested For statements should not exceed 2 levels. Clean Code book. Page 35.", count.ToString());
        }
        public void CheckNestedWhile()
        {
            int count = 0;
            var tree = CSharpSyntaxTree.ParseText(System.IO.File.ReadAllText(textBox.Text));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var whileStatement1 = root.DescendantNodes().OfType<WhileStatementSyntax>();
            foreach (var i in whileStatement1)
            {
                var whileStatement2 = i.DescendantNodes().OfType<WhileStatementSyntax>();
                foreach (var j in whileStatement2)
                {
                    var whileStatement3 = j.DescendantNodes().OfType<WhileStatementSyntax>();
                    if (whileStatement3.Count() > 0)
                    {
                        count = count + 1;
                        formCheckNestedWhile.Show(tree.GetLineSpan(i.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(j.Span).StartLinePosition.Line.ToString() + " ---> " + tree.GetLineSpan(whileStatement3.First().Span).StartLinePosition.Line.ToString());
                    }
                }
            }
            Show("The nested While statements should not exceed 2 levels. Clean Code book. Page 35.", count.ToString());
        }
    }
}