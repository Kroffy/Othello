﻿using System;
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

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool turnToWhite = true;
        Rectangle rectHover = new Rectangle();
        Board board = new Board();
        public MainWindow()
        {
            InitializeComponent();
            ImageBrush bgImage = new ImageBrush();
            bgImage.ImageSource = new BitmapImage(new Uri(@"imgs\BackgroundOthello.jpg", UriKind.Relative));
            this.Background = bgImage;
        }

        private void BoardHover(object sender, MouseEventArgs e)
        {
            rectHover.Name = "hoverRect";
            
            double h = canBoard.ActualHeight;
            double dH = h / 8.0;
            double w = canBoard.ActualWidth;
            double dW = w / 8.0;

            double eX = e.GetPosition(canBoard).X;
            double eY = e.GetPosition(canBoard).Y;

            rectHover.Width = dW;
            rectHover.Height = dH;

            int squareIdI = (int)(eX / dW);
            int squareIdJ = (int)(eY / dH);

            ImageBrush playerBrush = new ImageBrush();
            if (turnToWhite)
            {
                playerBrush.ImageSource = new BitmapImage(new Uri(@"imgs\m_blueberry.png", UriKind.Relative));
            }
            else
            {
                playerBrush.ImageSource = new BitmapImage(new Uri(@"imgs\m_mango.png", UriKind.Relative));
            }
            rectHover.Fill = playerBrush;
            
            if (board.IsPlayable(squareIdI, squareIdJ, turnToWhite)){

                Canvas.SetTop(rectHover, (squareIdJ * dH));
                Canvas.SetLeft(rectHover, (squareIdI * dW));
                playerBrush.Opacity = 0.55;
            } else
            {

                Canvas.SetTop(rectHover, eY - dH / 2);
                Canvas.SetLeft(rectHover, eX - dW / 2);
                playerBrush.Opacity = 0.2;
            }
            
            rectHover.InvalidateVisual();
            if (canBoard.Children.Contains(rectHover))
            {
                if(eX > w || eX > h || eX < 0 || eY < 0)
                {
                    canBoard.Children.Remove(rectHover);
                }
            } else
            {
                canBoard.Children.Add(rectHover);
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            printBoard();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int boardDimensions = (int)Width - 50;
            if (Height < Width)
            {
                boardDimensions = (int)Height - 100;
            }
            canBoard.Height = boardDimensions;
            canBoard.Width = canBoard.Height;
            printBoard();
        }
        private void printBoard()
        {

            canBoard.Children.Clear();

            int h = (int)canBoard.ActualHeight;
            int w = (int)canBoard.ActualWidth;
            
            Rectangle bg = new Rectangle();

            bg.Height = h;
            bg.Width = w;

            Canvas.SetTop(bg, 0);
            Canvas.SetLeft(bg, 0);
            bg.Fill = Brushes.White;

            canBoard.Children.Add(bg);

            Brush myBrush = new SolidColorBrush(Color.FromArgb(100,200,10,10));
            

            for (int i = 0; i < 4; i++)
            {
                Line myLine = new Line();
                myLine.Stroke = myBrush;
                myLine.X1 = 0;
                myLine.X2 = w;
                myLine.Y1 = (h / 4.0 * i) + (w/16.0);
                myLine.Y2 = (h / 4.0 * i) + (h/16.0);
                myLine.StrokeThickness = (int)(h/8.0);
                canBoard.Children.Add(myLine);
                myLine = new Line();
                myLine.Stroke = myBrush;
                myLine.Y1 = 0;
                myLine.Y2 = h;
                myLine.X1 = (w / 4.0 * i) + (w/16.0);
                myLine.X2 = (h / 4.0 * i) + (h/16.0);
                myLine.StrokeThickness = (int)(w/8.0);
                canBoard.Children.Add(myLine);
            }
            Rectangle textileFilter = new Rectangle();

            textileFilter.Height = h;
            textileFilter.Width = w;

            Canvas.SetTop(textileFilter, 0);
            Canvas.SetLeft(textileFilter, 0);
            ImageBrush textileBrush = new ImageBrush();
            textileBrush.ImageSource = new BitmapImage(new Uri(@"imgs\texttexture.png", UriKind.Relative));
            textileFilter.Fill = textileBrush;

            canBoard.Children.Add(textileFilter);
            ImageBrush whitePlayerBrush = new ImageBrush();
            whitePlayerBrush.ImageSource = new BitmapImage(new Uri(@"imgs\m_blueberry.png", UriKind.Relative));
            ImageBrush blackPlayerBrush = new ImageBrush();
            blackPlayerBrush.ImageSource = new BitmapImage(new Uri(@"imgs\m_mango.png", UriKind.Relative));
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(board.GetBoard()[i,j] >= 0)
                    {
                        Rectangle square = new Rectangle();

                        square.Height = h/8.0;
                        square.Width = w/8.0;
                        Canvas.SetTop(square, j * h / 8.0);
                        Canvas.SetLeft(square, i * w / 8.0);
                        if (board.GetBoard()[i, j] == 0)
                        {
                            square.Fill = whitePlayerBrush;
                        } else
                        {
                            square.Fill = blackPlayerBrush;
                        }
                        canBoard.Children.Add(square);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rectHover.Fill = Brushes.Black;
            canBoard.Children.Add(rectHover);
            printBoard();
        }

        private void canBoard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (canBoard.Children.Contains(rectHover))
            {
                canBoard.Children.Remove(rectHover);
            }
        }

        private void canBoard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double dH = canBoard.ActualHeight / 8.0;
            double dW = canBoard.ActualWidth / 8.0;

            double eX = e.GetPosition(canBoard).X;
            double eY = e.GetPosition(canBoard).Y;

            int squareIdI = (int)(eX / dW);
            int squareIdJ = (int)(eY / dH);

            if (board.IsPlayable(squareIdI, squareIdJ, turnToWhite))
            {
                board.PlayMove(squareIdI, squareIdJ, turnToWhite);
                turnToWhite = !turnToWhite;
            }
            printBoard();
        }
    }
}
