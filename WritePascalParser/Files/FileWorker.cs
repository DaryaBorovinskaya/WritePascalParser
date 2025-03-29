using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WritePascalParser.Files
{
    class FileWorker
    {
        public string PathName => _pathName;
        private string _pathName;

        public string GetData()
        {
            if (string.IsNullOrEmpty(PathName)) throw new ArgumentException("Не указан путь до файла");

            return File.ReadAllText(PathName);
        }

        public void Save(string text)
        {
            if (string.IsNullOrEmpty(PathName)) throw new ArgumentException("Не указан путь до файла");

            File.WriteAllText(PathName, text);
        }

        /// <summary>
        /// Метод для изменения пути к файлу.
        /// </summary>
        /// <param name="path"></param>
        public void OnPathChanged(string path)
        {
            _pathName = path;
        }
    }
}
