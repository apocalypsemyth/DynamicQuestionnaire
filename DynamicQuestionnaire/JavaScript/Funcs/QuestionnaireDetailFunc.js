function BackToListForServer() {
    window.location.href = "QuestionnaireList.aspx";
    return true;
}

function ResetUserInputsForServer() {
    $(txtUserName).val("");
    $(txtUserPhone).val("");
    $(txtUserEmail).val("");
    $(txtUserAge).val("");
}
function ResetUserInputsItsIsInvalidClassForServer() {
    $(txtUserName).removeClass(isInvalidClass);
    $(txtUserPhone).removeClass(isInvalidClass);
    $(txtUserEmail).removeClass(isInvalidClass);
    $(txtUserAge).removeClass(isInvalidClass);
}
function ResetUserInputsItsValidMessageForServer() {
    $(divValidateUserName).text("");
    $(divValidateUserPhone).text("");
    $(divValidateUserEmail).text("");
    $(divValidateUserAge).text("");
}
function ResetUserAnswerInputsForServer() {
    $(rdoQuestionAnswer).prop("checked", false);
    $(ckbQuestionAnswer).prop("checked", false);
    $(txtQuestionAnswer).val("");
}

function ResetQuestionnaireDetailInputsForServer() {
    let queryString = window.location.search;
    let isExistQueryString = queryString.indexOf("?ID=") !== -1;
    let isCheckingQuestionnaireDetail =
        window.location.href.indexOf("CheckingQuestionnaireDetail.aspx") !== -1
    let strQuestionnaireID =
        isExistQueryString
            ? !isCheckingQuestionnaireDetail
                ? queryString.split("?ID=")[1]
                : NULL
            : FAILED;

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
                ResetUserInputsForServer();
                ResetUserInputsItsIsInvalidClassForServer();
                ResetUserInputsItsValidMessageForServer();
                ResetUserAnswerInputsForServer();
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