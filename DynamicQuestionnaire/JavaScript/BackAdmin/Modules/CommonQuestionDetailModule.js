var GetCommonQuestionInputs = function () {
    let strCommonQuestionName = $("input[id*=txtCommonQuestionName]").val();

    let objCommonQuestion = {
        "commonQuestionName": strCommonQuestionName,
    };

    return objCommonQuestion;
}
var GetQuestionOfCommonQuestionInputs = function () {
    let strQuestionNameOfCommonQuestion = $("input[id*=txtQuestionNameOfCommonQuestion]").val();
    let strQuestionAnswerOfCommonQuestion = $("input[id*=txtQuestionAnswerOfCommonQuestion]").val();
    let strCategoryName = $("select[id*=ddlCategoryList]").val();
    let strTypingName = $("select[id*=ddlTypingList]").val();
    let boolQuestionRequiredOfCommonQuestion =
        $("input[id*=ckbQuestionRequiredOfCommonQuestion]").is(":checked");

    let objQuestionOfCommonQuestion = {
        "questionName": strQuestionNameOfCommonQuestion,
        "questionAnswer": strQuestionAnswerOfCommonQuestion,
        "questionCategory": strCategoryName,
        "questionTyping": strTypingName,
        "questionRequired": boolQuestionRequiredOfCommonQuestion,
    };

    return objQuestionOfCommonQuestion;
}
var CheckCommonQuestionInputs = function (objCommonQuestion) {
    let arrErrorMsg = [];

    if (!objCommonQuestion.commonQuestionName)
        arrErrorMsg.push("請填入常用問題名稱。");

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join();
    else
        return true;
}
var CheckQuestionOfCommonQuestionInputs = function (objQuestionOfCommonQuestion) {
    let arrErrorMsg = [];

    if (!objQuestionOfCommonQuestion.questionName)
        arrErrorMsg.push("請填入問題名稱。");

    if (!objQuestionOfCommonQuestion.questionAnswer)
        arrErrorMsg.push("請填入問題回答。");
    else {
        let checkingStrArr = objQuestionOfCommonQuestion.questionAnswer.indexOf(";") !== -1
            ? objQuestionOfCommonQuestion.questionAnswer.trim().split(";").map(str => str.trim())
            : objQuestionOfCommonQuestion.questionAnswer.trim();

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

var GetCommonQuestion = function (strCommonQuestionID) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=GET_COMMONQUESTION",
        method: "POST",
        data: { "commonQuestionID": strCommonQuestionID },
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
var CreateCommonQuestion = function (objCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=CREATE_COMMONQUESTION",
        method: "POST",
        data: objCommonQuestion,
        success: function () {
            let objQuestionOfCommonQuestion = GetQuestionOfCommonQuestionInputs();
            let isValidQuestionOfCommonQuestion =
                CheckQuestionOfCommonQuestionInputs(objQuestionOfCommonQuestion);
            if (typeof isValidQuestionOfCommonQuestion === "string") {
                alert(isValidQuestionOfCommonQuestion);
                return;
            }

            CreateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var UpdateCommonQuestion = function (objCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=UPDATE_COMMONQUESTION",
        method: "POST",
        data: objCommonQuestion,
        success: function (notUpdatedOrUpdatedObjCommonQuestion) {
            $("input[id*=txtCommonQuestionName]")
                .val(notUpdatedOrUpdatedObjCommonQuestion.CommonQuestionName);
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}

var ResetQuestionOfCommonQuestionInputs = function () {
    $("select[id*=ddlCategoryList]").val("常用問題").change();
    $("select[id*=ddlTypingList]").val("單選方塊").change();
    $("input[id*=txtQuestionNameOfCommonQuestion]").val("");
    $("input[id*=txtQuestionAnswerOfCommonQuestion]").val("");
    $("input[id*=ckbQuestionRequiredOfCommonQuestion]").prop("checked", false);
}
var CreateQuestionListOfCommonQuestionTable = function (objArrQuestionOfCommonQuestion) {
    $("#divQuestionListOfCommonQuestionContainer").append(
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

    for (let i = 0; i < objArrQuestionOfCommonQuestion.length; i++) {
        $("#divQuestionListOfCommonQuestionContainer table tbody").append(
            `
                <tr>
                    <td>
                        <input id="${objArrQuestionOfCommonQuestion[i].QuestionID}" type="checkbox">
                    </td>
                    <td>
                        ${i + 1}
                    </td>
                    <td>
                        ${objArrQuestionOfCommonQuestion[i].QuestionName}
                    </td>
                    <td>
                        ${objArrQuestionOfCommonQuestion[i].QuestionTyping}
                    </td>
                    <td>
                        ${objArrQuestionOfCommonQuestion[i].QuestionRequired}
                    </td>
                    <td>
                        <a
                            id="aLinkEditQuestionOfCommonQuestion-${objArrQuestionOfCommonQuestion[i].QuestionID}"
                            href="CommonQuestionDetail.aspx?QuestionIDOfCommonQuestion=${objArrQuestionOfCommonQuestion[i].QuestionID}"
                        >
                            編輯
                        </a>
                    </td>
                </tr>
            `
        );
    }
}
var SetQuestionListOfCommonQuestionTableSession = function () {
    let strHtml = $("#divQuestionListOfCommonQuestionContainer").html();
    sessionStorage.setItem("currentQuestionListOfCommonQuestionTable", strHtml);
}
var GetQuestionListOfCommonQuestion = function (strCommonQuestionID) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=GET_QUESTIONLIST_OF_COMMONQUESTION",
        method: "POST",
        data: { "commonQuestionID": strCommonQuestionID },
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            $("#divQuestionListOfCommonQuestionContainer").empty();

            if (strOrObjArrQuestionOfCommonQuestion === FAILED)
                alert("發生錯誤，請刷新重試");
            else if (strOrObjArrQuestionOfCommonQuestion === NULL)
                $("#divQuestionListOfCommonQuestionContainer").append("<p>尚未有資料</p>");
            else {
                CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                SetQuestionListOfCommonQuestionTableSession();
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var CreateQuestionOfCommonQuestion = function (objQuestionOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=CREATE_QUESTION_OF_COMMONQUESTION",
        method: "POST",
        data: objQuestionOfCommonQuestion,
        success: function (objArrQuestionOfCommonQuestion) {
            ResetQuestionOfCommonQuestionInputs();
            $("#divQuestionListOfCommonQuestionContainer").empty();

            CreateQuestionListOfCommonQuestionTable(objArrQuestionOfCommonQuestion);
            SetQuestionListOfCommonQuestionTableSession();
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var DeleteQuestionListOfCommonQuestion = function (strQuestionIDListOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=DELETE_QUESTIONLIST_OF_COMMONQUESTION",
        method: "POST",
        data: { "checkedQuestionIDListOfCommonQuestion": strQuestionIDListOfCommonQuestion, },
        success: function (strOrObjArrQuestionOfCommonQuestionAndIsUpdateMode) {
            let [strOrObjArrQuestionOfCommonQuestion, isUpdateMode] = strOrObjArrQuestionOfCommonQuestionAndIsUpdateMode;

            if (strOrObjArrQuestionOfCommonQuestion === NULL)
                alert("沒有可以刪除的問題");
            else if (strOrObjArrQuestionOfCommonQuestion === FAILED)
                alert("請選擇要刪除的問題");
            else {
                $("#divQuestionListOfCommonQuestionContainer").empty();

                if (isUpdateMode) {
                    if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                        let filteredObjArrQuestionOfCommonQuestion =
                            strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                        if (filteredObjArrQuestionOfCommonQuestion.length === 0) {
                            $("#divQuestionListOfCommonQuestionContainer").append("<p>尚未有資料</p>");
                            sessionStorage.setItem("currentQuestionListOfCommonQuestionTable", "");
                        }
                        else {
                            CreateQuestionListOfCommonQuestionTable(filteredObjArrQuestionOfCommonQuestion);
                            SetQuestionListOfCommonQuestionTableSession();
                        }
                    }

                    return false;
                }

                if (strOrObjArrQuestionOfCommonQuestion.length === 0) {
                    $("#divQuestionListOfCommonQuestionContainer").append("<p>尚未有資料</p>");
                    sessionStorage.setItem("currentQuestionListOfCommonQuestionTable", "");
                }
                else {
                    CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                    SetQuestionListOfCommonQuestionTableSession();
                }
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var ShowToUpdateQuestionOfCommonQuestion = function (strQuestionIDOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=SHOW_TO_UPDATE_QUESTION_OF_COMMONQUESTION",
        method: "POST",
        data: { "clickedQuestionIDOfCommonQuestion": strQuestionIDOfCommonQuestion },
        success: function (strOrObjQuestionOfCommonQuestion) {
            if (strOrObjQuestionOfCommonQuestion === FAILED
                || strOrObjQuestionOfCommonQuestion === NULL)
                alert("發生錯誤，請再嘗試。");
            else {
                $("select[id*=ddlCategoryList]")
                    .val(strOrObjQuestionOfCommonQuestion.QuestionCategory)
                    .change();
                $("select[id*=ddlTypingList]")
                    .val(strOrObjQuestionOfCommonQuestion.QuestionTyping)
                    .change();
                $("input[id*=txtQuestionNameOfCommonQuestion]")
                    .val(strOrObjQuestionOfCommonQuestion.QuestionName);
                $("input[id*=txtQuestionAnswerOfCommonQuestion]")
                    .val(strOrObjQuestionOfCommonQuestion.QuestionAnswer);
                $("input[id*=ckbQuestionRequiredOfCommonQuestion]")
                    .prop("checked", strOrObjQuestionOfCommonQuestion.QuestionRequired);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });

}
var UpdateQuestionOfCommonQuestion = function (objQuestionOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=UPDATE_QUESTION_OF_COMMONQUESTION",
        method: "POST",
        data: objQuestionOfCommonQuestion,
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            if (strOrObjArrQuestionOfCommonQuestion === FAILED)
                alert("發生錯誤，請再嘗試。");
            else {
                $("#divQuestionListOfCommonQuestionContainer").empty();
                ResetQuestionOfCommonQuestionInputs(strOrObjArrQuestionOfCommonQuestion);

                CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                SetQuestionListOfCommonQuestionTableSession();
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}