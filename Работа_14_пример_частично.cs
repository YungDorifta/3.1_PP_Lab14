using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Работа_с_бинарным_файлом
{
	[Serializable]
    class Note
    {
        DateTime date;
        int t_Day;      //температура днем
        int t_Naigh;    //температура ночью
        byte comment;   //коммент к погоде



        //constructor
        public Note(DateTime date, int t_Day, int t_Naigh, string comment)
        {
            //ввод полец, кастом метод
            this.comment = StringToByte(comment);
        }

        //метод сохранения результата
        public void Save_Result(string filename)
        {
            FileStream fa = new FileStream(filename, FileMode.Append);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Deserialize(); //???
            fa.Close();
        }
    }
}
