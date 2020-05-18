using MiniTC.Model;
using MiniTC.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTC.ViewModel
{
    class PanelClass : ViewModelBase
    {
        #region Konstruktor

        public PanelClass()
        {
            _modelObject = new DriveInformation();
            ListaDyskow = ListaDyskow;
            WybranyDysk = _listaDyskow[0];
            _wybranyFolder = -1;
            Sciezka = WybranyDysk;
            ListaFolderow = new string[] { "1", "2" };
            odswiez();
        }
        #endregion

        #region Pola prywatne           
        private DriveInformation _modelObject;
        private string _sciezka = null;
        private string _wybranyDysk = null;
        private string[] _listaDyskow = null;
        private string[] _listaFolderow = null;
        private int _wybranyFolder = -1;
        private string _plik = "";

        private ICommand _odswiezDyski = null;
        #endregion

        #region Właściwości

        public DriveInformation ModelObject { get { return _modelObject; } }
        public string Sciezka
        {
            get { return _sciezka; }
            set { _sciezka = value; }
        }
        public string WybranyDysk
        {
            get { return _wybranyDysk; }
            set
            {
                bool sprawdzDysk = _modelObject.sprawdzSciezke(value);
                if (_wybranyDysk != value && sprawdzDysk == true)
                {
                    _wybranyDysk = value;
                    Sciezka = WybranyDysk;
                    Plik = WybranyDysk;
                    ListaFolderow = ListaFolderow;
                    _wybranyFolder = -1;
                    onPropertyChanged(nameof(Sciezka), nameof(ListaFolderow));
                }
            }
        }
        public string[] ListaDyskow
        {
            get
            {
                _listaDyskow = _modelObject.zwrocDyski();
                return _listaDyskow;
            }
            set
            {
                _listaDyskow = _modelObject.zwrocDyski();
                onPropertyChanged(nameof(ListaDyskow));
            }
        }
        public int WybranyFolder
        {
            get { return _wybranyFolder; }
            set
            {                
                if (value != -1)
                {
                    _wybranyFolder = value;
                    string faktycznaSciezka = "";
                    if (!_listaFolderow[_wybranyFolder].Equals(".."))
                    {
                        if (_sciezka.EndsWith("\\"))
                            faktycznaSciezka = $"{_sciezka}{_listaFolderow[_wybranyFolder].Substring(3)}";
                        else
                            faktycznaSciezka = $"{_sciezka}\\{_listaFolderow[_wybranyFolder].Substring(3)}";
                    }
                    // Sprawdzenie, czy wybrano ".."
                    
                    if (_listaFolderow[_wybranyFolder].Equals(".."))
                    {
                        _sciezka = _modelObject.zwrocNadfolder(Sciezka);
                        Plik = Sciezka;
                    }
                    // Sprawdzenie, czy wybrano folder
                    else if (_modelObject.sprawdzSciezke(faktycznaSciezka) == true)
                    {
                        // Sprawdzenie, czy mamy dostęp do tego folderu
                        if (_modelObject.sprawdzDostep(faktycznaSciezka) == true)
                        {
                            _sciezka = faktycznaSciezka;
                            Plik = Sciezka;
                        }
                        else
                        {
                            _wybranyFolder = -1;
                            return;
                        }
                    }
                    // Wybrano plik
                    else
                    {
                        Plik = faktycznaSciezka;
                        return;
                    }

                    _wybranyFolder = -1;
                    ListaFolderow = ListaFolderow;

                    onPropertyChanged(nameof(Sciezka), nameof(ListaFolderow));
                }
            }
        }
        public string[] ListaFolderow
        {
            get { return _listaFolderow; }
            set
            {                
                string[] tempFoldery = _modelObject.zwrocPodfoldery(Sciezka);
                string[] finalneFoldery = new string[tempFoldery.Length];
                int finalneIterator = 0;
                string tempFilename = "";
                for (int i = 0; i < tempFoldery.Length; i++)
                {
                    for (int j = tempFoldery[i].Length - 1; j >= 0; j--)
                    {
                        if (tempFoldery[i][j].Equals('\\'))
                            break;
                        else
                            tempFilename = $"{tempFoldery[i][j]}{tempFilename}";
                        if (tempFilename.Equals(".."))
                            break;
                    }
                    if (tempFilename.Equals(".."))
                        finalneFoldery[finalneIterator] = tempFilename;
                    else
                        finalneFoldery[finalneIterator] = $"<{tempFoldery[i].Substring(0,1)}>{tempFilename}";
                    tempFilename = "";
                    finalneIterator++;
                }
                _listaFolderow = finalneFoldery;
            }
        }
        public string Plik
        {
            get { return _plik; }
            set
            {
                if (value != null)
                {
                    _plik = value;
                    onPropertyChanged(nameof(Plik));
                }
            }
        }
        #endregion

        #region Komendy i funkcje

        public ICommand OdswiezDyski
        {
            get
            {
                if (_odswiezDyski == null)
                    _odswiezDyski = new RelayCommand(
                        arg =>
                        {
                            ListaDyskow = ListaDyskow;
                            MessageBox.Show("Odświeżono dyski");
                            onPropertyChanged(nameof(ListaDyskow));
                        },
                        arg => true
                        );
                return _odswiezDyski;
            }
        }
        public void odswiez() { onPropertyChanged(nameof(ListaDyskow), nameof(WybranyDysk), nameof(Sciezka), nameof(ListaFolderow), nameof(Plik)); }
        public void reset()
        {
            WybranyFolder = -1;
            ListaFolderow = ListaFolderow;
            Plik = Sciezka;
            odswiez();
        }
        #endregion
    }
}
