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
using System.IO;
using Aspose.Words;
using Aspose.Words.Tables;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(minutes.Text, out int totalMinutes))
            {
                // Определяем выбранный тариф
                if (tarif1.IsChecked == true)
                {
                    CalculateCost(totalMinutes, 0.7, 1.6, 200);
                    limit.Content = $"Минут сверх установленной нормы: {totalMinutes - 200}";
                }
                else if (tarif2.IsChecked == true)
                {
                    CalculateCost(totalMinutes, 0.3, 1.6, 100); // Пример другого тарифа
                    limit.Content = $"Минут сверх установленной нормы: {totalMinutes - 100}"; 
                }
                else
                {
                    limit.Content = "Выберите тариф!";
                }
            }
            else
            {
                limit.Content = "Введите корректное количество минут!";
            }
        }

        private void CalculateCost (int minutes, double rate1, double rate2, int freeMinutes)
        {
            double cost;
            if (minutes <= freeMinutes)
            {
                cost = minutes * rate1;
            }
            else
            {
                int extraMinutes = minutes - freeMinutes;
                cost = (freeMinutes * rate1) + (extraMinutes * rate2);
            }

            payment.Content = $"Стоимость разговора: {cost:F2} руб.";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Устанавливаем шрифт и формат
            builder.Font.Size = 12;
            builder.Font.Name = "Arial";

            // Заголовок
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Bold = true;
            builder.Writeln("ООО Телефонная компания");

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.Font.Bold = false;
            builder.Writeln("ЗАО \"Банк\" г. Москва");
            builder.Writeln("Оплата по счету № АТ-2, в т.ч. НДС (18%)");
            builder.Writeln();

            // Таблица с данными
            Aspose.Words.Tables.Table table = builder.StartTable();

            // Заголовки таблицы
            builder.InsertCell();
            builder.Write("Ф.И.О. плательщика:");
            builder.InsertCell();
            builder.Write("Васильев А.В.");
            builder.EndRow();

            builder.InsertCell();
            builder.Write("Адрес плательщика:");
            builder.InsertCell();
            builder.Write("129626, г. Москва, проспект Мира, д. 10");
            builder.EndRow();

            builder.InsertCell();
            builder.Write("Сумма платежа:");
            builder.InsertCell();
            builder.Write($"{payment.Content} руб.");
            builder.EndRow();

            builder.InsertCell();
            builder.Write("Дата:");
            builder.InsertCell();
            builder.Write($"{DateTime.Now:dd.MM.yyyy}");
            builder.EndRow();

            builder.EndTable();
            builder.Writeln();

            // Дополнительный текст
            builder.Writeln("С условиями приема указанной в платежном документе суммы согласен.");
            builder.Writeln();
            builder.Writeln("Плательщик (подпись): ___________________");

            // Сохранение документа в PDF
            string filePath = "C:\\Users\\nvidi\\OneDrive\\Рабочий стол\\Квитанция.pdf";
            doc.Save(filePath);

            // Уведомление пользователя
            MessageBox.Show($"Квитанция сохранена в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
