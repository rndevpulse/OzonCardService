

namespace OzonCard.BizClient.Models.Data
{
    public enum TransactionType
    {
        CloseOrder,/// Закрытие заказа
        RefillWallet,/// Пополнение счёта через iiko.biz
        ResetRefillWallet,/// Сгорание пополнения счёта из iiko.biz
        PayFromWallet,/// Оплата заказа со счёта
        CancelPayFromWallet,/// Отмена оплаты заказа со счёта
        RefillWalletFromOrder,/// Пополнение счёта из заказа
        CancelRefillWalletFromOrder,/// Отмена пополнения баланса из заказа
        AutomaticRefillWallet,/// Автоматическое пополнение или списание счёта
        ResetAutomaticRefillWallet,/// Сгорание периодического пополнения счёта
        AbortOrder,/// Отмена заказа
        RemoveGuestCategory,/// Удаление категории гостя
        SetGuestCategory,/// Присвоение категории гостя
        DiscountSum,/// Применение скидки к заказу
        CouponActivation,/// Активация купона
        RefillWalletFromApi,/// Пополнение счёта через API

    }
}
