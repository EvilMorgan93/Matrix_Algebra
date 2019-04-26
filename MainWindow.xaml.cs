/* Курсовая работа
 * 30.03.2019
 * Моргунов А.В.
 * copyright © 2019 all rights reserved
 */

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System;
using System.Windows.Input;

namespace Kursa
{
    partial class MainWindow : Window
    {
        private int SIZE_ROWS; // количество строк
        private int SIZE_COLUMNS; // количество столбцов

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Determinate_Button_Click(object sender, RoutedEventArgs e) // Нажатие кнопки "Определитель"
        {
            if (GetArray(out int[,] arr, text_matrix_1, size_row_1, size_column_1))
            {              
                if (size_row_1.Text != size_column_1.Text)
                    MessageBox.Show("Нахождение определителя невозможно!"); 
                else
                    text_result.Text = "Определитель матрицы #1: \n" + Determinate(arr);
            }
        }

        private void Scalar_Button_Click(object sender, RoutedEventArgs e) //Кнопка "Умножение на число"
        {
            // проверка на ввод в scalar_text
            if (scalar_text.Text == "")
                MessageBox.Show("Необходимо ввести число!");
            else
            {
                if (GetArray(out int[,] array, text_matrix_1, size_row_1, size_column_1))
                {
                    int score = int.Parse(scalar_text.Text);
                    text_result.Clear();
                    text_result.Text += $"Умножение матрицы #1 на {score}\n";
                    for (int i = 0; i < SIZE_ROWS; i++)
                    {
                        for (int j = 0; j < SIZE_COLUMNS; j++)
                        {
                            array[i, j] *= score;
                            text_result.Text += array[i, j] + " ";
                        }
                        text_result.Text += "\r\n";
                    }
                }
            }
        }

        private void Transpose_Button_Click(object sender, RoutedEventArgs e) // Кнопка "Транспонирование"
        {
            if (GetArray(out int[,] array, text_matrix_1, size_row_1, size_column_1))
            {
                int[,] transpose = Transpose(array);
                text_result.Clear();
                text_result.Text += "Транспонированная матрица #1: \n";
                for (int i = 0; i < SIZE_COLUMNS; i++)
                {
                    for (int j = 0; j < SIZE_ROWS; j++)
                    {
                        text_result.Text += transpose[i, j].ToString() + " ";
                    }
                    text_result.Text += "\r\n";
                }
            }
        }

        private int[,] Transpose(int[,] array) // алгоритм транспонирования
        {
            int[,] transpose = new int[SIZE_COLUMNS, SIZE_ROWS];
            for (int i = 0; i < SIZE_COLUMNS; i++)
            {
                for (int j = 0; j < SIZE_ROWS; j++)
                {
                    transpose[i, j] = array[j, i];
                }
            }
            return transpose;
        }

        private void Sum_Button_Click(object sender, RoutedEventArgs e) // Кнопка "Сложение матриц"
        {
            if (size_row_1.Text != size_row_2.Text || size_column_1.Text != size_column_2.Text) // проверка на совпадение размерности матриц
                MessageBox.Show("Введенные матрицы должны быть одинакового размера!");
            else
            {
                if (GetArray(out int[,] array1, text_matrix_1, size_row_1, size_column_1) && GetArray(out int[,] array2, text_matrix_2, size_row_2, size_column_2))
                {
                    text_result.Clear();
                    text_result.Text += "Сумма двух матриц: \n";
                    for (int i = 0; i < SIZE_ROWS; i++)
                    {
                        for (int j = 0; j < SIZE_COLUMNS; j++)
                        {
                            text_result.Text += (array1[i, j] + array2[i, j]).ToString() + " ";
                        }
                        text_result.Text += "\r\n";
                    }
                }
            }
        }

        private void Mult_Button_Click(object sender, RoutedEventArgs e) // Кнопка "Произведение матриц"
        {
            if (size_column_1.Text != size_row_2.Text)
                MessageBox.Show("Умножение матриц невозможно!");
            else
            {
                if (GetArray(out int[,] array1, text_matrix_1, size_row_1, size_column_1) &&
                GetArray(out int[,] array2, text_matrix_2, size_row_2, size_column_2))
                {
                    text_result.Clear();
                    text_result.Text += "Произведение двух матриц: \n";
                    int[,] temp = new int[int.Parse(size_row_1.Text), int.Parse(size_column_2.Text)]; // матрица размером row1 * column2, для конечного вывода
                    for (int i = 0; i < int.Parse(size_row_1.Text); i++)
                    {
                        for (int j = 0; j < int.Parse(size_column_2.Text); j++)
                        {
                            for (int k = 0; k < int.Parse(size_row_2.Text); k++)
                            {
                                temp[i, j] += array1[i, k] * array2[k, j];
                            }
                            text_result.Text += temp[i, j].ToString() + " ";
                        }
                        text_result.Text += "\r\n";
                    }
                }
            }
        }

