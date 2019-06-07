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

        #region methods
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

        public void task4() {
            Task<int[]> parent = Task.Run(() => {
                var results = new int[3];
                new Task(() => {
                    results[0] = 0;
                }, TaskCreationOptions.AttachedToParent).Start();

                new Task(() => {
                    results[1] = 1;
                }, TaskCreationOptions.AttachedToParent).Start();

                new Task(() => {
                    results[2] = 2;
                }, TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            //parent.ContinueWith((i) => {
            //    foreach (int item in i.Result) {
            //        Console.WriteLine(item);
            //    }
            //});

            parent.ContinueWith(i => Console.WriteLine(i.Result.Length));
        }

        public void task5() {
            Task t = Task.Run(() => {
                Console.WriteLine("parent task begin");
                new Task(() => {
                    for (int i = 0; i < 10; i++) {
                        Thread.Sleep(10);
                        Console.WriteLine(i);
                    }
                }, TaskCreationOptions.AttachedToParent).Start();

                new Task(() => {
                    for (int i = 0; i < 10; i++) {
                        Thread.Sleep(100);
                        Console.WriteLine(i);
                    }
                }, TaskCreationOptions.AttachedToParent).Start();

                Console.WriteLine("parent task finished.");
            });

            var s = t.ContinueWith((i)=> {
                Console.WriteLine("parent continue task finished.");
            });

            s.Wait();
        }

        public void task6() {
            Task<int[]> parent = Task.Run(() => {
                int[] results = new int[3];

                TaskFactory factory = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);
                factory.StartNew(() => { results[0] = 100; });
                factory.StartNew(() => { results[1] = 200; });
                factory.StartNew(() => { results[2] = 300; });

                return results;
            });

            foreach (var item in parent.Result) {
                Console.WriteLine(item);
            }
        }

        public void task7() {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("i am 0");
            });

            tasks[1] = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("i am 1");
            });

            tasks[2] = Task.Run(() => {
                Thread.Sleep(3000);
                Console.WriteLine("i am 2");
            });

            //Task.WaitAll(tasks);
            Task.WhenAll(tasks);
        }

        /// <summary>
        /// 并行执行 10 个线程
        /// </summary>
        public void task8() {
            Parallel.For(0, 10, i => Console.WriteLine(Thread.CurrentThread.Name +i));
        }

        public void task9() {
            var numbers = Enumerable.Range(0, 8);
            Parallel.ForEach(numbers, i => Console.WriteLine(i));
        }
        #endregion

        private void test() {
            //task1();
            //thread1();
            //task2();
            //task3();
            //task4();
            //task5();
            //task6();
            //task7();
            //task8();
            task9();
        }
    }
}
