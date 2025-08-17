using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Maui.Controls;
using HourTracking.Models;
using HourTracking.Services.MyApp.Services;

namespace HourTracking
{
    public partial class ZileListPage : ContentPage, INotifyPropertyChanged
    {
        private readonly ZileService _service = new();

        public ObservableCollection<Grouping<string, ZiLucru>> GrupuriZile { get; set; } = new();

        private const double PretPeOra = 30.0;

        public ZileListPage()
        {
            InitializeComponent();
            BindingContext = this;
            RefreshList();
        }

        private void RefreshList()
        {
            var zile = _service.GetZile();

            // Calculează bani neîncasati
            double oreNeplatite = zile.Where(z => !z.Platit).Sum(z => z.OreLucrate);
            double baniNeincasati = oreNeplatite * PretPeOra;
            baniNeincasatiLabel.Text = $"Bani neîncasati: {baniNeincasati} lei";

            // Grupează zilele în Plătit / Neplătit
            var grupat = zile
                .GroupBy(z => z.Platit ? "Plătit" : "Neplătit")
                .Select(g => new Grouping<string, ZiLucru>(g.Key, g.ToList()))
                .ToList();

            GrupuriZile.Clear();
            foreach (var grup in grupat)
            {
                GrupuriZile.Add(grup);
            }
        }

        private async void OnZiSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            var ziSelectata = e.CurrentSelection[0] as ZiLucru;
            if (ziSelectata != null)
            {
                await Navigation.PushAsync(new EditZiPage(ziSelectata));
            }

            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnAddZiClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditZiPage(null));
        }

        private async void OnPlatesteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var zi = button?.BindingContext as ZiLucru;
            if (zi == null)
                return;

            zi.Platit = true;
            _service.UpdateZi(zi);
            RefreshList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshList();
        }


        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var zi = button?.BindingContext as ZiLucru;
            if (zi == null)
                return;

            bool confirm = await DisplayAlert("Confirmare", $"Ștergi ziua din {zi.Data:dd/MM/yyyy}?", "Da", "Nu");
            if (!confirm)
                return;

            _service.StergeZi(zi.Id);
            RefreshList();
        }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items) : base(items)
        {
            Key = key;
        }
    }
}
