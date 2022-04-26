var SetContainerSession = function (strSelector, strSessionName) {
    let strHtml = $(strSelector).html();
    sessionStorage.setItem(strSessionName, strHtml);
}

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
    let strCategoryName = $("select[id*=ddlCategoryList]").find(":selected").text();
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
    $("select[id*=ddlCategoryList] option").filter(function () {
        return $(this).text() == "自訂問題";
    }).prop('selected', true);
    $("select[id*=ddlTypingList]").val("單選方塊").change();
    $("input[id*=txtQuestionName]").val("");
    $("input[id*=txtQuestionAnswer]").val("");
    $("input[id*=ckbQuestionRequired]").prop("checked", false);
}
var CreateQuestionListTable = function (objArrQuestion) {
    $(divQuestionListContainer).append(
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
        $(divQuestionListContainer + " table tbody").append(
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
var GetQuestionList = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_QUESTIONLIST",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrQuestion) {
            $(divQuestionListContainer).empty();

            if (strOrObjArrQuestion === FAILED) 
                alert("發生錯誤，請刷新重試");
            else if (strOrObjArrQuestion === NULL)
                $(divQuestionListContainer).append("<p>尚未有資料</p>");
            else {
                CreateQuestionListTable(strOrObjArrQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
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
            $(divQuestionListContainer).empty();

            CreateQuestionListTable(objArrQuestion);
            SetContainerSession(divQuestionListContainer, currentQuestionListTable);
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
                $(divQuestionListContainer).empty();

                if (strOrObjArrQuestion.length === 0) 
                    $(divQuestionListContainer).append("<p>尚未有資料</p>");
                else {
                    if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                        let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                        return;
                    }

                    CreateQuestionListTable(strOrObjArrQuestion);
                    SetContainerSession(divQuestionListContainer, currentQuestionListTable);
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
                $("select[id*=ddlCategoryList] option").filter(function () {
                    return $(this).text() == objQuestion.QuestionCategory;
                }).prop('selected', true);
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
                $(divQuestionListContainer).empty();
                ResetQuestionInputs();

                CreateQuestionListTable(objArrQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var SetQuestionListOfCommonQuestionOnQuestionnaire = function (strSelectedCategoryID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=SET_QUESTIONLIST_OF_COMMONQUESTION_ON_QUESTIONNAIRE",
        method: "POST",
        data: { "selectedCategoryID": strSelectedCategoryID },
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            if (strOrObjArrQuestionOfCommonQuestion === FAILED)
                alert("發生錯誤，請再嘗試。");
            else {
                ResetQuestionInputs();
                $(divQuestionListContainer).empty();

                CreateQuestionListTable(strOrObjArrQuestionOfCommonQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}

var CreateUserListTable = function (objArrUserModel) {
    $(divUserListContainer).append(
        `
            <table class="table table-bordered w-auto">
                <thead>
                    <tr>
                        <th scope="col">
                            #
                        </th>
                        <th scope="col">
                            姓名
                        </th>
                        <th scope="col">
                            填寫時間
                        </th>
                        <th scope="col">
                            觀看細節
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        `
    );

    for (let i = 0; i < objArrUserModel.length; i++) {
        $(divUserListContainer + " table tbody").append(
            `
                <tr>
                    <td>
                        ${objArrUserModel.length - i}
                    </td>
                    <td>
                        ${objArrUserModel[i].UserName}
                    </td>
                    <td>
                        ${objArrUserModel[i].AnswerDate}
                    </td>
                    <td>
                        <a
                            id="aLinkUserAnswer-${objArrUserModel[i].UserID}"
                            href="QuestionnaireDetail.aspx?UserID=${objArrUserModel[i].UserID}"
                        >
                            前往
                        </a>
                    </td>
                </tr>
            `
        );
    }
}
var SetUserListShowStateSession = function (strShowState) {
    sessionStorage.setItem(currentUserListShowState, strShowState);
}
var CreateUserListPager = function (intPageSize) {
    $(divUserListPagerContainer).append(
        `
            <a id="aLinkUserListPager-First"class="text-decoration-none"  href="QuestionnaireDetail.aspx?Index=First">
                首頁
            </a>
            <a id="aLinkUserListPager-Prev" class="text-decoration-none" href="QuestionnaireDetail.aspx?Index=Prev">
                上一頁
            </a>
        `
    );

    for (let i = 1; i <= intPageSize; i++) {
        $(divUserListPagerContainer).append(
            `
                <a id="aLinkUserListPager-${i}" class="text-decoration-none" href="QuestionnaireDetail.aspx?Index=${i}">
                    ${i}
                </a>
            `
        );
    }


    $(divUserListPagerContainer).append(
        `
            <a id="aLinkUserListPager-Next" class="text-decoration-none" href="QuestionnaireDetail.aspx?Index=Next">
                下一頁
            </a>
            <a id="aLinkUserListPager-Last" class="text-decoration-none" href="QuestionnaireDetail.aspx?Index=Last">
                末頁
            </a>
        `
    );
}
var GetUserList = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_USERLIST",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrUserModel) {
            if (strOrObjArrUserModel === FAILED)
                alert("發生錯誤，請再嘗試。");
            else if (strOrObjArrUserModel === NULL)
                $(divUserListContainer).html(emptyMessageOfUserListOrStatistics);
            else {
                let [objArrUserModel, totalRows] = strOrObjArrUserModel;

                $(divUserListContainer).empty();
                CreateUserListTable(objArrUserModel);
                SetContainerSession(divUserListContainer, currentUserList);
                SetUserListShowStateSession(showState);
                SetUserAnswerShowStateSession(hideState);

                $(divUserListPagerContainer).empty();
                let pageSize = 1;
                if (totalRows < PAGESIZE)
                    pageSize = 1;
                else if ((totalRows % PAGESIZE) == 0)
                    pageSize = totalRows / PAGESIZE;
                else
                    pageSize = totalRows / PAGESIZE + 1;
                CreateUserListPager(pageSize);
                SetContainerSession(divUserListPagerContainer, currentUserListPager);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
var UpdateUserList = function (objQuestionnaireIDAndIndex) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=UPDATE_USERLIST",
        method: "POST",
        data: objQuestionnaireIDAndIndex,
        success: function (strOrObjArrUserModel) {
            if (strOrObjArrUserModel === FAILED)
                alert("發生錯誤，請再嘗試。");
            else {
                let [objArrUserModel, totalRows] = strOrObjArrUserModel;

                $(divUserListContainer).empty();
                CreateUserListTable(objArrUserModel);
                SetContainerSession(divUserListContainer, currentUserList);
                SetUserListShowStateSession(showState);
                SetUserAnswerShowStateSession(hideState);

                $(divUserListPagerContainer).empty();
                let pageSize = 1;
                if (totalRows < PAGESIZE)
                    pageSize = 1;
                else if ((totalRows % PAGESIZE) == 0)
                    pageSize = totalRows / PAGESIZE;
                else
                    pageSize = totalRows / PAGESIZE + 1;
                CreateUserListPager(pageSize);
                SetContainerSession(divUserListPagerContainer, currentUserListPager);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}

var CreateUserDetail = function (objUserModel) {
    let userName = objUserModel.UserName;
    let phone = objUserModel.Phone;
    let email = objUserModel.Email;
    let age = objUserModel.Age;
    let answerDate = objUserModel.AnswerDate;

    $(divUserAnswerContainer).append(
        `
            <div class="row align-items-center justify-content-center gy-3">
                <div class="col-md-10">
        `
    );

    $(divUserAnswerContainer).append(
        `
            <div class="row align-items-center justify-content-center">
                <div class="col-md-6">
                    <div class="row mb-3">
                        <label for="txtUserName" class="col-sm-2 col-form-label">姓名</label>
                        <div class="col-sm-10">
                            <input id="txtUserName" class="form-control" value="${userName}" disabled />
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="row mb-3">
                        <label for="txtUserPhone" class="col-sm-2 col-form-label">手機</label>
                        <div class="col-sm-10">
                            <input id="txtUserPhone" class="form-control" value="${phone}" disabled />
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="row mb-3">
                        <label for="txtUserEmail" class="col-sm-2 col-form-label">Email</label>
                        <div class="col-sm-10">
                            <input id="txtUserEmail" class="form-control" value="${email}" disabled />
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="row mb-3">
                        <label for="txtUserAge" class="col-sm-2 col-form-label">年齡</label>
                        <div class="col-sm-10">
                            <input id="txtUserAge" class="form-control" value="${age}" disabled />
                        </div>
                    </div>
                </div>

                <div class="col-12">
                    <div class="d-flex align-items-center justify-content-end">
                        填寫時間 ${answerDate}
                    </div>
                </div>
            </div>

            </div>
        `
    );
    // 此末</div>為開頭<div class="col-md-10">的結束標籤
}
var CreateUserAnswerDetail = function (objArrQuestionModel, objArrUserAnswerModel) {
    $(divUserAnswerContainer).append(
        `
            <div class="col-md-10">
                <div class="row align-items-center justify-content-center gy-3">
        `
    );

    let i = 1;
    for (let question of objArrQuestionModel) {
        let questionID = question.QuestionID;
        let questionName = question.QuestionName;
        let questionRequired = question.QuestionRequired;
        let questionTyping = question.QuestionTyping;
        let arrQuestionAnswer = question.QuestionAnswer.split(";");

        let arrQuestionItsUserAnswer = objArrUserAnswerModel
            .filter(item => item.QuestionID === questionID);
        let arrUserAnswerNum = [];
        if (arrQuestionItsUserAnswer.length) 
            arrUserAnswerNum = arrQuestionItsUserAnswer.map(item2 => item2.AnswerNum);
        else 
            arrUserAnswerNum.push(-1);
        
        $(divUserAnswerContainer).append(
            `
                <div class="col-12">
                    <div class="d-flex flex-column">
                        <h3>
                            ${i}. ${questionName} ${questionRequired ? "(必填)" : ""}
                        </h3>
            `
        );

        for (let j = 0; j < arrQuestionAnswer.length; j++) {
            let anothorJ = j;
            let jPlus1 = anothorJ + 1;

            if (questionTyping === "單選方塊") {
                $(divUserAnswerContainer).append(
                    `
                        <div class="form-check">
                            <input id="rdoQuestionAnswer_${questionID}_${jPlus1}" class="form-check-input" type="radio" ${arrUserAnswerNum.indexOf(jPlus1) !== -1 ? "checked" : null} disabled />
                            <label class="form-check-label" for="rdoQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}
                            </label>
                        </div>
                    `
                );
            }

            if (questionTyping == "複選方塊") {
                $(divUserAnswerContainer).append(
                    `
                        <div class="form-check">
                            <input id="ckbQuestionAnswer_${questionID}_${jPlus1}" class="form-check-input" type="checkbox" ${arrUserAnswerNum.indexOf(jPlus1) !== -1 ? "checked" : null} disabled />
                            <label class="form-check-label" for="ckbQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}
                            </label>
                        </div>
                    `
                );
            }

            if (questionTyping == "文字") {
                let isExitValue = arrUserAnswerNum.indexOf(jPlus1) === -1
                    ? false
                    : arrQuestionItsUserAnswer.filter(item => item.AnswerNum === jPlus1)[0].Answer;

                $(divUserAnswerContainer).append(
                    `
                        <div class="row">
                            <label class="col-sm-2 col-form-label" for="txtQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}
                            </label>
                            <div class="col-sm-10">
                                <input id="txtQuestionAnswer_${questionID}_${jPlus1}" class="form-control" type="text" value="${isExitValue === false ? "" : isExitValue}" disabled />
                            </div>
                        </div>
                    `
                );
            }
        }

        $(divUserAnswerContainer).append(
            `
                    </div>
                </div>
            `
        );

        i++;
    }

    $(divUserAnswerContainer).append(
        `
                    </div>
                </div>

                <div class="col-12">
                    <div class="d-flex align-items-center justify-content-end">
                        <button id="btnBackToUserList" class="btn btn-success">返回</button>
                    </div>
                </div>
            </div>
        `
    );
    // 此末</div>為CreateUserDetail函式開頭的
    // <div class="row align-items-center justify-content-center">之結尾標籤
}
var SetUserAnswerShowStateSession = function (strShowState) {
    sessionStorage.setItem(currentUserAnswerShowState, strShowState);
}
var GetUserAnswer = function (objQuestionnaireAndUserID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_USERANSWER",
        method: "POST",
        data: objQuestionnaireAndUserID,
        success: function (strOrObjArrUserAnswerDetail) {
            if (strOrObjArrUserAnswerDetail === FAILED)
                alert("發生錯誤，請再嘗試。");
            else {
                let [objUserModel, objArrQuestionModel, objArrUserAnswerModel] =
                    strOrObjArrUserAnswerDetail;

                $(btnExportAndDownloadDataToCSV).hide();
                $(divUserListContainer).empty();
                $(divUserListPagerContainer).empty();
                $(divUserAnswerContainer).empty();

                CreateUserDetail(objUserModel);
                CreateUserAnswerDetail(objArrQuestionModel, objArrUserAnswerModel);
                SetContainerSession(divUserAnswerContainer, currentUserAnswer);
                SetUserAnswerShowStateSession(showState);
                SetUserListShowStateSession(hideState);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}

var CreateStatistics = function (objArrQuestionModel, objArrUserAnswerModel) {
    $(divStatisticsContainer).append(
        `
            <div class="row align-items-center justify-content-center gy-3">
                <div class="col-md-10">
                    <div class="row align-items-center justify-content-center gy-3">
        `
    );

    let i = 1;
    for (let question of objArrQuestionModel) {
        let questionID = question.QuestionID;
        let questionName = question.QuestionName;
        let questionRequired = question.QuestionRequired;
        let questionTyping = question.QuestionTyping;
        let arrQuestionAnswer = question.QuestionAnswer.split(";");

        let arrQuestionItsUserAnswer = objArrUserAnswerModel
            .filter(item => item.QuestionID === questionID);

        let arrUserAnswerNum = [];
        if (arrQuestionItsUserAnswer.length) 
            arrUserAnswerNum = arrQuestionItsUserAnswer.map(item2 => item2.AnswerNum);
        else 
            arrUserAnswerNum.push(-1);
        
        let arrEachUserAnswerNum = [];
        if (arrUserAnswerNum.indexOf(-1) === -1) {
            arrEachUserAnswerNum = arrUserAnswerNum.reduce((acc, val) => {
                    acc[val] = acc[val] == null ? 1 : acc[val] += 1;
                    return acc;
                }, []);
        }
        else 
            arrEachUserAnswerNum = new Array(arrQuestionAnswer.length + 1).fill(null);

        $(divStatisticsContainer).append(
            `
                <div class="col-12">
                    <div class="d-flex flex-column">
                        <h3>
                            ${i}. ${questionName} ${questionRequired ? "(必填)" : ""}
                        </h3>
            `
        );

        for (let j = 0; j < arrQuestionAnswer.length; j++) {
            let anothorJ = j;
            let jPlus1 = anothorJ + 1;

            let prepareEachUserAnswerPercentage1 =
                (arrEachUserAnswerNum[jPlus1] / arrUserAnswerNum.length) * 100;
            let prepareEachUserAnswerPercentage2 = prepareEachUserAnswerPercentage1.toFixed();
            let eachUserAnswerPercentage =
                arrEachUserAnswerNum[jPlus1] == null
                    ? "0%"
                    : prepareEachUserAnswerPercentage2 + "%";
            let eachUserAnswerTotal =
                arrEachUserAnswerNum[jPlus1] == null
                    ? 0
                    : arrEachUserAnswerNum[jPlus1];

            if (questionTyping === "單選方塊") {
                $(divStatisticsContainer).append(
                    `
                        <div class="form-check">
                            <label class="form-check-label" for="rdoQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}
                                ${eachUserAnswerPercentage}
                                (${eachUserAnswerTotal})
                            </label>
                        </div>
                    `
                );
            }

            if (questionTyping == "複選方塊") {
                $(divStatisticsContainer).append(
                    `
                        <div class="form-check">
                            <label class="form-check-label" for="ckbQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}
                                ${eachUserAnswerPercentage}
                                (${eachUserAnswerTotal})
                            </label>
                        </div>
                    `
                );
            }

            if (questionTyping == "文字") {
                $(divStatisticsContainer).append(
                    `
                        <div class="form-check">
                            <label class="form-check-label" for="txtQuestionAnswer_${questionID}_${jPlus1}">
                                ${arrQuestionAnswer[j]}  -
                            </label>
                        </div>
                    `
                );
            }
        }

        $(divStatisticsContainer).append(
            `
                    </div>
                </div>
            `
        );

        i++;
    }

    $(divStatisticsContainer).append(
        `
                    </div>
                </div>
            </div>
        `
    );
}
var GetStatistics = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_STATISTICS",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrStatistics) {
            if (strOrObjArrStatistics === FAILED)
                alert("發生錯誤，請再嘗試。");
            else if (strOrObjArrStatistics === NULL)
                $(divStatisticsContainer).html(emptyMessageOfUserListOrStatistics);
            else {
                let [objArrQuestionModel, objArrUserAnswerModel] = strOrObjArrStatistics;
                
                $(divStatisticsContainer).empty();

                CreateStatistics(objArrQuestionModel, objArrUserAnswerModel);
                SetContainerSession(divStatisticsContainer, currentStatistics);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}