# MIDI to grandMA2
Приложение для связи MIDI контроллера и светового пульта семейства grandMA2

## Используемые технологии
Приложение написано на языке C#, интерфейс на winfowrms. Для разработки и компиляции приложения достаточно иметь Visual Studio и .net framework.

Дополнительно используются библиотеки:

- NAudio - для работы с MIDI входом
- Newtonsoft.Json - для работы с форматом JSON
- WebSocketSharp - для связи с пультом по websocket

Содержимое директории "gma" было найдено на просторах интернета, поэтому я не знаю авторов файлов MA2Connector.cs, MA2Playbacks.cs и MATypes.cs, но с удовльствием их укажу тут, если когда-то узнаю их.

## Полезные ссылки
- [Сайт приложения](https://midigma.com)
- [Чат пользователей в телеграме](https://t.me/midi2gma)