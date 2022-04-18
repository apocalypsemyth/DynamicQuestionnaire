var ResetUserInputsIsInvalidClass = function () {
    $("input[id=txtUserName]").removeClass("is-invalid");
    $("input[id=txtUserPhone]").removeClass("is-invalid");
    $("input[id=txtUserEmail]").removeClass("is-invalid");
    $("input[id=txtUserAge]").removeClass("is-invalid");
}
var ResetUserInputs = function () {
    $("div[id=divValidateUserName]").text("");
    $("div[id=divValidateUserPhone]").text("");
    $("div[id=divValidateUserEmail]").text("");
    $("div[id=divValidateUserAge]").text("");
}
var SetUserInputsIsInvalidClass = function (strInputID, strDivID, strDivErrMsg) {
    $(strInputID).addClass("is-invalid");
    $(strDivID).text(strDivErrMsg);
}
var GetUserInputs = function () {
    let strUserName = $("input[id=txtUserName]").val();
    let strUserPhone = $("input[id=txtUserPhone]").val();
    let strUserEmail = $("input[id=txtUserEmail]").val();
    let strUserAge = $("input[id=txtUserAge]").val();

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
            "input[id=txtUserName]",
            "div[id=divValidateUserName]",
            "請填入您的姓名。"
        );
    }

    if (!objUser.phone) {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id=txtUserPhone]",
            "div[id=divValidateUserPhone]",
            "請填入您的手機。"
        );
    }
    else
    {
        if (!phoneRx.test(objUser.phone)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id=txtUserPhone]",
                "div[id=divValidateUserPhone]",
                `請以 "0123456789" 開頭零後九碼的格式填寫。`
            );
        }
    }

    if (!objUser.email)
    {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id=txtUserEmail]",
            "div[id=divValidateUserEmail]",
            "請填入您的信箱。"
        );
    }
    else
    {
        if (!emailRx.test(objUser.email)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id=txtUserEmail]",
                "div[id=divValidateUserEmail]",
                "請填入合法的信箱格式。"
            );
        }
    }

    if (!objUser.age)
    {
        resultChecked = false;
        SetUserInputsIsInvalidClass(
            "input[id=txtUserAge]",
            "div[id=divValidateUserAge]",
            "請填入您的年齡。"
        );
    }
    else
    {
        if (isNaN(objUser.age)) {
            resultChecked = false;
            SetUserInputsIsInvalidClass(
                "input[id=txtUserAge]",
                "div[id=divValidateUserAge]",
                "請填寫數字。"
            );
        }
    }

    return resultChecked;
}

var GetUserAnswerInputs = function (strQuestionTypingItsControl, strQuestionTyping) {
    let arrResult = [];

    $(strQuestionTypingItsControl).each(function () {
        let arrQuestionID_AnswerNum_Answer_QuestionTyping = $(this).attr("id").split("_").slice(1);
        let strAnswer = $(this).val();
        arrQuestionID_AnswerNum_Answer_QuestionTyping.push(strAnswer, strQuestionTyping);

        arrResult.push(arrQuestionID_AnswerNum_Answer_QuestionTyping.join("_"));
    });

    return arrResult;
}