        private void Inverse_Button_Click(object sender, RoutedEventArgs e) // Кнопка "Обратная матрица"
        {
            if (GetArray(out int[,] array, text_matrix_1, size_row_1, size_column_1))
            {
                if (size_row_1.Text != size_column_1.Text)
                    MessageBox.Show("Выберите квадратную матрицу");
                else if (Determinate(array) == 0) // проверка на существование обратной матрицы
                    MessageBox.Show("Обратной матрицы не существует");
                else
                {
                    int[,] matrix = new int[SIZE_ROWS, SIZE_COLUMNS]; // матрица алгебраических дополнений
                    for (int i = 0; i < SIZE_ROWS; i++)
                        for (int j = 0; j < SIZE_COLUMNS; j++)
                        {
                            int k = i; int k1 = j;
                            int[,] temp = new int[SIZE_ROWS - 1, SIZE_COLUMNS - 1];
                            int s = 0;
                            for (int m = 0; m < SIZE_COLUMNS; ++m)
                                if (m != k)
                                {
                                    int s1 = 0;
                                    for (int n = 0; n < SIZE_COLUMNS; ++n)
                                        if (n != k1)
                                        {
                                            temp[s, s1] = array[m, n];
                                            s1++;
                                        }
                                    s++;
                                }
                            matrix[i, j] = (int)Math.Pow(-1, k + 1 + k1 + 1) * Determinate(temp); // проверка знака и запись в матрицу алгебраических дополнений
                        }

                    int[,] result = Transpose(matrix); // транспонируем матрицу алгебраических дополнений
                    text_result.Clear();
                    text_result.Text += "Обратная матрица #1: \n";
                    for (int i = 0; i < SIZE_ROWS; i++)
                    {
                        for (int j = 0; j < SIZE_COLUMNS; j++)
                        {
                            text_result.Text += result[i, j] + $"/{Determinate(array)} ";
                        }
                        text_result.Text += "\r\n";
                    }
                }
            }
        }

        // Функция считывания массива из TextBox
        private bool GetArray(out int[,] array, TextBox textbox, ComboBox size_row, ComboBox size_column) // Функция получения матрицы 
        {
            if (GetSize(size_row, size_column, textbox) == true) // Если выбранная размерность и введенная матрица совпала
            {
                // Заносим числа из TextBox в одномерный массив
                int[] s = textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                    StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
                int[,] ar = new int[SIZE_ROWS, SIZE_COLUMNS]; // считываем из одномерного массива в двумерный 
                int k = 0;
                for (int i = 0; i < SIZE_ROWS; i++)
                {
                    for (int j = 0; j < SIZE_COLUMNS; j++)
                    {
                        ar[i, j] = s[k++];
                    }
                }
                array = ar;
                return true;
            }
            else
                array = null;
            return false;
        }

        // Функция получения определителя матрицы
        private int Determinate(int[,] array) // рекурсивная функция нахождения определителя, разложением по первой строке
        {
            if (array.Length == 1)
                return array[0, 0];
            if (array.Length == 4)
                return array[0, 0] * array[1, 1] - array[0, 1] * array[1, 0];
            int sign = 1, det = 0;
            for (int i = 0; i < array.GetLength(1); i++)
            {
                int[,] minor = GetMinor(array, i);
                det += sign * array[0, i] * Determinate(minor);
                sign = -sign; // проверка на знак
            }
            return det;
        }

        private static int[,] GetMinor(int[,] matrix, int lenghtrow) // Получение минора от элемента первой строки
        {
            int[,] result = new int[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0, col = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == lenghtrow)
                        continue;
                    result[i - 1, col] = matrix[i, j];
                    col++;
                }
            }
            return result;
        }

        private bool GetSize(ComboBox size_row, ComboBox size_column, TextBox textbox) // Функция получения размера матрицы
        {
            if (size_row.SelectedIndex < 0 || size_column.SelectedIndex < 0) // проверка на выбор размерности
            {
                MessageBox.Show("Выберите размерность матрицы");
                return false;
            } // ОЧЕНЬ ПЛОХАЯ ПРОВЕРКА ЗАПОЛНЕННОЙ МАТРИЦЫ С ВЫБРАННОЙ РАЗМЕРНОСТЬЮ !!!!!!!!!!!!!!!!!!
            else if (size_row.Text == "2" && size_column.Text == "2" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 4 || size_row.Text == "2" && size_column.Text == "3" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 6 || size_row.Text == "2" && size_column.Text == "4" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 8 || size_row.Text == "3" && size_column.Text == "2" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 6 || size_row.Text == "4" && size_column.Text == "2" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 8 || size_row.Text == "3" && size_column.Text == "3" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 9 || size_row.Text == "4" && size_column.Text == "3" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 12 || size_row.Text == "3" && size_column.Text == "4" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 12 || size_row.Text == "4" && size_column.Text == "4" && textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
            StringSplitOptions.RemoveEmptyEntries).Length != 16)
            {
                MessageBox.Show("Введенная матрица не совпадает с выбранной размерностью!");
                return false;
            }
            int.TryParse(size_row.Text, out SIZE_ROWS);         // считывание количество строк
            int.TryParse(size_column.Text, out SIZE_COLUMNS);   // и столбцов из combobox
            return true;
        }

        // события на отлавливание клавиш
        private void Text1_KeyDown(object sender, KeyEventArgs e)
         {
            // проверка на нажатие только цифр, пробела и enter
            if (!(((int)e.Key >= 34) && ((int)e.Key <= 43)) && !(((int)e.Key >= 74) && ((int)e.Key <= 83)) && (int)e.Key != 143)
                e.Handled = true;
        }

        private void Scalar_text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // отлавливание пробела в scalar_text
                e.Handled = true;
        }
        // события на отлавливание клавиш
    }
}