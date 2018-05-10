$(document).ready(function () {
    $('form').on('submit', function (e) {
        e.preventDefault();
    });
    var postMsg = function () {
        var $el = $(".input-msg");
        var msg = $el.val();
        $el.val("");
        $("input").attr("disabled", true);
        $.ajax({
            url: "/Home/PostMessage",
            method: "POST",
            data: {
                message: msg
            }
        }).done(function() {
            reloadMsgs();
            $el.focus();
        }).fail(function () {
            location.reload(true);
        }).always(function () {
            $("input").attr("disabled", false);
        });
    }
    var reloadMsgs = function() {
        $.ajax({
            url: "/Home/GetMessage"
        }).done(function (data) {
            $("span").remove();
            _.each(data.UserMessages, function(msgStr) {
                $("#user-messages").append(`<span>${msgStr}</span>`);
            });
            _.each(data.AllMessages, function (msg) {
                $("#last-messages").append(`<span>Id пользователя: ${msg.UserId}<br/>Сообщение: ${msg.Message}</span>`);
            });
        });
    }
    $(".input-msg").on('keyup', function (e) {
        if (e.keyCode == 13) {
            postMsg();
        }
    });
    $(".input-btn").click(function() {
        postMsg();
    });
    reloadMsgs();
    setInterval(function() {
        reloadMsgs();
    }, 2000);
});