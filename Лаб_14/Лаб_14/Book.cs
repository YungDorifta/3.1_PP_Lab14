using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Лаб_14
{
    [Serializable]
    class Book : IComparable
    {
        private int inventoryNumber;
        private byte[] author;
        private byte[] nameOfBook;
        private byte[] izdatelstvo;
        private int year;
        private int pages;

        [NonSerialized]
        //не публичной сделать константу
        public const int max_length = 20;
        static Encoding encoding = System.Text.Encoding.GetEncoding(1251);

        /// <summary>
        /// Конвертер строки в последовательность байт
        /// </summary>
        /// <param name="s">Входная строка</param>
        /// <returns></returns>
        private byte[] StringToByte(string s)
        {
            char[] ar = new char[max_length];
            for (int i = 0; i < max_length; i++)
            {
                if (i < s.Length) ar[i] = s[i];
                else ar[i] = ' ';
            }
            byte[] buff = encoding.GetBytes(ar);
            return buff;
        }

        /// <summary>
        /// Конвертер из последовательности байт в строку
        /// </summary>
        /// <param name="ar">Массив байт</param>
        /// <returns>Строка</returns>
        private static string ByteArrayToString(byte[] b)
        {
            char[] buff = encoding.GetChars(b);
            string result = "";
            for (int i = 0; i < buff.Length; i++)
            {
                result += (char)buff[i];
            }
            return result;
        }
        
        /// <summary>
        /// Конструктор объекта
        /// </summary>
        /// <param name="inventoryNumber">Инфентарный номер</param>
        /// <param name="author">Автор книги</param>
        /// <param name="nameOfBook">Название книги</param>
        /// <param name="izdatelstvo">Издательство</param>
        /// <param name="year">Год издания</param>
        /// <param name="pages">Кол-во страниц</param>
        public Book(int inventoryNumber, string author, string nameOfBook, string izdatelstvo, int year, int pages)
        {
            this.inventoryNumber = inventoryNumber;
            this.author = StringToByte(author);
            this.nameOfBook = StringToByte(nameOfBook);
            this.izdatelstvo = StringToByte(izdatelstvo);
            this.year = year;
            this.pages = pages;
        }

        /// <summary>
        /// Метод добавления объекта Book в бинарный файл
        /// </summary>
        /// <param name="filename">Путь к бинарному файлу</param>
        public void Add(string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Append);
                BinaryFormatter bw = new BinaryFormatter();
                bw.Serialize(fs, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fs.Close();
            }
        }

        //метод вывода в таблицу
        public static void ViewInTable(Book[] books, DataGrid table) {
            
        }

        //поиск в бинарном файле записей
        public static Book[] FindBooks(string filename)
        {
            Book[] books = new Book[10];
            return books;
        }

        //

        /// <summary>
        /// Корректировка записи об объекте Book в файле
        /// </summary>
        /// <param name="filename">Путь к файлу</param>
        /// <param name="num">Байт, с которого начинается запись</param>
        /// <returns></returns>
        public bool Correct(string filename, long num)
        {
            bool result = false;
            if (num >= 0)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(filename, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    // установим указатель файла на запись с позицией num
                    fs.Seek(num, SeekOrigin.Begin);
                    // записываем в файл изменения 
                    bf.Serialize(fs, this);                             
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //закрытие потока записи  
                    fs.Close();      
                }
            }
            return result;
        }
        
        /// <summary>
        /// Метод сравнения объектов Book
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (pages > (obj as Book).pages) return 1;
            else if (pages < (obj as Book).pages) return -1;
            else return 0;
        }
    }
}
