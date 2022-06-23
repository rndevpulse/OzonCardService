
namespace OzonCard.BizClient.Models.Data
{
    public enum CounterPeriod
    {
        ///Всё время
        AllTime = 0,
        ///День
        Day = 1,
        ///Неделя
        Week = 2,
        ///Месяц
        Month = 3,
        ///Четверть
        Quarter = 4,
        ///Год
        Year = 5
    }
    public enum CounterMetric
    {
        ///Сумма заказов
        OrdersSum = 1,
        ///Количество заказов
        OrdersCount = 2
    }

    
}
