using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void Adicionar_Produto_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = categoryPicker.SelectedItem?.ToString() ?? "Sem Categoria"
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucesso!", "Produto adicionado.", "OK");
            await Navigation.PopAsync(); // serve para retornar a outra tela
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void EscolherCategoria(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1 && BindingContext is Produto produto)
        {
            // Atualiza a categoria do produto com o valor selecionado
            produto.Categoria = picker.Items[selectedIndex];
        }
    }
}