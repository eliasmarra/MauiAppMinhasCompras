using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    Produto produto;

    public EditarProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        produto = BindingContext as Produto;

        if (produto != null)
        {
            txt_descricao.Text = produto.Descricao;
            txt_quantidade.Text = produto.Quantidade.ToString();
            txt_preco.Text = produto.Preco.ToString();
        }
    }

    private async void Button_Clicked_Salvar(object sender, EventArgs e)
    {
        try
        {
            if (produto == null)
            {
                await DisplayAlertAsync("Erro", "Produto não encontrado.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlertAsync("Atenção", "Digite a descrição do produto.", "OK");
                return;
            }

            if (!double.TryParse(txt_quantidade.Text, out double quantidade))
            {
                await DisplayAlertAsync("Atenção", "Digite uma quantidade válida.", "OK");
                return;
            }

            if (!double.TryParse(txt_preco.Text, out double preco))
            {
                await DisplayAlertAsync("Atenção", "Digite um preço válido.", "OK");
                return;
            }

            produto.Descricao = txt_descricao.Text;
            produto.Quantidade = quantidade;
            produto.Preco = preco;

            await App.Db.Update(produto);

            await DisplayAlertAsync(
                "Sucesso",
                "Produto alterado com sucesso!",
                "OK"
            );

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}