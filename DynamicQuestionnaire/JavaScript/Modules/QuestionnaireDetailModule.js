const SINGLE_SELECT = "單選方塊";
const MULTIPLE_SELECT = "複選方塊";
const TEXT = "文字";

var ResetUserInputsIsInvalidClass = function () {
    $("input[id*=txtUserName]").removeClass("is-invalid");
    $("input[id*=txtUserPhone]").removeClass("is-invalid");
    $("input[id*=txtUserEmail]").removeClass("is-invalid");
    $("input[id*=txtUserAge]").removeClass("is-invalid");
}
var ResetUserInputs = function () {
    $("div[id=divValidateUserName]").text("");
    $("div[id=divValidateUserPhone]").text("");
    $("div[id=divValidateUserEmail]").text("");
    $("div[id=divValidateUserAge]").text("");
}
var SetUserInputsIsInvalidClass = function (strInputSelector, strDivSelector, strDivErrMsg) {
    $(strInputSelector).addClass("is-invalid");
    $(strDivSelector).text(strDivErrMsg);
}
var GetUserInputs = function () {
    let strUserName = $("input[id*=txtUserName]").val();
    let strUserPhone = $("input[id*=txtUserPhone]").val();
    let strUserEmail = $("input[id*=txtUserEmail]").val();
    let strUserAge = $("input[id*=txtUserAge]").val();

    let objUser = {
        "userName": strUserName,
        "phone": strUserPhone,
        "email": strUserEmail,
        "age": strUserAge,
    };

    return objUser;
}
var CheckUserInputs = function (objUser) {
    let resultChecked = true;
    let phoneRx = /^[0]{1}[0-9]{9}/;
    let emailRx = /^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!objUser.userName) {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id*=txtUserName]",
            "div[id=divValidateUserName]",
            "請填入您的姓名。"
        );
    }

    if (!objUser.phone) {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id*=txtUserPhone]",
            "div[id=divValidateUserPhone]",
            "請填入您的手機。"
        );
    }
    else
    {
        if (!phoneRx.test(objUser.phone)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id*=txtUserPhone]",
                "div[id=divValidateUserPhone]",
                `請以 "0123456789" 開頭零後九碼的格式填寫。`
            );
        }
    }

    if (!objUser.email)
    {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id*=txtUserEmail]",
            "div[id=divValidateUserEmail]",
            "請填入您的信箱。"
        );
    }
    else
    {
        if (!emailRx.test(objUser.email)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id*=txtUserEmail]",
                "div[id=divValidateUserEmail]",
                "請填入合法的信箱格式。"
            );
        }
    }

    if (!objUser.age)
    {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id*=txtUserAge]",
            "div[id=divValidateUserAge]",
            "請填入您的年齡。"
        );
    }
    else
    {
        if (isNaN(objUser.age)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id*=txtUserAge]",
                "div[id=divValidateUserAge]",
                "請填寫數字。"
            );
        }
    }

    return resultChecked;
}

var GetAnsweredTextInputs = function (strInputSelector) {
    let textEl = $(strInputSelector);
    let arrAnsweredText = [];

    textEl.each(function () {
        let answeredText = $(this).val();
        if (answeredText)
            arrAnsweredText.push(answeredText);
    });

    return arrAnsweredText;
}
var CheckRequiredQuestionInputs = function () {
    let resultChecked = true;
    let arrResult = [];

    if (!$("input[id*=rdoQuestionAnswer][required=True]:checked").length) {
        resultChecked = false;
        arrResult.push("請勾選必填單選方塊。");
    }

    if (!$("input[id*=ckbQuestionAnswer][required=True]:checked").length) {
        resultChecked = false;
        arrResult.push("請勾選必填複選方塊。");
    }

    let requiredText = $("input[id*=txtQuestionAnswer][required=True]");
    let arrAnsweredRequiredText = GetAnsweredTextInputs("input[id*=txtQuestionAnswer][required=True]");
    if (requiredText.length !== arrAnsweredRequiredText.length) {
        resultChecked = false;
        arrResult.push("請填寫必填文字。");
    }

    if (arrResult.length)
        return arrResult.join("\n");
    else
        return resultChecked;
}
var GetUserAnswerInputs = function (strQuestionTypingItsControl, strQuestionTyping) {
    let arrResult = [];

    $(strQuestionTypingItsControl).each(function () {
        let arrQuestionID_AnswerNum_Answer_QuestionTyping = $(this).attr("id").split("_").slice(1);
        let strAnswer = $(this).val();
        if (strAnswer) {
            arrQuestionID_AnswerNum_Answer_QuestionTyping.push(strAnswer, strQuestionTyping);
            arrResult.push(arrQuestionID_AnswerNum_Answer_QuestionTyping.join("_"));
        }
    });

    return arrResult;
}
var CheckAtLeastOneQuestionInputs = function () {
    let arrResult = [];

    if ($("input:radio[id*=rdoQuestionAnswer]").length) {
        arrResult.push(
            GetUserAnswerInputs(
                "input:radio[id*=rdoQuestionAnswer]:checked"
                , SINGLE_SELECT)
        );
    }

    if ($("input:checkbox[id*=ckbQuestionAnswer]").length) {
        arrResult.push(
            GetUserAnswerInputs(
                "input:checkbox[id*=ckbQuestionAnswer]:checked"
                , MULTIPLE_SELECT)
        );
    }

    let arrAnsweredText = GetAnsweredTextInputs("input[id*=txtQuestionAnswer]");
    if (arrAnsweredText.length) {
        arrResult.push(
            GetUserAnswerInputs(
                "input[id*=txtQuestionAnswer]"
                , TEXT)
        );
    }

    let arrUserAnswer = $.map(arrResult, function (n) {
        return n;
    })

    if (!arrUserAnswer.length)
        return "請至少作答一個問題。";
    else
        return arrUserAnswer;
}