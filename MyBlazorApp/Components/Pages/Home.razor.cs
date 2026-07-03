using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace MyBlazorApp.Components.Pages;

public partial class Home
{
    
    private static readonly int[] DonutChartData = { 30, 70 , 50};
    private static readonly string[] DonutChartLabels = { "Pendentes", "Em Progresso", "Concluidos" };

    private int _index = -1; //default value cannot be 0 -> first selectedindex is 0.
    private BarChartOptions _axisChartOptions = new BarChartOptions()
    {
        XAxisTitle = "Months",
        YAxisTitle = "Sales",
        FixedBarWidth = 8,
    };
    private List<ChartSeries<double>> _series = new()
    {
        new() { Name = "United States", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new() { Name = "Germany", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new() { Name = "Sweden", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };
    private string[] _xAxisLabels = { "January", "February", "March", "April", "May", "June", "July", "August", "September" };


    public record Employee(string Name, string Position, int YearsEmployed, int Salary, int Rating);
    public IEnumerable<Employee> employees;

    protected override void OnInitialized()
    {
        employees = new List<Employee>
        {
            new Employee("Sam", "CPA", 23, 87_000, 4),
            new Employee("Alicia", "Product Manager", 11, 143_000, 5),
            new Employee("Ira", "Developer", 4, 92_000, 3),
            new Employee("John", "IT Director", 17, 229_000, 4),
        };
    }

}
