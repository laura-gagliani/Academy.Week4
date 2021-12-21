// See https://aka.ms/new-console-template for more information
using Academy.Week4.ADO.ConsoleApp;

Console.WriteLine("Demo ADO.Net");

//AdoNetDemo.ConnectionDemo();

//AdoNetDemo.InsertDemo();


AdoNetDemo.InsertWithParametersDemo("Ciliegia", "Ciliegia al maraschino", "unità");
AdoNetDemo.DataReaderDemo();

AdoNetDemo.DeleteWithParametersDemo(34);