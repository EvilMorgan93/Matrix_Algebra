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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Determinate_Button_Click(object sender, RoutedEventArgs e) // Нажатие кнопки "Определитель"
        {
            int size = GetSize();
            if (size > 0)
            {
                GetArray(out int[,] arr, size, text_matrix_1);
                text_result.Text = "Определитель: \n" + Determinate(arr);
            }
        }

        private void Scalar_Button_Click(object sender, RoutedEventArgs e) //Кнопка "Умножение на число"
        {
            // проверка на ввод в scalar_text
            if (scalar_text.Text == "")
                MessageBox.Show("Необходимо ввести число!");
            else
            {
                int size = GetSize();
                if (size > 0)
                {
                    GetArray(out int[,] array, size, text_matrix_1);
                    int score = int.Parse(scalar_text.Text);
                    text_result.Clear();
                    text_result.Text += $"Умножение матрицы на {score}\n";
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
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
            int size = GetSize();
            if (size > 0)
            {
                GetArray(out int[,] array, size, text_matrix_1);
                Transpose(size, array);
                text_result.Clear();
                text_result.Text += "Транспонированная матрица: \n";
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        text_result.Text += array[i, j].ToString() + " ";
                    }
                    text_result.Text += "\r\n";
                }
            }
        }

        private void Transpose(int size, int[,] array) // алгоритм транспонирования
        {
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    int temp = array[i, j];
                    array[i, j] = array[j, i];
                    array[j, i] = temp;
                }
            }
        }

        private void Sum_Button_Click(object sender, RoutedEventArgs e) // Кнопка "Сложение матриц"
        {
            int size = GetSize();
            if (size > 0)
            {
                if (text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                StringSplitOptions.RemoveEmptyEntries).Length != text_matrix_2.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                StringSplitOptions.RemoveEmptyEntries).Length) // проверка на совпадение размерности матриц
                    MessageBox.Show("Введенные матрицы должны быть одинакового размера!");
                else
                {
                    GetArray(out int[,] array1, size, text_matrix_1);
                    GetArray(out int[,] array2, size, text_matrix_2);
                    text_result.Clear();
                    text_result.Text += "Сумма двух матриц: \n";
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
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
            int size = GetSize();
            if (size > 0)
            {
                if (text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                StringSplitOptions.RemoveEmptyEntries).Length != text_matrix_2.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                StringSplitOptions.RemoveEmptyEntries).Length) 
                    MessageBox.Show("Умножение матриц недопустимо!");
                else
                {
                    GetArray(out int[,] array1, size, text_matrix_1);
                    GetArray(out int[,] array2, size, text_matrix_2);
                    text_result.Clear();
                    text_result.Text += "Произведение двух матриц: \n";
                    int[,] temp = new int[size, size]; // матрица для умноженных элементов
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            temp[i, j] = 0;
                            for (int k = 0; k < size; k++)
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
            int size = GetSize();
            if (size > 0)
            {
                GetArray(out int[,] array, size, text_matrix_1);
                if (Determinate(array) == 0) // проверка на существование обратной матрицы
                    MessageBox.Show("Обратной матрицы не существует");
                else
                {
                    int[,] matrix = new int[size, size]; // матрица алгебраических дополнений
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                        {
                            int k = i; int k1 = j;
                            int[,] temp = new int[size - 1, size - 1];
                            int s = 0;
                            for (int m = 0; m < size; ++m)
                                if (m != k)
                                {
                                    int s1 = 0;
                                    for (int n = 0; n < size; ++n)
                                        if (n != k1)
                                        {
                                            temp[s, s1] = array[m, n];
                                            s1++;
                                        }
                                    s++;
                                }
                            matrix[i, j] = (int)Math.Pow(-1, k + 1 + k1 + 1) * Determinate(temp); // проверка знака и запись в матрицу алгебраических дополнений
                        }

                    Transpose(size, matrix); // транспонируем матрицу алгебраических дополнений
                    text_result.Clear();
                    text_result.Text += "Обратная матрица: \n";
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            text_result.Text += matrix[i, j] + $"/{Determinate(array)} ";
                        }
                        text_result.Text += "\r\n";
                    }
                }
            }
        }

        // Функция считывания массива из TextBox
        private void GetArray(out int[,] array, int size, TextBox textbox)
        {
            // Заносим числа из TextBox в одномерный массив
            int[] s = textbox.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            int[,] ar = new int[size, size]; // считываем из одномерного массива в двумерный 
            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ar[i, j] = s[k++];
                }
            }
            array = ar;
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
       
        private int GetSize() // Функция получения размера матрицы
        {
            int size = 0;
            if (matrix_size.SelectedIndex < 0) // проверка на выбор размерности
                MessageBox.Show("Выберите размерность матрицы");
            else
            {
                // ПРОВЕРКА НА СОВПАДЕНИЕ ЗАПОЛНЕННОЙ МАТРИЦЫ С ВЫБОРОМ РАЗМЕРНОСТИ
                if (matrix_size.Text == "3X3" && text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                    StringSplitOptions.RemoveEmptyEntries).Length == 9)
                    size = 3;
                else if (matrix_size.Text == "2X2" && text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                    StringSplitOptions.RemoveEmptyEntries).Length == 4)
                    size = 2;
                else if (matrix_size.Text == "4X4" && text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                    StringSplitOptions.RemoveEmptyEntries).Length == 16)
                    size = 4;
                else if (matrix_size.Text == "5X5" && text_matrix_1.Text.Split(new char[] { ' ', '\r', '\n', '\t', '\0' },
                    StringSplitOptions.RemoveEmptyEntries).Length == 25)
                    size = 5;
                else
                    MessageBox.Show("Заполненная матрица не совпадает с выбранной размерностью");
            }
            return size;
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