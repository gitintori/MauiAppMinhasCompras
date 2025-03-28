using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Configura o Picker com a categoria atual do produto
        if (BindingContext is Produto produto)
        {
            txt_descricao.Text = produto.Descricao;
            txt_quantidade.Text = produto.Quantidade.ToString();
            txt_preco.Text = produto.Preco.ToString();

            // Seleciona a categoria atual no Picker
            if (!string.IsNullOrEmpty(produto.Categoria))
            {
                int index = categoryPicker.Items.IndexOf(produto.Categoria);
                categoryPicker.SelectedIndex = index != -1 ? index : 0;
            }
        }
    }

    private async void Salvar_Edicao_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = produto_anexado.Categoria
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Produto atualizado.", "OK");
            await Navigation.PopAsync();
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