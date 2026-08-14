using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void Button_Clicked_Salvar(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlertAsync("Atenção", "Preencha a descrição do produto", "OK");
                return;
            }

            Produto p = BindingContext as Produto ?? new Produto();

            p.Descricao = txt_descricao.Text;
            p.Quantidade = Convert.ToDouble(txt_quantidade.Text);
            p.Preco = Convert.ToDouble(txt_preco.Text);

            if (p.Id > 0)
            {
               
                await App.Db.Update(p);
                await DisplayAlertAsync("Sucesso!", "Produto atualizado com sucesso!", "OK");
            }
            else
            {
              
                await App.Db.Insert(p);
                await DisplayAlertAsync("Sucesso!", "Produto inserido com sucesso!", "OK");
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

}