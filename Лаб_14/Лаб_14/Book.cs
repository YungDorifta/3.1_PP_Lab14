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
        //поля класса - данные объекта
        private int inventoryNumber;
        private byte[] author;
        private byte[] nameOfBook;
        private byte[] izdatelstvo;
        private int year;
        private int pages;

        [NonSerialized]
        const int max_length = 20;
        const int correctionIndex = sizeof(int) * 3 + max_length * 3;
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
        /// Получение длины кол-ва символов, содержащихся в 1 сериализуемом объекте
        /// </summary>
        /// <returns></returns>
        public static int GetCorrectionIndex()
        {
            return correctionIndex;
        }

        /// <summary>
        /// Конструктор объекта Book
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
        /// Добавление объекта Book в конец бинарного файла
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

        /// <summary>
        /// Вывод всех данных из массива объектов Book в таблицу
        /// </summary>
        /// <param name="books">массив книг</param>
        /// <param name="table">таблица в окне</param>
        public static void ViewInTable(Book[] books, DataGridView table) {
            table.RowCount = books.Length;
            table.ColumnCount = 6;
            for (int i = 0; i < books.Length; i++)
            {
                table[0, i].Value = books[i].inventoryNumber.ToString();
                table[1, i].Value = ByteArrayToString(books[i].author);
                table[2, i].Value = ByteArrayToString(books[i].nameOfBook);
                table[3, i].Value = ByteArrayToString(books[i].izdatelstvo);
                table[4, i].Value = books[i].year.ToString();
                table[5, i].Value = books[i].pages.ToString();
            }
        }

        //!!!метод поиска в бинарном файле записей
        public static Book[] FindBooks(string filename)
        {
            Book[] books = new Book[10];


            return books;
        }

        /// <summary>
        /// Поиск в бинарном файле записей, удовлетворяющих критерию
        /// </summary>
        /// <param name="filename">Путь к файлу</param>
        /// <param name="authorToFind">Имя автора</param>
        /// <returns></returns>
        public static Book[] FindBooksWithAuthor(string filename, string authorToFind)
        {
            //изъять все книги из файла
            Book[] allBooks = FindBooks(filename);

            //подсчитать кол-во книг с указанным автороом
            int authorBooksCount = 0;
            foreach (Book book in allBooks)
            {
                if (ByteArrayToString(book.author) == authorToFind) authorBooksCount++;
            }

            //создать массив с книгами с указанным автором
            Book[] authorBooks = new Book[authorBooksCount];
            authorBooksCount = 0;
            foreach (Book book in allBooks)
            {
                if (ByteArrayToString(book.author) == authorToFind)
                {
                    authorBooks[authorBooksCount] = book;
                    authorBooksCount++;
                }   
            }

            //вернуть массив с найденными книгами
            return authorBooks;
        }


        //!!!переделать: корректировка записи не с начала не работает
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
        /// Сравнение книг по кол-ву страниц
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
