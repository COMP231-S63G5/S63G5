using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDBObject;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SwimWorkoutDBContextNew newObj = new SwimWorkoutDBContextNew();
           Console.WriteLine(newObj.insertWorkoutPlan("10-10-2014",1000,"1.20"));
        //   Console.WriteLine(newObj.getWorkOutPlanIds());
           Console.ReadLine();
        }
    }
}
