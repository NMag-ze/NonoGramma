using System;
using System.IO;
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

namespace NonoGramm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string lev)
        {
            InitializeComponent();
            crossName = lev;

            switch (lev)
            {
                case "Heart": w = 5; h = 5; break;
                case "Cat": w = 23; h = 19; break;
                case "Lenin": w = 35; h = 35; break;
                default: break;
            }
            main.Title = crossName;
            //создание полотна для рисования
            createGrid(w, h, ng);
            
            resultat();//создание полотна для проверки кроссворда
            
            createPalette(Ver,wide(),h); //Построение ячеек слева
            createPalette(Hor,w,high()); //Построение ячеек сверху
            createCipher(Ver); //Заполнение ячеек цифрами
            createCipher(Hor); //Заполнение ячеек цифрами
            //создание квадрата текущего цвета
            create_curColor(curColor);//создание клетки текущего цвета

        }

        string crossName="Японские кроссворды"; //название уровня
        int w = 0, h = 0; // длина и ширина
        
        WriteableBitmap wb1, wb2, wb3, wb4;
        System.Drawing.Bitmap bmp1, bmp2, bmp3, bmp4; //Bimmap-изображения

       

      

        private void createGrid(int wid, int hei, Canvas canv) //Создание пустой сетки
        {
            
            canv.Height = 20 * hei;
            canv.Width = 20 * wid;
            Rectangle[,] mas = new Rectangle[hei,wid];
            for (int k = 0; k < mas.GetLength(0); k++)
            {
                for (int m = 0; m < mas.GetLength(1); m++)
                {
                    mas[k,m] = new Rectangle();
                    mas[k,m].Width = 20;
                    mas[k,m].Height = 20;
                    mas[k, m].Fill = Brushes.White;
                    mas[k,m].Stroke = Brushes.Gray;
                    mas[k,m].StrokeThickness = 1;
                }
            }

            for (int i = 0; i < hei; i++)
            {
                for (int j = 0; j < wid; j++)
                {
                    Canvas.SetTop(mas[i,j], 20 * i);
                    Canvas.SetLeft(mas[i,j], 20 * j);
                    canv.Children.Add(mas[i,j]);
                    
                }
            }
        }
        private void createPalette(Canvas pal, int wid, int hei) //Создание цветных сеток слева и сверху
        {
            Rectangle[,] colors = new Rectangle[hei, wid];
            Color[,] cvet = hintcolors(pal, wid, hei);

            if (pal == Ver)
            {
                for (int k = 0; k < colors.GetLength(0); k++)
                {
                    for (int m = 0; m < colors.GetLength(1); m++)
                    {
                        colors[k, m] = new Rectangle();
                        colors[k, m].Width = 20;
                        colors[k, m].Height = 20;
                        colors[k, m].Fill = new SolidColorBrush(cvet[k, m]);
                        colors[k, m].Stroke = Brushes.Gray;
                        colors[k, m].StrokeThickness = 1;
                    }
                }

                pal.Height = 20 * hei;
                pal.Width = 20 * wid;
                for (int i = 0; i < hei; i++)
                {
                    for (int j = 0; j < wid; j++)
                    {
                        Canvas.SetTop(colors[i, j], 20 * i);
                        Canvas.SetLeft(colors[i, j], 20 * j);
                        pal.Children.Add(colors[i, j]);
                    }
                }
            }
            if (pal == Hor)
            {
                for (int m = 0; m < colors.GetLength(1); m++)
                {
                    for (int k = colors.GetLength(0)-1; k >=0; k--)
                    {
                        colors[k, m] = new Rectangle();
                        colors[k, m].Width = 20;
                        colors[k, m].Height = 20;
                        colors[k, m].Fill = new SolidColorBrush(cvet[k, m]);
                        colors[k, m].Stroke = Brushes.Gray;
                        colors[k, m].StrokeThickness = 1;
                    }
                }

                pal.Height = 20 * hei;
                pal.Width = 20 * wid;
                for (int j = 0; j < wid; j++)
                {
                    for (int i = 0; i <hei; i++)
                    {
                        Canvas.SetTop(colors[i, j], 20 * i);
                        Canvas.SetLeft(colors[i, j], 20 * j);
                        pal.Children.Add(colors[i, j]);
                    }
                }
            }
        }

        private void createCipher(Canvas pal) //Заполнение левой и верхней сетки цифрами
        {
            if (pal == Ver)
            {
                int[][] cip = cipher(pal); int k = 0;
                TextBlock[][] nums = new TextBlock[h][];
                int wd=0, hg=0;
                for (int i = 0; i < nums.Length; i++)
                {
                    k = cip[i].Length;
                    nums[i] = new TextBlock[k];
                    for (int j = 0; j < nums[i].Length; j++)
                    {
                        
                            nums[i][j] = new TextBlock();
                            nums[i][j].Height = 19;
                            nums[i][j].Width = 19;
                            wd = bmp3.Width - 10 - 20 * j;
                            hg = 10 + 20 * i;
                            nums[i][j].Background = (cip[i][j] != 0) ? new SolidColorBrush(convert(bmp3.GetPixel(wd, hg))):Brushes.White;
                            nums[i][j].Foreground = ((convert(bmp3.GetPixel(wd,hg)).R <= 128) && (convert(bmp3.GetPixel(wd,hg)).G <= 128) && (convert(bmp3.GetPixel(wd,hg)).B <= 128)) ?
                                Brushes.White : Brushes.Black;
                            if (cip[i][j] != 0) nums[i][j].Text += cip[i][j];
                            nums[i][j].TextAlignment = TextAlignment.Center;
                            Canvas.SetRight(nums[i][j], 20 * j+1);
                            Canvas.SetTop(nums[i][j], 20 * i+1);
                            pal.Children.Add(nums[i][j]);
                        
                    }
                }
            }
            if (pal == Hor)
            {
                int[][] cip = cipher(pal); int k = 0;
                TextBlock[][] nums1 = new TextBlock[w][];
                int wd = 0, hg = 0;
                for (int i = 0; i < nums1.Length; i++)
                {
                    k = cip[i].Length;
                    nums1[i] = new TextBlock[k];
                    for (int j = 0; j < nums1[i].Length; j++)
                    {
                        if (cip[i][j] != 0)
                        {
                            nums1[i][j] = new TextBlock();
                            nums1[i][j].Height = 19;
                            nums1[i][j].Width = 19;
                            hg = bmp4.Height - 10 - 20 * j;
                            wd = 10 + 20 * i;
                            nums1[i][j].Background = (cip[i][j] != 0) ? new SolidColorBrush(convert(bmp4.GetPixel(wd, hg))):Brushes.White;
                            nums1[i][j].Foreground = ((convert(bmp4.GetPixel(wd,hg)).R <= 128) && (convert(bmp4.GetPixel(wd,hg)).G <= 128) && (convert(bmp4.GetPixel(wd,hg)).B <= 128)) ?
                                Brushes.White : Brushes.Black;
                            if (cip[i][j] != 0) nums1[i][j].Text += cip[i][j];
                            nums1[i][j].TextAlignment = TextAlignment.Center;
                            
                            Canvas.SetLeft(nums1[i][j], 20 * i+1);
                            Canvas.SetBottom(nums1[i][j], 20 * j+1);
                            pal.Children.Add(nums1[i][j]);
                        }
                    }
                }
            }
        }

       

       
       
        private void Ng_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //Заливка клеток
        {
            Point p = e.GetPosition(ng);
            Rectangle r = (Rectangle)ng.InputHitTest(p);
            r.Fill = curColor.Fill;
            
        }

        private void Ng_MouseRightButtonDown(object sender, MouseButtonEventArgs e) //Стирание клеток
        {
            Point p = e.GetPosition(ng);
            Rectangle r = (Rectangle)ng.InputHitTest(p);
            r.Fill = Brushes.White;
            
        }


        private void Ng_MouseMove(object sender, MouseEventArgs e) //Заливка при зажатой кнопке мыши
        {
            Point p;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                p = e.GetPosition(ng);
                Rectangle r = (Rectangle)ng.InputHitTest(p);
                r.Fill = curColor.Fill;
            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                p = e.GetPosition(ng);

                Rectangle r = (Rectangle)ng.InputHitTest(p);
                r.Fill = Brushes.White;
            }
        }

        
        public static void ToImageSource(Canvas canvas, string filename) //Функция конвертирования Canvas в png-файл
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(
            (int)canvas.Width, (int)canvas.Height, 96d, 96d, PixelFormats.Pbgra32);
            canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
            canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));
            bmp.Render(canvas);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }


        





        public WriteableBitmap SaveAsWriteableBitmap(Canvas surface) //конвертация canvas в writeable bitmap
        {
            if (surface == null) return null;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
              (int)size.Width,
              (int)size.Height,
              96d,
              96d,
              PixelFormats.Pbgra32);
            renderBitmap.Render(surface);


            //Restore previously saved layout
            surface.LayoutTransform = transform;

            //create and return a new WriteableBitmap using the RenderTargetBitmap
            return new WriteableBitmap(renderBitmap);

        }

        private System.Drawing.Bitmap BitmapFromWriteableBitmap(WriteableBitmap writeBmp) //конвертация из writeable bitmap в bitmap
        {
            System.Drawing.Bitmap bmp;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)writeBmp));
                enc.Save(outStream);
                bmp = new System.Drawing.Bitmap(outStream);
            }
            return bmp;
        }

        private bool proverka(System.Drawing.Bitmap b1, System.Drawing.Bitmap b2) //проверка на правильность решения
        {
            bool flag = true;
            for (int i=0;i<b1.Width;i++)
            {
                for(int j =0;j<b1.Height;j++)
                {
                    if (b1.GetPixel(i, j) != b2.GetPixel(i, j)) flag = false;
                }
            }
            return flag;
        }
        
        
       

        private void resultat() //создание сравнительного canvas
        {
            
            string path = "";
            switch (crossName)
            {
                case "Heart": path = @"images\Heart.png"; break;
                case "Lenin": path = @"images\Lenin.png"; break;
                case "Cat": path = @"images\Cat.png"; break;
                default: break;
            }
            BitmapImage theImage = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            Result.Height = 20 * h;
            Result.Width = 20 * w;
            Result.Background = new ImageBrush(theImage);
        }

        private Color convert(System.Drawing.Color c) //конвертация из System.Drawing.Color в System.Media.Color
        {
            Color newColor = Color.FromArgb(c.A, c.R, c.G, c.B);
            return newColor;
        }

        private void create_curColor(Rectangle r) //создание клетки текущего цвета с динамическим размером
        {
            r.Height = Hor.Height;
            r.Width = Ver.Width;
            r.Fill = new SolidColorBrush(Colors.White);
        }
  

       
        private void convertToBitmapResult() //конвертация в bitmap canvas'а сравнения
        {
            wb2 = SaveAsWriteableBitmap(Result);
            bmp2 = BitmapFromWriteableBitmap(wb2);
        }
        private void convertToBitmapNg() //конвертация в bitmap игровой сетки
        {
            wb1 = SaveAsWriteableBitmap(ng);
            bmp1 = BitmapFromWriteableBitmap(wb1);
        }
        private void convertToBitmapVer() //конвертация в bitmap левой сетки
        {
            wb3 = SaveAsWriteableBitmap(Ver);
            bmp3 = BitmapFromWriteableBitmap(wb3);
        }
        private void convertToBitmapHor() //конвертация в bitmap верхней сетки
        {
            wb4 = SaveAsWriteableBitmap(Hor);
            bmp4 = BitmapFromWriteableBitmap(wb4);
        }
        

        private void ScrollChanged(object sender, ScrollChangedEventArgs e) //синхронная прокрутка левой и верхней сеток вместе с основной
        {
            if (sender == sw)
            {
                VertHint.ScrollToVerticalOffset(e.VerticalOffset);
                HorHint.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        private void Ng_MouseUp(object sender, MouseButtonEventArgs e) //проверка правильного решения при отпускании кнопки мыши
        {

            convertToBitmapNg();
            if (proverka(bmp1, bmp2))
            {
                MessageBoxResult d = MessageBox.Show("Поздравляем с победой! Не желаете ли сохранить эту картинку на память?", "Победа", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(d==MessageBoxResult.Yes)
                {
                    Microsoft.Win32.SaveFileDialog saveimg = new Microsoft.Win32.SaveFileDialog();
                    saveimg.InitialDirectory = @"C:\Users\N\Desktop";
                    saveimg.DefaultExt = ".png";
                    saveimg.Filter = "Image (.png)|*.png";
                    if (saveimg.ShowDialog() == true)
                    {
                        ToImageSource(ng, saveimg.FileName);
                    }
                }
                LevelSelect newGame = new LevelSelect();
                newGame.Show();
                this.Close();
            }
        }

        private void Ver_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //получение цвета нажатой ячейки левой сетки
        {
            Point c = e.GetPosition(Ver);
            try
            {
                TextBlock r = (TextBlock)Ver.InputHitTest(c);
                curColor.Fill = r.Background;
            }
            catch
            {
                Rectangle r = (Rectangle)Ver.InputHitTest(c);
                curColor.Fill = r.Fill;
            }
        }

        private void Ver_MouseRightButtonDown(object sender, MouseButtonEventArgs e) //установка метки (кружка) на нужную ячейку левой сетки и ее снятие
        {
            Point p = e.GetPosition(Ver);
            try
            {
                TextBlock r = (TextBlock)Ver.InputHitTest(p);
                
                Ellipse el = new Ellipse();
                el.Height = 19;
                el.Width = 19;
                el.Fill = Brushes.Transparent;
                el.Stroke = Brushes.White;
                el.StrokeThickness = 1;
                Canvas.SetRight(el,Canvas.GetRight(r));
                Canvas.SetTop(el, Canvas.GetTop(r));
                Ver.Children.Add(el);

            }
            catch
            {
                Ellipse el = (Ellipse)Ver.InputHitTest(p);
                Ver.Children.Remove(el);
            }
        }

        private void Hor_MouseRightButtonDown(object sender, MouseButtonEventArgs e) //установка метки (кружка) на нужную ячейку верхней сетки и ее снятие
        {
            Point p = e.GetPosition(Hor);
            try
            {
                TextBlock r = (TextBlock)Hor.InputHitTest(p);

                Ellipse el = new Ellipse();
                el.Height = 19;
                el.Width = 19;
                el.Fill = Brushes.Transparent;
                el.Stroke = Brushes.White;
                el.StrokeThickness = 1;
                Canvas.SetLeft(el, Canvas.GetLeft(r));
                Canvas.SetBottom(el, Canvas.GetBottom(r));
                Hor.Children.Add(el);

            }
            catch
            {
                Ellipse el = (Ellipse)Hor.InputHitTest(p);
                Hor.Children.Remove(el);
            }
        }
        private void Restart_Click(object sender, RoutedEventArgs e) //рестарт уровня
        {
            MessageBoxResult restart = MessageBox.Show("Вы уверены?", "Рестарт", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (restart == MessageBoxResult.Yes)
            {
                createGrid(w, h, ng);
                curColor.Fill = Brushes.White;
                foreach (var el in Ver.Children.OfType<Ellipse>().ToList())
                    Ver.Children.Remove(el);
                foreach (var el in Hor.Children.OfType<Ellipse>().ToList())
                    Hor.Children.Remove(el);
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e) //как играть
        {
            MessageBox.Show("Создайте рисунок по заданным числам.\nЧтобы выбрать цвет, нажмите на нужную ячейку слева или сверху.\nЧтобы закрасить ячейку, нажмите на нее левой кнопкой мыши,\n" +
                "а чтобы стереть ячейку - правой кнопкой мыши.\nЧтобы пометить ячейку с цифрой, нажмите на нее правой кнопкой мыши, а чтобы снять метку - правой кнопкой мыши по помеченной ячейке.", "Помощь", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void About_Click(object sender, RoutedEventArgs e) //о программе
        {
            MessageBox.Show("Создатель: Никита Федчун\nОригинальная идея:\nНон Исида (石田 のん) и Тэцуя Нисио (西尾 徹也)\n\t\t2020 г.", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void NewGame_Click(object sender, RoutedEventArgs e) //начать новую игру
        {
            LevelSelect newGame = new LevelSelect();
            newGame.Show();
            this.Close();

        }

        

        private void Hor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //получение цвета нажатой ячейки верхней сетки
        {
            Point c = e.GetPosition(Hor);
            try
            {
                TextBlock r = (TextBlock)Hor.InputHitTest(c);
                curColor.Fill = r.Background;
            }
            catch
            {
                Rectangle r = (Rectangle)Hor.InputHitTest(c);
                curColor.Fill = r.Fill;
            }
        }

        private int wide() //установление ширины левой сетки
        {
            int max = 0, tek = 0;
            
            Color tekCvet = Colors.White;
            convertToBitmapResult();
            for(int j = 9; j < bmp2.Height; j += 20) 
            {
                for (int i = 9; i < bmp2.Width; i += 20)
                {

                    if ((convert(bmp2.GetPixel(i, j)) != tekCvet) && (convert(bmp2.GetPixel(i, j)) != Colors.White))
                    {
                        tekCvet = convert(bmp2.GetPixel(i, j));
                        tek++;
                    }
                    else
                    {
                        tekCvet = convert(bmp2.GetPixel(i, j));
                    }
                    
                }
                if (tek > max) max = tek;
                tek = 0;
            }
            return max;
        }

        private int high() //установление высоты верхней сетки
        {
            int max = 0, tek = 0;
            Color tekCvet;
            convertToBitmapResult();
            for (int j = 9; j < bmp2.Width; j += 20)
            {
                tekCvet = Colors.White;
                for (int i = 9; i < bmp2.Height; i += 20)
                {

                    if ((convert(bmp2.GetPixel(j, i)) != tekCvet) && (convert(bmp2.GetPixel(j, i)) != Colors.White))
                    {
                        tekCvet = convert(bmp2.GetPixel(j, i));
                        tek++;
                    }
                    else
                    {
                        tekCvet = convert(bmp2.GetPixel(j, i));
                    }
                }
                if (tek > max) max = tek;
                tek = 0;
            }
           
            return max;
        }
       
        private Color [,] hintcolors(Canvas canv,int width, int height) //массив цветов для левой и верхней сеток
        {
            convertToBitmapResult();
            int i, j;
            Color[,] c = new Color[height, width];
            Color tekCvet;
            for (int p = 0; p < c.GetLength(0); p++)
            {
                for (int q = 0; q < c.GetLength(1); q++) c[p, q] = Colors.Transparent;
            }
            if (canv == Ver)
            {

                i = bmp2.Width - 11; j = 9;
                for (int k = 0; k < c.GetLength(0); k++) //массив цветов
                {

                    while (j < bmp2.Height)
                    {
                        tekCvet = Colors.White;
                        for (int m = c.GetLength(1) - 1; m >= 0; m--) //массив цветов
                        {

                            while (i >= 9)
                            {
                                if ((convert(bmp2.GetPixel(i, j)) != tekCvet) && (convert(bmp2.GetPixel(i, j)) != Colors.White))
                                {
                                    tekCvet = convert(bmp2.GetPixel(i, j));
                                    c[k, m] = tekCvet;
                                    i -= 20;
                                    break;
                                }
                                else { tekCvet = convert(bmp2.GetPixel(i, j)); i -= 20; }
                            }
                            if (i < 9) { i = bmp2.Width - 11; break; }
                        }
                        j += 20; break;
                    }
                }
            }
            if (canv == Hor)
            {

                i = bmp2.Height - 11; j = 9;
                
                for (int m = 0; m <c.GetLength(1); m++) //массив цветов
                {
                    while (j < bmp2.Width)
                    {
                        tekCvet = Colors.White;
                        for (int k = c.GetLength(0)-1; k >=0; k--) //массив цветов
                        {

                            while (i>=9)
                            {
                                if ((convert(bmp2.GetPixel(j, i)) != tekCvet) && (convert(bmp2.GetPixel(j, i)) != Colors.White))
                                {
                                    tekCvet = convert(bmp2.GetPixel(j, i));
                                    c[k, m] = tekCvet;
                                    i -= 20;
                                    break;
                                }
                                else { tekCvet = convert(bmp2.GetPixel(j, i)); i -= 20; }
                            }
                            if (i < 11) { i = bmp2.Height - 11; break; }
                        }
                        j += 20; break;
                    }
                }
            }
            return c;
        }


        private int[][] cipher(Canvas p) //"зубчатый" массив чисел для левой и верхней сеток
        {
            int size = 0, c=0, n=-1;
            int[] [] counts = new int[1][];
            if (p == Ver)
            {
                counts = new int[h][];
                convertToBitmapVer();
                for (int j = 9; j < bmp3.Height; j += 20)
                {
                    for (int i = bmp3.Width - 11; i >= 9; i -= 20)
                    {
                        if (convert(bmp3.GetPixel(i, j)) != Colors.White) size++;
                        else break;
                    }
                    counts[c] = new int[size];
                    c++;
                    size = 0;
                }

                int k = bmp2.Width - 11;
                Color tekCvet; c = 0;
                for (int j = 9; j < bmp2.Height; j += 20)
                {
                    tekCvet = Colors.White;
                    while(k>=9)
                    {

                        if ((convert(bmp2.GetPixel(k, j)) != tekCvet) && (convert(bmp2.GetPixel(k, j)) != Colors.White))
                        {

                            tekCvet = convert(bmp2.GetPixel(k, j));
                            n++; counts[c][n]++; k -= 20;

                        }
                        else if ((convert(bmp2.GetPixel(k, j)) == tekCvet) && (convert(bmp2.GetPixel(k, j)) != Colors.White)) { counts[c][n]++; k -= 20; }
                        else { tekCvet = Colors.White; k -= 20; }
                    }
                    k = bmp2.Width - 11; n = -1; c++;
                }
            }
            if (p==Hor)
            {
                counts = new int[w][];
                convertToBitmapHor();
                for (int j = 9; j < bmp4.Width; j += 20)
                {
                    for (int i = bmp4.Height - 11; i >= 9; i -= 20)
                    {
                        if (convert(bmp4.GetPixel(j, i)) != Colors.White) size++;
                        else break;
                    }
                    counts[c] = new int[size]; c++; size = 0;
                }

                int k = bmp2.Height - 11;
                Color tekCvet; c = 0;
                
                for (int j = 9; j < bmp2.Width; j += 20)
                {
                    tekCvet = Colors.White;
                    while (k >= 9)
                    {

                        if ((convert(bmp2.GetPixel(j, k)) != tekCvet) && (convert(bmp2.GetPixel(j, k)) != Colors.White))
                        {

                            tekCvet = convert(bmp2.GetPixel(j, k));
                            n++; counts[c][n]++; k -= 20;

                        }
                        else if ((convert(bmp2.GetPixel(j, k)) == tekCvet) && (convert(bmp2.GetPixel(j, k)) != Colors.White))
                        {
                            counts[c][n]++; k -= 20;
                        }
                        else
                        {
                            tekCvet = Colors.White; k -= 20;
                        }
                    }
                    k = bmp2.Height - 11; n = -1; c++;
                }
            }
            return counts;
        }  
    }
}
