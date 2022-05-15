function ResetCommonQuestionDetailInputsForServer() {
    $(txtCommonQuestionName).val("");

    $(selectCategoryList).val(commonQuestionOfCategoryName).change();
    $(selectTypingList).val(SINGLE_SELECT).change();
    $(txtQuestionNameOfCommonQuestion).val("");
    $(txtQuestionAnswerOfCommonQuestion).val("");
    $(ckbQuestionRequiredOfCommonQuestion).prop("checked", false);
}
function GetCommonQuestionInputsForServer() {
    let strCommonQuestionName = $(txtCommonQuestionName).val();

    let objCommonQuestion = {
        "commonQuestionName": strCommonQuestionName,
    };

    return objCommonQuestion;
}
function CheckCommonQuestionInputsForServer(objCommonQuestion) {
    let arrErrorMsg = [];

    if (!objCommonQuestion.commonQuestionName)
        arrErrorMsg.push("請填入常用問題名稱。");

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join();
    else
        return true;
}

function SubmitCommonQuestionForServer(strOperate) {
    let objCommonQuestionForServer = GetCommonQuestionInputsForServer();
    let isValidCommonQuestionForServer =
        CheckCommonQuestionInputsForServer(objCommonQuestionForServer);
    if (typeof isValidCommonQuestionForServer === "string") {
        alert(isValidCommonQuestionForServer);
        return false;
    }

    $.ajax({
        async: false,
        url: `/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=${strOperate}_COMMONQUESTION`,
        method: "POST",
        data: objCommonQuestionForServer,
        success: function (strOrObjCommonQuestion) {
            if (strOrObjCommonQuestion === SUCCESSED) 
                return true;
            else if (strOrObjCommonQuestion != null) {
                return true;
            }
            else {
                alert(errorMessageOfRetry);
                return false;
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
            return false;
        }
    });
}