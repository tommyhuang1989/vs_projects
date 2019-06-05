using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace tasks_demo_20190605 {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            test();
        }

        public void task1() {
            Task t = Task.Run(() => {
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine(i);
                }
            });
        }

        public void thread1() {
            Thread t = new Thread(new ThreadStart(() => {
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine(i);
                }
            }));

            t.Start();
        }

        public void task2() {
            Task<string> t = Task.Run(() => {
                return "i love you";
            }).ContinueWith((i) => {
                string content = i.Result + " tommy";
                return content;
            });

            Console.WriteLine(t.Result);
        }

        public void task3() {
            CancellationTokenSource token_source = new CancellationTokenSource();
            CancellationToken token = token_source.Token;

            Task t = Task.Run(() => {
                for (int i = 0; i < 100; i++) {
                    Thread.Sleep(10);
                    if (i == 40) {
                    }

                    if (!token.IsCancellationRequested) {
                        Console.WriteLine(i);
                    }
                    else {
                        Console.WriteLine("task is canceled.");
                    }
                }
            }, token);

            t.ContinueWith((i) => {
                Console.WriteLine("run on continue canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i)=> {
                Console.WriteLine("run on continue faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            t.ContinueWith((i) => {
                Console.WriteLine("run on continue completion");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);


            Thread.Sleep(50);
            //token_source.Cancel();
        }


        private void test() {
            //task1();
            //thread1();
            //task2();
            task3();
        }
    }
}
