var GetQuestionnaireInputs = function () {
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
var GetQuestionInputs = function () {
    let strQuestionName = $("input[id*=txtQuestionName]").val();
    let strQuestionAnswer = $("input[id*=txtQuestionAnswer]").val();
    let strCategoryName = $("select[id*=ddlCategoryList]").val();
    let strTypingName = $("select[id*=ddlTypingList]").val();
    let boolQuestionRequired = $("input[id*=ckbQuestionRequired]").is(":checked");

    let objQuestion = {
        "questionName": strQuestionName,
        "questionAnswer": strQuestionAnswer,
        "questionCategory": strCategoryName,
        "questionTyping": strTypingName,
        "questionRequired": boolQuestionRequired,
    };

    return objQuestion;
}
var CheckQuestionnaireInputs = function (objQuestionnaire) {
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
    }

    if (objQuestionnaire.endDate) {
        if (!regex.test(objQuestionnaire.endDate))
            arrErrorMsg.push(`請以 "yyyy/MM/dd" 的格式輸入結束時間。`);
    }

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join("\n");
    else
        return true;
}
var CheckQuestionInputs = function (objQuestion) {
    let arrErrorMsg = [];

    if (!objQuestion.questionName)
        arrErrorMsg.push("請填入問題名稱。");

    if (!objQuestion.questionAnswer)
        arrErrorMsg.push("請填入問題回答。");
    else {
        let checkingStrArr = objQuestion.questionAnswer.indexOf(";") !== -1
            ? objQuestion.questionAnswer.trim().split(";").map(str => str.trim())
            : objQuestion.questionAnswer.trim();
        
        if (Array.isArray(checkingStrArr)) {
            let isExitWhiteSpace = checkingStrArr.some(checkingStr => !checkingStr);
            let result = isExitWhiteSpace ? false : checkingStrArr;
            if (!result)
                arrErrorMsg.push(`請不要留空於分號之間。`);
        }
    }

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join("\n");
    else
        return true;
}

