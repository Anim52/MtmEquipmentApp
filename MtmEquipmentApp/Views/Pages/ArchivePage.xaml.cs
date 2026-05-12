using MtmEquipmentApp.ViewModel.Equipments;
using System.Windows.Controls;

namespace MtmEquipmentApp.Views.Pages
{
    public partial class ArchivePage : UserControl
    {
        public ArchivePage()
        {
            InitializeComponent();
            DataContext = new ArchivePageViewModel();
        }
    }
}
