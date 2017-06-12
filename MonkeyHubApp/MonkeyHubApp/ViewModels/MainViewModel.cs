using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;

namespace MonkeyHubApp.Model
{
    public class MainViewModel : BaseViewModel
    {
        
        private string _searchTerm;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (SetProperty(ref _searchTerm, value))
                    SearchCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Tag> Resultados { get; }
        public Command SearchCommand { get; }
        public Command AboutCommand { get; }

        public MainViewModel()
        {
            SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
            AboutCommand = new Command(ExecuteAboutCommand);
            Resultados = new ObservableCollection<Tag>();
        }

        async void ExecuteAboutCommand()
        {
            await PushAsync<AboutViewModel>();
        }

        bool CanExecuteSearchCommand()
        {
            return !string.IsNullOrWhiteSpace(SearchTerm);
        }

        async void ExecuteSearchCommand()
        {
            //await Task.Delay(1000);

            bool resonse = await App.Current.MainPage.DisplayAlert("Informação MonkeyHub", $"Você pesquisou por {SearchTerm}.", "sim", "não");

            if (resonse)
            {
                await App.Current.MainPage.DisplayAlert(string.Empty, $"Pesquisa feita", "Ok");

                var tagRetornadas = await GetTagsAsync();

                Resultados.Clear();

                if (tagRetornadas != null)
                {
                    foreach (var tag in tagRetornadas)
                        Resultados.Add(tag);

                }
                else
                    await App.Current.MainPage.DisplayAlert("Alerta", $"Nenhum resultado encontrado", "Ok");

            }
            else
                await App.Current.MainPage.DisplayAlert("Alerta", $"Pesquise novamente", "Ok");

        }
    }
}
