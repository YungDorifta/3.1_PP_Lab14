using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаб_14
{
    public partial class Form1 : Form
    {
        private string filename = Path.GetFullPath(Directory.GetCurrentDirectory() + "//..//..//TestFiles//TheFile.txt");

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            Book addedbook = new Book(123, "A", "D", "I", 2000, 10);
            addedbook.Add(filename);
            Book addedbook2 = new Book(123, "S", "N", "I", 100, 50);
            addedbook2.Add(filename);
        }

        /// <summary>
        /// Сохранить книгу с введенными данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBook(object sender, EventArgs e)
        {
            Book addedbook3 = new Book(123, "A", "N", "I", 2000, 150);
            //addedbook.Add(filename);
            addedbook3.Correct(filename, 0);
        }
    }
}
