using Ecng.Serialization;
using StockSharp.Algo;
using StockSharp.Configuration;
using System.IO;
using System.Windows;

namespace FirstConnector
{
	public partial class MainWindow : Window
	{
		Connector connector = new Connector(); // Algo
		string fileName = "Connector";

		public MainWindow()
		{
			InitializeComponent();
			if (File.Exists(fileName)) // load saved configurations
				connector.Load(new XmlSerializer<SettingsStorage>().Deserialize(fileName));

			/* ------------------------ SecurityPicker ------------------------ */
			SecurityPicker.SecurityProvider = connector; // sourse of market instriments
			SecurityPicker.MarketDataProvider = connector;// Data with best price
		}

		private void ButtonConnect_Click(object sender, RoutedEventArgs e)
		{
			if (connector.Configure(this)) // Configuration 
			{
				// save connection configurations in file. If they have already been saved - set them
				new XmlSerializer<SettingsStorage>().Serialize(connector.Save(), fileName);
			}
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e)
		{
			connector.Connect();
		}

		// Обработчик события выделения инструментов панели инструментов
		private void SecurityPicker_SecuritySelected(StockSharp.BusinessEntities.Security security)
		{
			if (security == null) return;
			/* ------------------------ SecurityPicker ------------------------ */
			connector.RegisterSecurity(security); // Security picer - registrate instuument to get market data
		}
	}
}
