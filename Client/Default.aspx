<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!--Подключаем стили bootstrap и собственные стили-->
    <link href="assets/css/bootstrap.css" rel="stylesheet" />
    <link href="assets/css/main.css" rel="stylesheet" />
    <!--Подключаем семейство шрифтов-->
    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">
        <!--В контейнере с данным id рендерится приложение Vue.js-->
        <div id="app">

            <!--Секция для подтверждения сохранения данных. При нажатии кнопки "Сохранить"
                отображается строка подтверждения, где можно согласиться или отменить изменения-->
            <div class="saveSection">
                <button v-if="!saveConfirm" @click.prevent="getCountOfChanges" class="btn btn-primary">Сохранить</button>
                <div v-else class="confirm">
                    <span>Было изменено <strong>{{ countOfChanges }}</strong> полей. Сохранить новые данные?</span>
                    <button @click.prevent="save" class="btn btn-success">Сохранить</button>
                    <button @click.prevent="cancel" class="btn btn-warning">Отмена</button>
                </div>
            </div>

            <!--Главный раздел. Делится на левую и правую часть-->
            <div class="main">
            <div class="left">
                <!--Сначала отображаем заголовки для столбцов таблицы-->
                <ul class="title">
                    <li>
                        <span>UserID</span>
                        <span>UserName</span>
                        <span>MaskUserID</span>
                        <span></span>
                    </li>
                </ul>
                <!--Отображаем все данные масок-->
                <ul>
                    <li v-for="(item, i) in masks.items">
                        <span>{{ item.UserID }}</span>
                        <!--При нажатии на никнейм вызывем метод получения логов для данного пользователя-->
                        <span @click.prevent="getLogs(item.UserID, item.UserName)"><strong class="pointer">{{ item.UserName }}</strong></span>
                        <!--При нажатии на значение маски на ее место появляется input для ввода нового значения-->
                        <span v-if="!item.isChanging" @click="setChanging(i)" class="pointer text-center">{{ item.MaskUserID }} </span>
                        <!--Значение input нельзя изменять во время подтверждения сохранения. 
                            Также устанавливается двойной data-binding для связи поля со значением "newValue"-->
                        <input :disabled="saveConfirm" v-else type="text" v-model="item.newValue" value="item.newValue"/>
                        <!--При нажатии на кнопку вызывается метод удаления маски-->
                        <button @click.prevent="deleteMask(item.UserID)" class="btn btn-danger">Удалить</button>
                    </li>
                </ul>
            </div>

            <!--Правая часть-->
            <div class="right">
                <!--Отображение имени пользователя, для которого загружается история логов-->
                <span v-if="logUser">История масок пользователя <strong>{{ logUser }}</strong></span>
                <!--Отображение истории логов-->
                <ul>
                    <li v-for="(log, i) in logs.items">
                        {{ log.MaskUserID }}
                    </li>
                </ul>
            </div>
            </div>

        </div>
    </form>

    <!--Подключение скрипта Vue.js и скрипта самого приложения-->
    <script src="assets/js/vue.js"></script>
    <script src="assets/js/main.js"></script>
</body>
</html>
