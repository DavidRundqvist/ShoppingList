using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace SLApp
{
	public partial class App : Application
	{
		public App (ISpeech speech)
		{
			InitializeComponent();

		    var slUri = new Uri("http://gulnar.myqnapcloud.com/ShoppingList");
		    var client = new SLClient(slUri);
            MainPage = new MainPage(speech, client, slUri);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
