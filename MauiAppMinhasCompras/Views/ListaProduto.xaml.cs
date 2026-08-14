using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AtualizarLista();
    }

    private async Task AtualizarLista()
    {
        try
        {
            lista.Clear();
            var tmp = await App.Db.GetAll();
            foreach (var item in tmp)
            {
                lista.Add(item);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            lista.Clear();
            var tmp = await App.Db.Search(q);
            foreach (var item in tmp)
            {
                lista.Add(item);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
        double total = lista.Sum(i => i.Total);
        string msg = $"O total dos produtos é {total:C}";
        await DisplayAlert("Somatório", msg, "OK");
    }

    private async void ToolbarItem_Clicked_Adicionar(object sender, EventArgs e)
    {
       
        await Navigation.PushAsync(new NovoProduto());
    }

    private async void SwipeItem_Invoked_Excluir(object sender, EventArgs e)
    {
        var item = (sender as SwipeItem)?.CommandParameter as Produto;

        if (item != null)
        {
            bool confirm = await DisplayAlert("Tem certeza?", $"Remover {item.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(item.Id);
                await AtualizarLista();
            }
        }
    }

    private async void TapGestureRecognizer_Tapped_Editar(object sender, EventArgs e)
    {
        var item = (sender as TappedEventArgs)?.Parameter as Produto;

        if (item != null)
        {
           
            await Navigation.PushAsync(new EditarProduto
            {
                BindingContext = item
            });
        }
    }

    private async void ref_carregando_Refreshing(object sender, EventArgs e)
    {
        await AtualizarLista();
        ref_carregando.IsRefreshing = false;
    }
}