const FAILED = "FAILED";
const NULL = "NULL";
var GetQuestionnaire = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_QUESTIONNAIRE",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strErrorMsg) {
            if (strErrorMsg === FAILED)
                alert("發生錯誤，請再嘗試");
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var CreateQuestionnaire = function (objQuestionnaire) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=CREATE_QUESTIONNAIRE",
        method: "POST",
        data: objQuestionnaire,
        success: function () {
            let objQuestion = GetQuestionInputs();
            let isValidQuestion = CheckQuestionInputs(objQuestion);
            if (typeof isValidQuestion === "string") {
                alert(isValidQuestion);
                return;
            }

            CreateQuestion(objQuestion);
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var UpdateQuestionnaire = function (objQuestionnaire) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=UPDATE_QUESTIONNAIRE",
        method: "POST",
        data: objQuestionnaire,
        success: function (strErrorMsg) {
            if (strErrorMsg === FAILED)
                alert("發生錯誤，請再嘗試。");
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}

var ResetQuestionInputs = function () {
    $("select[id*=ddlCategoryList]").val("自訂問題").change();
    $("select[id*=ddlTypingList]").val("單選方塊").change();
    $("input[id*=txtQuestionName]").val("");
    $("input[id*=txtQuestionAnswer]").val("");
    $("input[id*=ckbQuestionRequired]").prop("checked", false);
}
var CreateQuestionListTable = function (objArrQuestion) {
    $("#divGvQuestionListContainer").append(
        `
            <table class="table table-bordered w-auto">
                <thead>
                    <tr>
                        <th scope="col">
                        </th>
                        <th scope="col">
                            #
                        </th>
                        <th scope="col">
                            問題
                        </th>
                        <th scope="col">
                            種類
                        </th>
                        <th scope="col">
                            必填
                        </th>
                        <th scope="col">
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        `
    );

    for (let i = 0; i < objArrQuestion.length; i++) {
        $("#divGvQuestionListContainer table tbody").append(
            `
                <tr>
                    <td>
                        <input id="${objArrQuestion[i].QuestionID}" type="checkbox">
                    </td>
                    <td>
                        ${i + 1}
                    </td>
                    <td>
                        ${objArrQuestion[i].QuestionName}
                    </td>
                    <td>
                        ${objArrQuestion[i].QuestionTyping}
                    </td>
                    <td>
                        ${objArrQuestion[i].QuestionRequired}
                    </td>
                    <td>
                        <a
                            id="aLinkEditQuestion-${objArrQuestion[i].QuestionID}"
                            href="QuestionnaireDetail.aspx?QuestionID=${objArrQuestion[i].QuestionID}"
                        >
                            編輯
                        </a>
                    </td>
                </tr>
            `
        );
    }
}
var SetQuestionListTableSession = function () {
    let strHtml = $("#divGvQuestionListContainer").html();
    sessionStorage.setItem("currentQuestionListTable", strHtml);
}
var GetQuestionList = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_QUESTIONLIST",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrQuestion) {
            $("#divGvQuestionListContainer").empty();

            if (strOrObjArrQuestion === FAILED) 
                alert("發生錯誤，請刷新重試");
            else if (strOrObjArrQuestion === NULL)
                $("#divGvQuestionListContainer").append("<p>尚未有資料</p>");
            else {
                CreateQuestionListTable(strOrObjArrQuestion);
                SetQuestionListTableSession();
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var CreateQuestion = function (objQuestion) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=CREATE_QUESTION",
        method: "POST",
        data: objQuestion,
        success: function (objArrQuestion) {
            ResetQuestionInputs();
            $("#divGvQuestionListContainer").empty();

            CreateQuestionListTable(objArrQuestion);
            SetQuestionListTableSession();
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var DeleteQuestionList = function (strQuestionIDList) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=DELETE_QUESTIONLIST",
        method: "POST",
        data: { "checkedQuestionIDList": strQuestionIDList, },
        success: function (strOrObjArrQuestion) {
            if (strOrObjArrQuestion === NULL) 
                alert("沒有可以刪除的問題");
            else if (strOrObjArrQuestion === FAILED) 
                alert("請選擇要刪除的問題");
            else {
                $("#divGvQuestionListContainer").empty();

                if (strOrObjArrQuestion.length === 0) 
                    $("#divGvQuestionListContainer").append("<p>尚未有資料</p>");
                else {
                    if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                        let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetQuestionListTableSession();
                        return;
                    }

                    CreateQuestionListTable(strOrObjArrQuestion);
                    SetQuestionListTableSession();
                }
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var ShowToUpdateQuestion = function (strQuestionID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=SHOW_TO_UPDATE_QUESTION",
        method: "POST",
        data: { "clickedQuestionID": strQuestionID },
        success: function (objQuestion) {
            if (objQuestion === FAILED || objQuestion === NULL) 
                alert("發生錯誤，請再嘗試。");
            else {
                $("select[id*=ddlCategoryList]").val(objQuestion.QuestionCategory).change();
                $("select[id*=ddlTypingList]").val(objQuestion.QuestionTyping).change();
                $("input[id*=txtQuestionName]").val(objQuestion.QuestionName);
                $("input[id*=txtQuestionAnswer]").val(objQuestion.QuestionAnswer);
                $("input[id*=ckbQuestionRequired]").prop("checked", objQuestion.QuestionRequired);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });

}
var UpdateQuestion = function (objQuestion) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=UPDATE_QUESTION",
        method: "POST",
        data: objQuestion,
        success: function (objArrQuestion) {
            if (objQuestion === FAILED) 
                alert("發生錯誤，請再嘗試。");
            else {
                $("#divGvQuestionListContainer").empty();
                ResetQuestionInputs();

                CreateQuestionListTable(objArrQuestion);
                SetQuestionListTableSession();
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}