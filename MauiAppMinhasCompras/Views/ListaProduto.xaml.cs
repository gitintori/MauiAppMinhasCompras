using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    private Produto _produtoSelecionado; // variável que armazena o produto selecionado

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            // Define "Todas" como selecionado por padrão
            categoryPicker.SelectedIndex = 0; // "Todas" é o primeiro item (índice 0)

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void Adicionar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void EscolherCategoria (object sender, EventArgs e)
    {

        try
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            if (picker == null) return;

            string selectedCategory = picker.Items[selectedIndex];

            lst_produtos.IsRefreshing = true;
            lista.Clear();

            List<Produto> tmp;

            if (selectedCategory == "Todas")
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                tmp = await App.Db.FiltrarPorCategoria(selectedCategory);
            }

            // Verifica se há produtos na categoria selecionada
            if (!tmp.Any() && selectedCategory != "Todas")
            {
                await DisplayAlert("Aviso", $"Não há produtos cadastrados na categoria '{selectedCategory}'.", "OK");
                // Volta para mostrar "Todas"
                picker.SelectedIndex = 0;
                tmp = await App.Db.GetAll();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void Somar_Clicked(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"Total: {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private void Editar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica se um produto foi selecionado
            if (_produtoSelecionado == null)
            {
                DisplayAlert("Aviso", "Selecione um produto para editar.", "OK");
                return;
            }

            // Navega para a página de edição, passando o produto selecionado
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = _produtoSelecionado,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    private async void Remover_Clicked(object sender, EventArgs e)
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

                await DisplayAlert("Sucesso", "Produto removido com sucesso.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }


    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Armazena o produto selecionado
            _produtoSelecionado = e.SelectedItem as Produto;

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

}