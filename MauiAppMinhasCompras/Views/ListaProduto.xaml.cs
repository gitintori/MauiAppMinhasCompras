using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    private Produto _produtoSelecionado; // capturar o produto selecionado
    public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
        lst_produtos.ItemSelected += Lst_produtos_ItemSelected; //implementar o evento de produto selecionado
    }

    private void Lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _produtoSelecionado = e.SelectedItem as Produto;
    }

    protected async override void OnAppearing() // Toda vez que a tela aparecer, vai no sqlite buscar a
												// lista de produtos e depois abastecer a observable collection
    {
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));
    }

    private void Botao_Adicionar_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}

    }

	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	{
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));
    }

	private void Botao_Somar_Clicked(object sender, EventArgs e)
	{
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}"; // o C transforma em valor de dinheiro

		DisplayAlert("Total dos produtos", msg, "OK");
	}

    private async void Botao_Remover_Clicked(object sender, EventArgs e)
    {
        if (_produtoSelecionado == null)
        {
            await DisplayAlert("Ops", "Nenhum produto selecionado", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirmar", $"Deseja remover o produto {_produtoSelecionado.Descricao}?", "Sim", "Não");

        if (confirm)
        {
            try
            {
                // Remover o produto do banco de dados usando o ID
                await App.Db.Delete(_produtoSelecionado.Id);

                // Remover o produto da lista
                lista.Remove(_produtoSelecionado);

                await DisplayAlert("Sucesso", "Produto removido com sucesso", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}