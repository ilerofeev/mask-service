new Vue({
    // Задаем id, где будет отрисованы данные
    el: "#app",
    // Задаем данные, хранящиеся в приложении
    data() {
        return {
            // Переменная отвечающая за показ строки сохранения
            saveConfirm: false,
            // Переменная, отвечающая за кол-во полей, которые будут изменены при сохранении
            countOfChanges: 0,
            // Массив объектов, содержащий данные о всех масках
            masks: { items: [] },
            // Информация о всех логах
            logs: { items: [] },
            // Никнейм пользователя, для которого в данный момент отображаются логи
            logUser: ""
        }
    },
    // Задаем методы, используемые в приложении
    methods: {
        // Получение кол-ва полей, которые будут изменены при сохранении
        getCountOfChanges: function () {
            // Показываем или убираем строку сохранения
            this.saveConfirm = !this.saveConfirm;
            // Если еще не считали кол-во полей
            if (this.countOfChanges == 0) {
                // Проходим по всем маскам и сравниваем новое заданное значение со старым
                // При нахождении разницы уеличиваем счетчик
                for (var i = 0; i < this.masks.items.length; i++) {
                    if (this.masks.items[i].newValue && this.masks.items[i].newValue != this.masks.items[i].MaskUserID)
                        this.countOfChanges++;
                };
            }
        },
        // Метод получения всех масок
        getMasks: function () {
            // Задаем url запроса
            var url = "http://localhost:53442/Service1.svc//usermasks/";
            // Делаем запрос по ссылке с методом GET
            fetch(url, {
                method: "GET"
            })
                // Ответ переводим в формат JSON
                .then(response => response.json())
                // Получения из ответа объекта, содержащего маски
                // и сохранение его в переменную приложения
                .then(json => this.masks.items = json.GetUserMasksResult)
        },
        // Метод получения логов
        getLogs: function (UserID, UserName) {
            // Показываем никнейм пользователя, для которого отображаются логи
            this.logUser = UserName;
            // Делаем запрос
            var url = "http://localhost:53442/Service1.svc//logs/" + UserID;
            fetch(url, {
                method: "GET"
            })
                // JSON данные сохраняем в переменную приложения
                .then(response => response.json())
                .then(json => this.logs.items = json.GetLogsResult)
        },
        // Метод отмены, который задает свойство isChanging в false у всех объектов
        cancel: function() {
            for (var i = 0; i < this.masks.items.length; i++) 
            {
                // Нельзя императивно изменить упомянутое свойство, точнее возникнут проблемы
                // с реактивным обновлением состояния DOM, поэтому используется метод Vue.set для
                // обновления массива по индексу. Но таким образом нельзя изменить отдельное свойство,
                // поэтому обновляется весь объект полностью, куда копируются все старые данные,
                // но свойство isChanging устанавливается в false
                let newObj = {};
                newObj.isChanging = false;
                newObj.UserID = this.masks.items[i].UserID;
                newObj.UserName = this.masks.items[i].UserName;
                newObj.MaskUserID = this.masks.items[i].MaskUserID;
                newObj.newValue = newObj.MaskUserID;
                Vue.set(this.masks.items, i, newObj);
            };
            // Показ или скрытие строки сохранения
            this.saveConfirm = !this.saveConfirm;
            // Сброс счетчика
            this.countOfChanges = 0;
        },
        // Метод, позволяющий учесть при сохранении случай, когда новое введеное число равно старому
        // значению маски. Тогда данный объект обновляется, значение newValue приравнивается
        // маске, а значение isChanging устанавливается в false, чтобы скрыть input
        maskEqualNewVal: function () {
            for (var i = 0; i < this.masks.items.length; i++) {
                if (this.masks.items[i].MaskUserID == this.masks.items[i].newValue) {
                    let newObj = {};
                    newObj.isChanging = false;
                    newObj.UserID = this.masks.items[i].UserID;
                    newObj.UserName = this.masks.items[i].UserName;
                    newObj.MaskUserID = this.masks.items[i].MaskUserID;
                    newObj.newValue = newObj.MaskUserID;
                    Vue.set(this.masks.items, i, newObj);
                }
            };
        },
        // Метод сохранения и отправки в БД новых данных
        save: function () {
            // Проходим по всем маскам. Проверяем, если есть новое значение, и оно не равно старому
            // значению маски, то отправляем это значение в БД
            for (var i = 0; i < this.masks.items.length; i++) {
                if (this.masks.items[i].newValue != this.masks.items[i].MaskUserID &&
                    this.masks.items[i].newValue != undefined) {
                    this.updateMask(this.masks.items[i].UserID, this.masks.items[i].MaskUserID, this.masks.items[i].newValue);
                } 
            };
            // Вызов метода, скрывающего input
            this.maskEqualNewVal();
            // Показ или скрытие строки сохранения
            this.saveConfirm = !this.saveConfirm;
            // Сброс счетчика
            this.countOfChanges = 0;
        },
        // Метод меняющий состояние isChanging по значению позиции. При смене состояния данной
        // переменной отображается или текстовое поле с текущей маской или input для ввода новой маски
        setChanging: function (position) {
            let newObj = {};
            newObj.isChanging = !this.masks.items[position].isChanging;
            newObj.UserID = this.masks.items[position].UserID;
            newObj.UserName = this.masks.items[position].UserName;
            newObj.MaskUserID = this.masks.items[position].MaskUserID;
            this.$set(this.masks.items, position, newObj);
        },
        // Метод удаленя маски. По UserID удаляется маска из соответствующей таблицы БД
        deleteMask: function (UserID) {
            var url = "http://localhost:53442/Service1.svc//delete/" + UserID;
            fetch(url, {
                method: "GET"
            })
                .then(response => this.getMasks())
        },
        // Метод обновления маски. По UserID обновляется маска MaskUserID переменной newValue,
        // а сама маска MaskUserID попадает в лог с соответствующим UserID
        updateMask: function (UserID, MaskUserID, newValue) {
            var url = "http://localhost:53442/Service1.svc//update/"+UserID+"_"+MaskUserID+"_"+newValue;
            fetch(url, {
                method: "GET"
            })
                .then(response => this.getMasks())
        }
    },
    // При создании экземпляра Vue вызывается метод получения всех масок
    created: function () {
        this.getMasks();
    }
});