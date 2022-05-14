function GetCommonQuestionInputsForServerSubmit() {
    let strCommonQuestionName = $(txtCommonQuestionName).val();

    let objCommonQuestion = {
        "commonQuestionName": strCommonQuestionName,
    };

    return objCommonQuestion;
}
function CheckCommonQuestionInputsForServerSubmit(objCommonQuestion) {
    let arrErrorMsg = [];

    if (!objCommonQuestion.commonQuestionName)
        arrErrorMsg.push("請填入常用問題名稱。");

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join();
    else
        return true;
}

function SubmitCommonQuestion(strOperate) {
    let objCommonQuestionForServerSubmit = GetCommonQuestionInputsForServerSubmit();
    let isValidCommonQuestionForServerSubmit =
        CheckCommonQuestionInputsForServerSubmit(objCommonQuestionForServerSubmit);
    if (typeof isValidCommonQuestionForServerSubmit === "string") {
        alert(isValidCommonQuestionForServerSubmit);
        return false;
    }

    $.ajax({
        async: false,
        url: `/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=${strOperate}_COMMONQUESTION`,
        method: "POST",
        data: objCommonQuestionForServerSubmit,
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