using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace MiniTC.Model
{
    class DriveInformation
    {
        #region Metody

        public string[] zwrocDyski() { return Directory.GetLogicalDrives(); }
        public string zwrocNadfolder(string sciezka) { return Directory.GetParent(sciezka).FullName; }
        public string[] zwrocPodfoldery(String sciezka)
        {
            string[] foldery = Directory.GetDirectories(sciezka);
            string[] pliki = Directory.GetFiles(sciezka);
            string[] wszystkieFoldery;

            // Zmienna kontynuacja jest indeksem-łącznikiem wyznaczającym w którym momencie należy wstawić foldery/".."/pliki
            int kontynuacja;

            if (sciezka[sciezka.Length - 1] == '\\' && sciezka[sciezka.Length - 2] == ':')
            {
                wszystkieFoldery = new string[foldery.Length + pliki.Length];
                kontynuacja = 0;
            }
            else
            {
                wszystkieFoldery = new string[foldery.Length + 1 + pliki.Length];
                wszystkieFoldery[0] = "..";
                kontynuacja = 1;
            }


            for (int i = 0; i < foldery.Length; i++)
            {
                wszystkieFoldery[kontynuacja] = foldery[i];
                kontynuacja++;
            }

            for (int i = 0; i < pliki.Length; i++)
            {                
                wszystkieFoldery[kontynuacja] = pliki[i];                
                kontynuacja++;
            }

            return wszystkieFoldery;
        }
        public void kopiujPlik(string skad, string dokad)
        {
            try
            {
                // Zmienna mająca na celu umożliwienie uzyskania nazwy pliku z podanej ścieżki skad
                bool zmiana = false;
                string nazwaPliku = "";
                string nowaSciezkaSkad = "";
                string d = dokad;

                for (int i = skad.Length - 1; i >= 0; i--)
                {
                    if (skad[i].Equals('\\') && zmiana == false)
                    {
                        zmiana = true;
                        continue;
                    }

                    if (zmiana == false)
                        nazwaPliku = $"{skad[i]}{nazwaPliku}";
                    else
                        nowaSciezkaSkad = $"{skad[i]}{nowaSciezkaSkad}";
                }

                if (nowaSciezkaSkad.EndsWith(":"))
                    nowaSciezkaSkad = $"{nowaSciezkaSkad}\\";
                string sourceFile = System.IO.Path.Combine(nowaSciezkaSkad, nazwaPliku);
                string destFile = System.IO.Path.Combine(dokad, nazwaPliku);              

                System.IO.File.Copy(sourceFile, destFile);
            }
            catch (System.IO.IOException)
            {                
                MessageBox.Show("Kopiowany plik jest używany przez inny proces lub znajduje się już w folderze docelowym!");
                return;
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Brak dostępu do danej ścieżki!");
                return;
            }
        }
        public bool sprawdzDostep(string path)
        {
            try
            {
                Directory.GetDirectories(path);
                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Nie masz dostępu do tej ścieżki!");
                return false;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Ten dysk nie jest gotowy do użycia!");
                return false;
            }
        }
        public bool sprawdzSciezke(string path)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    return true;
                return false;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Ten dysk nie jest gotowy do użycia!");
                return false;
            }
        }
        #endregion
    }
}
