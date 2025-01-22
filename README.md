# MIDI to grandMA2
Приложение для связи MIDI контроллера и светового пульта семейства grandMA2

## Используемые технологии
Приложение написано на языке C#, интерфейс на winfowrms. Для разработки и компиляции приложения достаточно иметь Visual Studio и .net framework.

Дополнительно используются библиотеки:

- NAudio - для работы с MIDI входом
- Newtonsoft.Json - для работы с форматом JSON
- WebSocketSharp - для связи с пультом по websocket

Отдельное спасибо [@rain917](https://github.com/rain917) за содержимое директории "gma": файлы MA2Connector.cs, MA2Playbacks.cs и MATypes.cs


![Made in Russia](https://raw.githubusercontent.com/sysolyatin/midigma/main/website/assets/img/made-in-russia-sign-ru.svg)
