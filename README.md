### Цель

Создать систему, которая следит за потенциально опасными объектами, приближающимися к Земле (астероиды и метеороиды), используя открытое NASA NEO API.

### 🔁 1. Сервис синхронизации

- Загружает данные с NASA API:
    
    ```
    https://api.nasa.gov/neo/rest/v1/feed?start_date=YYYY-MM-DD&end_date=YYYY-MM-DD&api_key=YOUR_KEY
    
    ```
    
- Сохраняет информацию об объектах в базу данных
- Работает по расписанию (например, раз в сутки), обновляя данные с учётом изменений

- ### 🌐 2. Web API

- Возвращает отфильтрованные и сгруппированные данные:
    - фильтрация по дате, опасности (`is_potentially_hazardous_asteroid`)
    - группировка по дню
    - сортировка по количеству объектов, по максимальному диаметру и т.д.

 ### 🖼 3. UI

- Показывает таблицу с опасными объектами по дням
- Позволяет фильтровать, сортировать, просматривать детальную информацию


---


### Goal

To create a system that monitors potentially dangerous objects approaching Earth (asteroids and meteoroids) using the open NASA NEO API.

### 🔁 1. Synchronization Service

- Downloads data from the NASA API:
    
    ```
    https://api.nasa.gov/neo/rest/v1/feed?start_date=YYYY-MM-DD&end_date=YYYY-MM-DD&api_key=YOUR_KEY
    
    ```
    
- Stores object information in a database
- Runs on a schedule (for example, once a day), updating the data based on changes

- ### 🌐 2. Web API

- Returns filtered and grouped data:
    - filtering by date and danger (`is_potentially_hazardous_asteroid`)
    - grouping by day
    - sorting by the number of objects, maximum diameter, etc.

 ### 🖼 3. UI

- Displays a table with dangerous objects by day
- Allows filtering, sorting, and viewing detailed information
