using HourTracking.Models;
using HourTracking.Services.MyApp.Services;

namespace HourTracking
{
    public partial class MainPage : ContentPage
    {
        private readonly ZileService _service = new();

        public MainPage()
        {
            InitializeComponent();
            RefreshUI();
        }

        private void OnSalveazaClicked(object sender, EventArgs e)
        {
            if (double.TryParse(oreEntry.Text, out double ore) && ore >= 0)
            {
                var zi = new ZiLucru
                {
                    Data = datePicker.Date,
                    OreLucrate = ore,
                    Comentariu = comentariuEditor.Text ?? string.Empty,
                    Platit = platitSwitch.IsToggled
                };

                _service.AdaugaZi(zi);
                ClearForm();
                RefreshUI();
            }
            else
            {
                DisplayAlert("Eroare", "Introdu un număr valid de ore.", "OK");
            }
        }

        private void RefreshUI()
        {
            var zile = _service.GetZile();
            zileList.ItemsSource = zile;

            double tarifPeOra = Preferences.Default.Get("TarifPeOra", 25.0); // Exemplu tarif
            double oreNeplatite = _service.CalculeazaOreNeplatite();

            labelBani.Text = $"Bani neîncasați: {oreNeplatite * tarifPeOra:0.00} lei";
        }

        private void ClearForm()
        {
            oreEntry.Text = string.Empty;
            comentariuEditor.Text = string.Empty;
            platitSwitch.IsToggled = false;
        }

        private void OnZiSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            if (e.CurrentSelection[0] is ZiLucru zi)

            {
                datePicker.Date = zi.Data;
                oreEntry.Text = zi.OreLucrate.ToString();
                comentariuEditor.Text = zi.Comentariu;
                platitSwitch.IsToggled = zi.Platit;
            }
        }
    }

}
