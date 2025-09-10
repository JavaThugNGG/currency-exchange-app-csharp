# Проект «Обмен валют»

REST API для описания валют и обменных курсов. Позволяет просматривать и редактировать списки валют и обменных курсов,
и совершать расчёт конвертации произвольных сумм из одной валюты в другую.  
Веб-интерфейс проекта находится по адресу: http://5.35.80.15:5000

---

Приложение написано на языке C# в ООП-стиле с использованием:
- ASP.NET Core MVC
- REST API
- SQLite
- Deploy in VPS (Ubuntu 20.04)



---

## База данных:
### Таблица `Currencies`

| Колонка  | Тип     | Комментарий          |
|----------|---------|----------------------|
| ID       | int     | Первичный ключ, айди |
| Code     | Varchar | Код валюты           |
| FullName | Varchar | Полное имя валюты    |
| Sign     | Varchar | Символ валюты        |

Пример записи в таблице для австралийского доллара:

| ID  | Code | FullName          | Sign |
|-----|------|-------------------|------|
| 1   | AUD  | Australian dollar | A$   |

### Таблица `ExchangeRates`

| Колонка          | Тип        | Комментарий                                                 |
|------------------|------------|-------------------------------------------------------------|
| ID               | int        | Айди курса обмена, первичный ключ                           |
| BaseCurrencyId   | int        | ID базовой валюты, `внешний ключ на Currencies.ID`          |
| TargetCurrencyId | int        | ID целевой валюты, `внешний ключ на Currencies.ID`          |
| Rate             | Decimal(6) | Курс обмена единицы базовой валюты к единице целевой валюты |

Пример записи в таблице для пары доллар-рубль:

| ID  | BaseCurrencyId | TargetCurrencyId | Rate |
|-----|----------------|------------------|------|
| 1   | 2              | 6                | 77   |

## Реализованные запросы:

* GET-запросы
  * <span style="color:white">/currencies</span> – получение списка валют
  * <span style="color:white">/currency/</span>EUR – получение конкретной валюты
  * <span style="color:white">/exchangeRates</span> – получение списка всех обменных курсов
  * <span style="color:white">/exchangeRate/</span>USDRUB – Получение конкретного обменного курса
  * <span style="color:white">/exchange</span>?from=USD&to=AUD&amount=10 – перевод определённого количества средств из одной валюты в другую
  

* POST-запросы
  * добавление новой валюты в базу
    * <span style="color:white">/currencies</span>?name=US Dollar&code=USD&sign=$
  * добавление нового обменного курса в базу
    * <span style="color:white">/exchangeRates</span>?baseCurrencyCode=USD&targetCurrencyCode=RUB&rate=77

  
* PATCH-запрос
  * <span style="color:white">/exchangeRate/</span>USDRUB?rate=70 – обновление существующего в базе обменного курса
___

## Скриншоты приложения

<img width="1063" height="958" alt="image" src="https://github.com/user-attachments/assets/eb570f97-e79a-4323-a0e2-d85ec1773ba2" />
