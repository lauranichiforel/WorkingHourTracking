using System;
using Microsoft.Maui.Controls;
using HourTracking.Models;
using HourTracking.Services.MyApp.Services;

namespace HourTracking
{
    public partial class EditZiPage : ContentPage
    {
        private readonly ZileService _service = new();
        private ZiLucru _zi;

        public EditZiPage(ZiLucru zi)
        {
            InitializeComponent();

            if (zi == null)
            {
                _zi = new ZiLucru { Data = DateTime.Today, OreLucrate = 0, Comentariu = "", Platit = false };
            }
            else
            {
                _zi = zi;
            }

            datePicker.Date = _zi.Data;
            oreEntry.Text = _zi.OreLucrate.ToString("F1");
            comentariuEditor.Text = _zi.Comentariu;
            platitSwitch.IsToggled = _zi.Platit;
        }

        private async void OnSalveazaClicked(object sender, EventArgs e)
        {

            if (!double.TryParse(oreEntry.Text, out double ore))
            {
                await DisplayAlert("Eroare", "Introdu un număr valid pentru ore.", "OK");
                return;
            }

            ore = Math.Round(ore, 1);

            _zi.Data = datePicker.Date;
            _zi.OreLucrate = ore;
            _zi.Comentariu = comentariuEditor.Text ?? "";
            _zi.Platit = platitSwitch.IsToggled;

            if (_zi.Id == 0)
            {
                _service.AdaugaZi(_zi);
            }
            else
            {
                _service.UpdateZi(_zi);
            }

            await Navigation.PopAsync();
        }
    }
}
