const FAILED = "FAILED";
const SINGLE_SELECT = "單選方塊";
const MULTIPLE_SELECT = "複選方塊";
const TEXT = "文字";

var CheckUserInputs = function () {
    $("span[id*=lblUserName]").val("");
    $("span[id*=lblUserPhone]").val("");
    $("span[id*=lblUserEmail]").val("");
    $("span[id*=lblUserAge]").val("");

    let resultChecked = true;
    let phoneRx = /^[0]{1}\d{9}"/;
    let emailRx = /^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!$("input[id*=txtUserName]").val()) {
        resultChecked = false;
        $("span[id*=lblUserName]").val("請填入您的姓名。");
    }

    if (!$("input[id*=txtUserPhone]").val()) {
        resultChecked = false;
        $("span[id*=lblUserPhone]").val("請填入您的手機。");
    }
    else
    {
        if (!phoneRx.test($("input[id*=txtUserPhone]").val())) {
            resultChecked = false;
            $("span[id*=lblUserPhone]").val(`請以 "0123456789" 開頭零後九碼的格式填寫。`);
        }
    }

    if (!$("input[id*=txtUserEmail]").val())
    {
        resultChecked = false;
        $("span[id*=lblUserEmali]").val("請填入您的信箱。");
    }
    else
    {
        if (!emailRx.test($("input[id*=txtUserEmail]").val())) {
            resultChecked = false;
            $("span[id*=lblUserEmail]").val("請填入合法的信箱格式。");
        }
    }

    if (!$("input[id*=txtUserAge]").val())
    {
        resultChecked = false;
        $("span[id*=lblUserAge]").val("請填入您的年齡。");
    }
    else
    {
        if (isNaN($("input[id*=txtUserAge]").val())) {
            resultChecked = false;
            $("span[id*=lblUserAge]").val("請填數數字。");
        }
    }

    return resultChecked;
}
var GetUserQuestionAnswerInputs = function (strQuestionTypingItsControl, strQuestionTyping) {
    let arrResult = [];

    $(strQuestionTypingItsControl).each(function () {
        let arrQuestionID_AnswerNum_Answer_QuestionTyping = $(this).attr("id").split("_").slice(1);
        let strAnswer = $(this).val();
        arrQuestionID_AnswerNum_Answer_QuestionTyping.push(strAnswer, strQuestionTyping);

        arrResult.push(arrQuestionID_AnswerNum_Answer_QuestionTyping.join("_"));
    });

    return arrResult;
}
var CreateUserQuestion = function (strUserQuestion) {
    $.ajax({
        url: "/API/QuestionnaireDetailDataHandler.ashx?Action=CREATE_USERQUESTION",
        method: "POST",
        data: { "userQuestion": strUserQuestion },
        success: function (strOrArrUserQuestionTemp) {
            if (strOrArrUserQuestionTemp === FAILED)
                alert("請至少作答一個問題。");
            else {
                console.log(strOrArrUserQuestionTemp);

                for (let userQuestion of strOrArrUserQuestionTemp) {
                    if (userQuestion.QuestionTyping === SINGLE_SELECT) {
                        $(`input:radio[id*=rdoQuestionAnswer_${userQuestion.QuestionID}_${userQuestion.AnswerNum}]`).parent().find("input").empty();
                    }
                }
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}