using System;
using System.Linq;
using System.Windows.Forms;
using boyBibl.classBook;

namespace boyBibl
{
    public partial class MainForm : Form
    {
        private BookManager bookManager; // Менеджер для работы с книгами
        public bool LogoutRequested { get; private set; } // Флаг для выхода из системы

        public MainForm(string role)
        {
            InitializeComponent();

            // Инициализация BookManager
            bookManager = new BookManager();

            // Установка роли пользователя
            labelRole.Text = $"Роль: {role}";

            // Отключаем кнопки для роли пользователя
            if (role == "User")
            {
                btnaddbook.Enabled = false; // "Добавить книгу"
                btnDeletebook.Enabled = false; // "Удалить книгу"
                bunifuButton24.Enabled = false; // "Показать список"
            }

            InitializeDataGridView(); // Инициализация DataGridView
        }

        private void InitializeDataGridView()
        {
            // Настройка столбцов DataGridView
            bunifuDataGridView1.Columns.Clear();
            bunifuDataGridView1.Columns.Add("Id", "ID");
            bunifuDataGridView1.Columns.Add("Title", "Название книги");
            bunifuDataGridView1.Columns.Add("Author", "Автор");
            bunifuDataGridView1.Columns.Add("Year", "Год издания");

            // Скрыть столбец ID (если необходимо)
            bunifuDataGridView1.Columns["Id"].Visible = true;
        }

        private void UpdateDataGridView()
        {
            bunifuDataGridView1.Rows.Clear();
            var books = bookManager.PrintAllBooks();

            foreach (var book in books)
            {
                bunifuDataGridView1.Rows.Add(book.Id, book.Title, book.Author, book.Year);
            }

            // Проверяем количество книг после обновления
            Console.WriteLine($"Количество книг в DataGridView: {bunifuDataGridView1.Rows.Count}");
        }

        private void ClearInputFields()
        {
            bunifuTextBox1.Text = ""; // Название книги
            bunifuTextBox2.Text = ""; // Автор
            bunifuTextBox3.Text = ""; // Год
        }

        private void btnaddbook_Click(object sender, EventArgs e) // "Добавить книгу"
        {
            string title = bunifuTextBox1.Text;
            string author = bunifuTextBox2.Text;
            string yearText = bunifuTextBox3.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(yearText))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(yearText, out int year) || year <= 0)
            {
                MessageBox.Show("Введите корректный год.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Добавление книги через BookManager
            bookManager.AddBook(title, author, year);

            UpdateDataGridView();
            ClearInputFields();
        }

        private void btnDeletebook_Click(object sender, EventArgs e) // "Удалить книгу"
        {
            if (bunifuDataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in bunifuDataGridView1.SelectedRows)
                {
                    Guid id = Guid.Parse(row.Cells["Id"].Value.ToString());
                    bookManager.RemoveBook(id);
                }

                UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDGW_Click(object sender, EventArgs e) // "Показать список"
        {
            UpdateDataGridView();
        }

        private void expBook_Click(object sender, EventArgs e) // "Экспорт"
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Сохранить список книг";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllLines(saveFileDialog.FileName,
                        bookManager.PrintAllBooks().Select(book => $"{book.Title};{book.Author};{book.Year}"));
                    MessageBox.Show("Список книг успешно экспортирован!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void impBook_Click(object sender, EventArgs e) // "Импорт"
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                openFileDialog.Title = "Загрузить список книг";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                        foreach (var line in lines)
                        {
                            var data = line.Split(';');
                            if (data.Length == 4 && Guid.TryParse(data[0], out _) && int.TryParse(data[3], out int year))
                            {
                                // Игнорируем GUID и добавляем книгу
                                bookManager.AddBook(data[1], data[2], year);
                            }
                            else
                            {
                                MessageBox.Show($"Ошибка в строке: {line}. Проверьте формат данных.", "Ошибка импорта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        UpdateDataGridView();
                        MessageBox.Show("Импорт выполнен успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void BTNsearche_Click(object sender, EventArgs e) // "Поиск"
        {
            string searchText = bunifuTextBox4.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                UpdateDataGridView();
                return;
            }

            var filteredBooks = bookManager.PrintAllBooks().Where(b =>
                b.Title.ToLower().Contains(searchText) ||
                b.Author.ToLower().Contains(searchText) ||
                b.Year.ToString().Contains(searchText) ||
                b.Id.ToString().Contains(searchText)).ToList();

            // Обновляем DataGridView с отфильтрованными данными
            bunifuDataGridView1.Rows.Clear();
            foreach (var book in filteredBooks)
            {
                bunifuDataGridView1.Rows.Add(book.Id, book.Title, book.Author, book.Year);
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Закрываем всё приложение при закрытии главной формы
            Application.Exit();
        }
    }
}
