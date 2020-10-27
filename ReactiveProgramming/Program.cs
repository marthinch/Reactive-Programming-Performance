using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactiveProgramming
{
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        static int generateNumber, divider;

        static void Main(string[] args)
        {
            Console.WriteLine("Input number to generate data : ");
            generateNumber = int.Parse(Console.ReadLine());

            Console.WriteLine("Input number as divider : ");
            divider = int.Parse(Console.ReadLine());

            ReactiveData();

            NonReactiveData();

            //MultiTaskData().ConfigureAwait(false);

            //ParallelData();
        }

        static List<int> Data(int generateNumber)
        {
            List<int> data = new List<int>();

            for (int i = 0; i <= generateNumber; i++)
            {
                data.Add(i);
            }

            return data;
        }

        static void ReactiveData()
        {
            stopwatch.Reset();
            stopwatch.Start();

            Console.WriteLine("Reactive Data Start {0}", DateTime.Now);

            Data(generateNumber).Where(item => item % divider == 0).ToObservable().SubscribeOn(Scheduler.NewThread).Subscribe(item =>
            {
                var newItem = item * 2;
                //Console.WriteLine(newItem);
            });

            stopwatch.Stop();
            Console.WriteLine("Reactive Data End {0} ms \n", stopwatch.Elapsed.TotalMilliseconds);
        }

        static void NonReactiveData()
        {
            stopwatch.Reset();
            stopwatch.Start();

            Console.WriteLine("Non-Reactive Data Start {0}", DateTime.Now);

            foreach (int item in Data(generateNumber).Where(x => x % divider == 0))
            {
                var newItem = item * 2;
                //Console.WriteLine(newItem);
            }

            stopwatch.Stop();
            Console.WriteLine("Non-Reactive Data End {0} ms \n", stopwatch.Elapsed.TotalMilliseconds);
        }

        static async Task MultiTaskData()
        {
            stopwatch.Reset();
            stopwatch.Start();

            Console.WriteLine("Multi Task Data Start {0}", DateTime.Now);

            List<Task> tasks = new List<Task>();
            foreach (int item in Data(generateNumber).Where(item => item % divider == 0))
            {
                Task task = Task.Run(() =>
                {
                    var newItem = item * 2;
                    //Console.WriteLine(newItem);
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine("Multi Task Data End {0} ms \n", stopwatch.Elapsed.TotalMilliseconds);
        }

        static void ParallelData()
        {
            stopwatch.Reset();
            stopwatch.Start();

            Console.WriteLine("Parallel Data Start {0}", DateTime.Now);

            Parallel.ForEach(Data(generateNumber).Where(item => item % divider == 0), item =>
            {
                var newItem = item * 2;
                //Console.WriteLine(newItem);
            });

            stopwatch.Stop();
            Console.WriteLine("Parallel Data End {0} ms \n", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
