var ResetUserInputs = function () {
    $(txtUserName).val("");
    $(txtUserPhone).val("");
    $(txtUserEmail).val("");
    $(txtUserAge).val("");
}
var ResetUserInputsItsIsInvalidClass = function () {
    $(txtUserName).removeClass(isInvalidClass);
    $(txtUserPhone).removeClass(isInvalidClass);
    $(txtUserEmail).removeClass(isInvalidClass);
    $(txtUserAge).removeClass(isInvalidClass);
}
var ResetUserInputsItsValidMessage = function () {
    $(divValidateUserName).text("");
    $(divValidateUserPhone).text("");
    $(divValidateUserEmail).text("");
    $(divValidateUserAge).text("");
}
var SetUserInputsItsIsInvalidClass = function (strInputSelector, strDivSelector, strDivErrMsg) {
    $(strInputSelector).addClass(isInvalidClass);
    $(strDivSelector).text(strDivErrMsg);
}
var GetUserInputs = function () {
    let strUserName = $(txtUserName).val();
    let strUserPhone = $(txtUserPhone).val();
    let strUserEmail = $(txtUserEmail).val();
    let strUserAge = $(txtUserAge).val();

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
        SetUserInputsItsIsInvalidClass(
            txtUserName,
            divValidateUserName,
            "請填入您的姓名。"
        );
    }

    if (!objUser.phone) {
        resultChecked = false;
        SetUserInputsItsIsInvalidClass(
            txtUserPhone,
            divValidateUserPhone,
            "請填入您的手機。"
        );
    }
    else
    {
        if (!phoneRx.test(objUser.phone)) {
            resultChecked = false;
            SetUserInputsItsIsInvalidClass(
                txtUserPhone,
                divValidateUserPhone,
                `請以 "0123456789" 開頭零後九碼的格式填寫。`
            );
        }
    }

    if (!objUser.email)
    {
        resultChecked = false;
        SetUserInputsItsIsInvalidClass(
            txtUserEmail,
            divValidateUserEmail,
            "請填入您的信箱。"
        );
    }
    else
    {
        if (!emailRx.test(objUser.email)) {
            resultChecked = false;
            SetUserInputsItsIsInvalidClass(
                txtUserEmail,
                divValidateUserEmail,
                "請填入合法的信箱格式。"
            );
        }
    }

    if (!objUser.age)
    {
        resultChecked = false;
        SetUserInputsItsIsInvalidClass(
            txtUserAge,
            divValidateUserAge,
            "請填入您的年齡。"
        );
    }
    else
    {
        if (isNaN(objUser.age)) {
            resultChecked = false;
            SetUserInputsItsIsInvalidClass(
                txtUserAge,
                divValidateUserAge,
                "請填寫數字。"
            );
        }
        else if (objUser.age <= 0) {
            resultChecked = false;
            SetUserInputsItsIsInvalidClass(
                txtUserAge,
                divValidateUserAge,
                "請填寫大於零的年齡。"
            );
        }
        else if (objUser.age > 150) {
            resultChecked = false;
            SetUserInputsItsIsInvalidClass(
                txtUserAge,
                divValidateUserAge,
                "請填寫小於150的年齡。"
            );
        }
    }

    return resultChecked;
}

var ResetUserAnswerInputs = function () {
    $(rdoQuestionAnswer).prop("checked", false);
    $(ckbQuestionAnswer).prop("checked", false);
    $(txtQuestionAnswer).val("");
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

    if (!$(rdoQuestionAnswer + "[required=True]:checked").length) {
        resultChecked = false;
        arrResult.push("請勾選必填單選方塊。");
    }

    if (!$(ckbQuestionAnswer + "[required=True]:checked").length) {
        resultChecked = false;
        arrResult.push("請勾選必填複選方塊。");
    }

    let requiredText = $(txtQuestionAnswer + "[required=True]");
    let arrAnsweredRequiredText = GetAnsweredTextInputs(txtQuestionAnswer + "[required=True]");
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

    if ($(rdoQuestionAnswer).length) {
        arrResult.push(
            GetUserAnswerInputs(
                rdoQuestionAnswer + ":checked"
                , SINGLE_SELECT)
        );
    }

    if ($(ckbQuestionAnswer).length) {
        arrResult.push(
            GetUserAnswerInputs(
                ckbQuestionAnswer + ":checked"
                , MULTIPLE_SELECT)
        );
    }

    let arrAnsweredText = GetAnsweredTextInputs(txtQuestionAnswer);
    if (arrAnsweredText.length) {
        arrResult.push(
            GetUserAnswerInputs(
                txtQuestionAnswer
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

var ResetCheckingOrNotQuestionnaireDetailSession = function () {
    $.ajax({
        url: "/API/QuestionnaireDetailDataHandler.ashx?Action=RESET_CHECKING_OR_NOT_QUESTIONNAIREDETAIL_SESSION",
        method: "GET",
        success: function (strMsg) {
            if (strMsg === SUCCESSED) {
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var ResetQuestionnaireDetailInputs = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/QuestionnaireDetailDataHandler.ashx?Action=RESET_QUESTIONNAIREDETAIL_INPUTS",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        beforeSend: function () {
            $(loadingProgressBarContainer).show();
            $(loadingProgressBar).animate({ width: startPercent }, 1);
        },
        success: function (strMsg) {
            if (strMsg === FAILED) {
                alert(errorMessageOfRetry);
            }
            else if (strMsg === NULL) {
            }
            else if (strMsg === NULL + FAILED) {
                ResetUserInputs();
                ResetUserInputsItsIsInvalidClass();
                ResetUserInputsItsValidMessage();
                ResetUserAnswerInputs();
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        },
        complete: function () {
            $(loadingProgressBar).animate({ width: endPercent }, 1);
            $(loadingProgressBarContainer).fadeOut(fadeOutDuration);
        }
    });
}