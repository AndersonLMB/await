using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Management;
using System.Management.Instrumentation;
using OpenTK.Graphics.OpenGL4;


namespace GL1
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBox(0, "Hello Word", "lmb", 3);
            //CudaDeviceVariable<float> devVar = new CudaDeviceVariable<float>(64);
            

            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * From Win32_Processor");
            var mosg = mos.Get();

            Car car = new Car();
            car.Name = "Volvo";
            car.OnNameChanged += Car_OnNameChanged;
            //car.Name = Console.ReadLine();
            //car.Name = "Mercedez";
            while (true)
            {
                string name = Console.ReadLine();
                if (name == "exit")
                {
                    break;
                }
                else
                {
                    car.Name = name;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        private static void Car_OnNameChanged(string before, string after)
        {
            Console.WriteLine("{0} -> {1}", before, after);
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="m"></param>
        /// <param name="c"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern int MessageBox(int h, string m, string c, int type);

    }

    /// <summary>
    /// 车
    /// </summary>
    public class Car
    {
        /// <summary>
        /// 名字
        /// </summary>
        private string name;

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (OnNameChanged != null)
                {
                    OnNameChanged(name, value);
                }
                name = value;
            }
        }
        /// <summary>
        /// 名字变化事件
        /// </summary>
        public event NameChange OnNameChanged;
    }
    /// <summary>
    /// 车名字变化委托
    /// </summary>
    public delegate void NameChange(string before, string after);


}





