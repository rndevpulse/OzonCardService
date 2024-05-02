namespace OzonCard.Common.Application.Reports.Data;

public static class TimeOfDay 
{
    private static readonly Dictionary<Eating, (TimeSpan Begin, TimeSpan End)> Time = new ()
    {
        {Eating.Breakfast, (new TimeSpan(3, 0, 0), new TimeSpan(11, 0, 0))},
        {Eating.Lunch, (new TimeSpan(11, 0, 0), new TimeSpan(16, 0, 0))},
        {Eating.Dinner, (new TimeSpan(16, 0, 0), new TimeSpan(21, 0, 0))},
    };
    private enum Eating
    {
        Breakfast,
        Lunch,
        Dinner
    }
    public static string GetNameEating(DateTime date)
    {
        var time = date.TimeOfDay;
        if (time > Time[Eating.Breakfast].Begin && time <= Time[Eating.Breakfast].End)
            return "Завтрак";
        if (time > Time[Eating.Lunch].Begin && time <= Time[Eating.Lunch].End)
            return "Обед";
        if (time > Time[Eating.Dinner].Begin && time <= Time[Eating.Dinner].End)
            return "Ужин";
        return "Ночной ужин";
    }
}