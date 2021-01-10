using CookBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data;

namespace Proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        CookBookEntitiesModel cbx = new CookBookEntitiesModel();
        CollectionViewSource recipeViewSource;
        CollectionViewSource ingredientViewSource;
        CollectionViewSource recipeRecipeIngredientsViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            recipeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("recipeViewSource")));
            recipeViewSource.Source = cbx.Recipes.Local;
            cbx.Recipes.Load();


            ingredientViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ingredientViewSource")));
            ingredientViewSource.Source = cbx.Ingredients.Local;
            cbx.Ingredients.Load();

            recipeRecipeIngredientsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("recipeRecipeIngredientsViewSource")));
            recipeRecipeIngredientsViewSource.Source = cbx.RecipeIngredients.Local;
            cbx.RecipeIngredients.Load();

            cmbRecipe.ItemsSource = cbx.Recipes.Local;
            //cmbRecipe.DisplayMemberPath = "Name";
            cmbRecipe.SelectedValuePath = "Id";

            cmbIngredient.ItemsSource = cbx.Ingredients.Local;
           // cmbIngredient.DisplayMemberPath = "Name";
            cmbIngredient.SelectedValuePath = "Id";

        }

        private void btnSaveR_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipe = new Recipe();
            if (action == ActionState.New)
            {
                try
                {
                    recipe = new Recipe()
                    {
                        Name = nameTextBox.Text.Trim(),
                        Steps = stepsTextBox.Text.Trim()
                    };
                    cbx.Recipes.Add(recipe);
                    recipeViewSource.View.Refresh();
                    cbx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                    if (action == ActionState.Edit)
            {
                try
                {
                    recipe = (Recipe)recipeDataGrid.SelectedItem;
                    recipe.Name = nameTextBox.Text.Trim();
                    recipe.Steps = stepsTextBox.Text.Trim();
                    cbx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                recipeViewSource.View.Refresh();
                recipeViewSource.View.MoveCurrentTo(recipe);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    recipe = (Recipe)recipeDataGrid.SelectedItem;
                    cbx.Recipes.Remove(recipe);
                    cbx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                recipeViewSource.View.Refresh();
            }
        }

        private void btnNewR_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewR.IsEnabled = false;
            btnEditR.IsEnabled = false;
            btnDelecteR.IsEnabled = false;
            btnSaveR.IsEnabled = true;
            btnCancelR.IsEnabled = true;

            nameTextBox.IsEnabled = true;
            stepsTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(nameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(stepsTextBox, TextBox.TextProperty);

            Keyboard.Focus(nameTextBox);
        }

        private void btnEditR_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnEditR.IsEnabled = false;
            btnNewR.IsEnabled = false;
            btnDelecteR.IsEnabled = false;
            btnCancelR.IsEnabled = true;
            btnSaveR.IsEnabled = true;


            nameTextBox.IsEnabled = true;
            stepsTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(nameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(stepsTextBox, TextBox.TextProperty);

            Keyboard.Focus(nameTextBox);
        }

        private void btnDelecteR_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempName = nameTextBox.Text.ToString();
            string tempSteps = stepsTextBox.Text.ToString();

            btnNewR.IsEnabled = false;
            btnEditR.IsEnabled = false;
            btnDelecteR.IsEnabled = false;

            btnSaveR.IsEnabled = true;
            btnCancelR.IsEnabled = true;



            BindingOperations.ClearBinding(nameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(stepsTextBox, TextBox.TextProperty);
            nameTextBox.Text = tempName;
            stepsTextBox.Text = tempSteps;
        }

        private void btnCancelR_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNewR.IsEnabled = true;
            btnEditR.IsEnabled = true;
            btnSaveR.IsEnabled = false;
            btnCancelR.IsEnabled = false;
            btnDelecteR.IsEnabled = true;
            nameTextBox.IsEnabled = false;
            stepsTextBox.IsEnabled = false;



        }

        private void btnSaveI_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = new Ingredient();
            if (action == ActionState.New)
            {
                try
                {
                    ingredient = new Ingredient()
                    {
                        Name = nameTextBox1.Text.Trim()
                    };
                    cbx.Ingredients.Add(ingredient);
                    ingredientViewSource.View.Refresh();
                    cbx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                    if (action == ActionState.Edit)
            {
                try
                {
                    ingredient = (Ingredient)ingredientDataGrid.SelectedItem;
                    ingredient.Name = nameTextBox1.Text.Trim();
                    cbx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ingredientViewSource.View.Refresh();
                ingredientViewSource.View.MoveCurrentTo(ingredient);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    ingredient = (Ingredient)ingredientDataGrid.SelectedItem;
                    cbx.Ingredients.Remove(ingredient);
                    cbx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ingredientViewSource.View.Refresh();
            }
        }

        private void btnNewI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewI.IsEnabled = false;
            btnEditI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;
            btnSaveI.IsEnabled = true;
            btnCancelI.IsEnabled = true;

            nameTextBox1.IsEnabled = true;


            BindingOperations.ClearBinding(nameTextBox1, TextBox.TextProperty);


            Keyboard.Focus(nameTextBox1);
        }

        private void btnEditI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnEditI.IsEnabled = false;
            btnNewI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;
            btnCancelI.IsEnabled = true;
            btnSaveI.IsEnabled = true;


            nameTextBox1.IsEnabled = true;


            BindingOperations.ClearBinding(nameTextBox1, TextBox.TextProperty);


            Keyboard.Focus(nameTextBox1);
        }

        private void btnDeleteI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempName = nameTextBox1.Text.ToString();

            btnNewI.IsEnabled = false;
            btnEditI.IsEnabled = false;
            btnDeleteI.IsEnabled = false;

            btnSaveI.IsEnabled = true;
            btnCancelI.IsEnabled = true;



            BindingOperations.ClearBinding(nameTextBox1, TextBox.TextProperty);

            nameTextBox1.Text = tempName;

        }

        private void btnCancelI_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNewI.IsEnabled = true;
            btnEditI.IsEnabled = true;
            btnSaveI.IsEnabled = false;
            btnCancelI.IsEnabled = false;
            btnDeleteI.IsEnabled = true;
            nameTextBox1.IsEnabled = false;

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            recipeRecipeIngredientsViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            recipeRecipeIngredientsViewSource.View.MoveCurrentToNext();
        }
    }
        }
       
        

    

            
    

