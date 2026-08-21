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
            var tmp = await App.Db.GetAll();

            lista.Clear();
                foreach (var item in tmp)
                {
                    lista.Add(item);
                }
           
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
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
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

     
    private async void ToolbarItem_Clicked_Refresh(object sender, EventArgs e)
    {
        await AtualizarLista();
    }

    private async void Button_Clicked_Refresh(object sender, EventArgs e)
    {
        await AtualizarLista();
    }
    private async void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
        double total = lista.Sum(i => i.Total);
        string msg = $"O total dos produtos é {total:C}";
        await DisplayAlertAsync("Somatório", msg, "OK");
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
            bool confirm = await DisplayAlertAsync("Tem certeza?", $"Remover {item.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(item);
                await AtualizarLista();
            }
        }
    }

    private async void TapGestureRecognizer_Tapped_Editar(object sender, TappedEventArgs e)
    {
        var grid = sender as Grid;
        var item = grid?.BindingContext as Produto;

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
    private async void BtnExcluir_Clicked(object sender, EventArgs e)
    {
        Button botao = (Button)sender;

        Produto produto = (Produto)botao.CommandParameter;

        bool confirmar = await DisplayAlertAsync(
            "Excluir produto",
            $"Deseja excluir o produto {produto.Descricao}?",
            "Sim",
            "Não");

        if (confirmar)
        {
            await App.Db.Delete(produto);

            AtualizarLista();
        }
    }
}




