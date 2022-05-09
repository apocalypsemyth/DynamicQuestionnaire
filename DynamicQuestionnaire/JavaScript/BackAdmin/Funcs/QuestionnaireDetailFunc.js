function SetCommonQuestionOnQuestionnaireStateSession(strSettedOrNotState) {
    if (sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState) == null) {
        sessionStorage.setItem(currentSetCommonQuestionOnQuestionnaireState, strSettedOrNotState);
    }
}

function GetQuestionnaireInputsForServerSubmit() {
    let strCaption = $("input[id*=txtCaption]").val();
    let strDescription = $("textarea[id*=txtDescription]").val();
    let strStartDate = $("input[id*=txtStartDate]").val();
    let strEndDate = $("input[id*=txtEndDate]").val();
    let boolIsEnable = $("input[id*=ckbIsEnable]").is(":checked");

    let objQuestionnaire = {
        "caption": strCaption,
        "description": strDescription,
        "startDate": strStartDate,
        "endDate": strEndDate,
        "isEnable": boolIsEnable,
    };

    return objQuestionnaire;
}
function CheckQuestionnaireInputsForServerSubmit(objQuestionnaire) {
    let arrErrorMsg = [];
    let regex = /^[0-9]{4}\/(0[1-9]|1[0-2])\/(0[1-9]|[1-2][0-9]|3[0-1])$/;

    if (!objQuestionnaire.caption)
        arrErrorMsg.push("請填入問卷名稱。");

    if (!objQuestionnaire.description)
        arrErrorMsg.push("請填入描述內容。");

    if (!objQuestionnaire.startDate)
        arrErrorMsg.push("請填入開始時間。");
    else {
        if (!regex.test(objQuestionnaire.startDate))
            arrErrorMsg.push(`請以 "yyyy/MM/dd" 的格式輸入開始時間。`);
        else {
            let today = new Date().toDateString();
            let startDate = new Date(objQuestionnaire.startDate);

            if (startDate.toDateString() < today)
                arrErrorMsg.push("請填入今天或其後的開始時間。");
        }
    }

    if (objQuestionnaire.endDate) {
        if (!regex.test(objQuestionnaire.endDate))
            arrErrorMsg.push(`請以 "yyyy/MM/dd" 的格式輸入結束時間。`);
        else {
            let today = new Date().toDateString();
            let startDate = new Date(objQuestionnaire.startDate);
            let endDate = new Date(objQuestionnaire.endDate);

            if (endDate.toDateString() < today)
                arrErrorMsg.push("請填入今天或其後的結束時間。");
            if (startDate > endDate) 
                arrErrorMsg.push("請填入一前一後時序的始末日期。");
        }
    }

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join("\n");
    else
        return true;
}

function SubmitQuestionnaire(strOperate) {
    let objQuestionnaireForServerSubmit = GetQuestionnaireInputsForServerSubmit();
    let isValidQuestionnaireForServerSubmit =
        CheckQuestionnaireInputsForServerSubmit(objQuestionnaireForServerSubmit);
    if (typeof isValidQuestionnaireForServerSubmit === "string") {
        alert(isValidQuestionnaireForServerSubmit);
        return false;
    }

    $.ajax({
        async: false,
        url: `/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=${strOperate}_QUESTIONNAIRE`,
        method: "POST",
        data: objQuestionnaireForServerSubmit,
        success: function (strMsg) {
            if (strMsg === FAILED) {
                alert(errorMessageOfRetry);
                return false;
            }
            else if (strMsg === SUCCESSED)
                return true;
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
            return false;
        }
    });
}