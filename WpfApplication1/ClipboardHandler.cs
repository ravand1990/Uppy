using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Forms;
using Clipboard = System.Windows.Forms.Clipboard;
using DataFormats = System.Windows.DataFormats;
using IDataObject = System.Windows.IDataObject;
using MessageBox = System.Windows.MessageBox;

namespace Uppy
{
    internal class ClipboardHandler
    {
        private readonly Clipboard clip;
        private Window window;
        public ClipboardHandler()
        {
        }


        public ClipboardHandler(Window window)
        {
            this.window = window;
        }
        public void saveClipboard()
        {
            IDataObject clip = System.Windows.Clipboard.GetDataObject();
            StringCollection files = System.Windows.Clipboard.GetFileDropList();
            StringEnumerator enumerator = files.GetEnumerator();

            String date = DateTime.UtcNow.ToShortDateString();

            if (Clipboard.ContainsText())
            {
                Clipboard.GetText();
                using (StreamWriter p = new StreamWriter(MemberWindow.SAVEFOLDER + "/test.txt"))
                {
                    p.Write(Clipboard.GetText());
                }
          }
            if (Clipboard.ContainsData(DataFormats.Html))
            {
                var data = Clipboard.GetData(DataFormats.Html);
                using (StreamWriter p = new StreamWriter(MemberWindow.SAVEFOLDER + "/test_html.txt"))
                {
                    p.Write(data);
                }
            }




            var fileList = new Dictionary<int, string>();

            int enumCount = 1;

            while (enumerator.MoveNext())
            {
                fileList.Add(enumCount, enumerator.Current);
                enumCount++;
            }

            if (fileList.Count > 1)
            {
                using (
                    ZipArchive zip = ZipFile.Open(MemberWindow.SAVEFOLDER + "/" + date + ".zip", ZipArchiveMode.Create))
                {
                    foreach (String key in fileList.Values)
                    {
                        var file = new FileInfo(key);
                        String name = file.Name;
                        String format = file.Extension;


                        zip.CreateEntryFromFile(key, name + format);
                    }
                }
            }
            else
            {
                foreach (String key in fileList.Values)
                {
                    var file = new FileInfo(key);
                    String name = file.Name;
                    String format = file.Extension;

                    using (var fs = new FileStream(key, FileMode.Open))
                    {
                        using (var fs2 = new FileStream(MemberWindow.SAVEFOLDER + "/" + name, FileMode.Create))
                        {
                            fs.CopyTo(fs2);
                        }
                    }
                }
            }
        }

        public void dumpClipboard()
        {
            StringCollection files = Clipboard.GetFileDropList();
            StringEnumerator stringEnum = files.GetEnumerator();
            Dictionary<int,string> list = new Dictionary<int, string>();

            int count = 1;
            while (stringEnum.MoveNext())
            {
                list.Add(count,stringEnum.Current);
                count++;
            }

            (window as MemberWindow).textBox1.Text = "";

            foreach (String key in list.Values)
            {

                (window as MemberWindow).textBox1.Text += key + "\n";


            }
        }
    }
}