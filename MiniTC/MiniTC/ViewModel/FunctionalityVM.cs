using MiniTC.Model;
using MiniTC.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTC.ViewModel
{
    class FunctionalityVM : ViewModelBase
    {
        #region Konstruktor

        public FunctionalityVM()
        {
            mainModelObject = new DriveInformation();
            lewy = new PanelClass();
            prawy = new PanelClass();
        }
        #endregion

        #region Obiekty
       
        private DriveInformation mainModelObject;
        private PanelClass lewy;
        private PanelClass prawy;
        #endregion

        #region Właściwości, komendy i metoda
        
        public PanelClass Lewy { get { return lewy; } }
        public PanelClass Prawy { get { return prawy; } }       
        private ICommand _kopiowanie = null;
        public ICommand Kopiowanie
        {
            get
            {
                if (_kopiowanie == null)
                {                    
                    _kopiowanie = new RelayCommand(
                        x =>
                        {
                            mainModelObject.kopiujPlik(lewy.ModelObject.sprawdzSciezke(lewy.Plik) == false ? lewy.Plik : prawy.Plik, prawy.ModelObject.sprawdzSciezke(lewy.Plik) == false ? prawy.Plik : lewy.Plik);
                            odwiezanie();                            
                        },
                        x =>
                        {
                            if (lewy.Plik == null || prawy.Plik == null || (lewy.ModelObject.sprawdzSciezke(lewy.Plik) == false && prawy.ModelObject.sprawdzSciezke(prawy.Plik) == false) || (lewy.ModelObject.sprawdzSciezke(lewy.Plik) == true && prawy.ModelObject.sprawdzSciezke(prawy.Plik) == true))                                                            
                                return false;                            
                            else
                                return true;
                        }
                        );
                }
                return _kopiowanie;
            }
        }
        public void odwiezanie()
        {
            lewy.reset();
            prawy.reset();
        }
        #endregion
    }
}
