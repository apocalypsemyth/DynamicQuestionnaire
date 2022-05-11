var SetContainerSession = function (strSelector, strSessionName) {
    let strHtml = $(strSelector).html();
    sessionStorage.setItem(strSessionName, strHtml);
}
var SetContainerShowStateSession = function (strSessionName, strShowState) {
    sessionStorage.setItem(strSessionName, strShowState);
}
var SetElementCurrentStateSession = function (strSessionName, strCurrentState) {
    sessionStorage.setItem(strSessionName, strCurrentState);
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

    let isUpdateMode = window.location.search.indexOf("?ID=") !== -1;
    let today = new Date().toDateString();
    let todayMillisecond = Date.parse(today);

    if (!objQuestionnaire.startDate)
        arrErrorMsg.push("請填入開始時間。");
    else {
        if (!regex.test(objQuestionnaire.startDate))
            arrErrorMsg.push(`請以 "yyyy/MM/dd" 的格式輸入開始時間。`);
        else {
            let startDate = new Date(objQuestionnaire.startDate);
            let startDateMillisecond = Date.parse(startDate);

            if (objQuestionnaire.isEnable && startDateMillisecond > todayMillisecond)
                arrErrorMsg.push("系統會屆時開放此問卷，請取消已啟用的勾選");
            else if (!isUpdateMode && startDateMillisecond < todayMillisecond)
                arrErrorMsg.push("請填入今天或其後的開始時間。");
        }
    }

    if (objQuestionnaire.endDate) {
        if (!regex.test(objQuestionnaire.endDate))
            arrErrorMsg.push(`請以 "yyyy/MM/dd" 的格式輸入結束時間。`);
        else {
            let startDate = new Date(objQuestionnaire.startDate);
            let startDateMillisecond = Date.parse(startDate);
            let endDate = new Date(objQuestionnaire.endDate);
            let endDateMillisecond = Date.parse(endDate);

            if (objQuestionnaire.isEnable && endDateMillisecond < todayMillisecond)
                arrErrorMsg.push("若要開放此問卷，請填入今天或其後的結束時間。");
            else if (!isUpdateMode && endDateMillisecond < todayMillisecond)
                arrErrorMsg.push("請填入今天或其後的結束時間。");
            else if (startDateMillisecond > endDateMillisecond)
                arrErrorMsg.push("請填入一前一後時序的始末日期。");
        }
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
                alert(errorMessageOfRetry);
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
            CreateQuestion(objQuestion);
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
                alert(errorMessageOfRetry);
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}

var ResetQuestionInputs = function (strCategoryName) {
    $("select[id*=ddlCategoryList] option").filter(function () {
        return $(this).text() == strCategoryName;
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
            $(btnDeleteQuestion).hide();
            $(divQuestionListContainer).empty();

            if (strOrObjArrQuestion === NULL + FAILED) {
                window.location.replace(document.referrer);
            }
            else if (strOrObjArrQuestion === FAILED) {
                alert(errorMessageOfRetry);
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
            else if (strOrObjArrQuestion === NULL) {
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
            else if (strOrObjArrQuestion.length === 0) {
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
            else {
                $(btnDeleteQuestion).show();

                if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestion.length === 0) {
                        $(btnDeleteQuestion).hide();
                        $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    else {
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    return;
                }
                CreateQuestionListTable(strOrObjArrQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var CreateQuestion = function (objQuestion) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=CREATE_QUESTION",
        method: "POST",
        data: objQuestion,
        success: function (strOrObjArrQuestion) {
            if (strOrObjArrQuestion === FAILED)
                alert(errorMessageOfRetry);
            else {
                let strCurrentSetCommonQuestionOnQuestionnaireState =
                    sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
                if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) 
                    ResetQuestionInputs(commonQuestionOfCategoryName);
                else 
                    ResetQuestionInputs(customizedQuestionOfCategoryName);

                $(btnDeleteQuestion).show();
                $(divQuestionListContainer).empty();

                if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestion.length === 0) {
                        $(btnDeleteQuestion).hide();
                        $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    else {
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    return;
                }
                CreateQuestionListTable(strOrObjArrQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var DeleteQuestionList = function (strQuestionIDList) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=DELETE_QUESTIONLIST",
        method: "POST",
        data: { "checkedQuestionIDList": strQuestionIDList, },
        success: function (strOrObjArrQuestion) {
            if (strOrObjArrQuestion === NULL + FAILED)
                alert(errorMessageOfRetry);
            else {
                let strCurrentSetCommonQuestionOnQuestionnaireState =
                    sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
                if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) 
                    ResetQuestionInputs(commonQuestionOfCategoryName);
                else 
                    ResetQuestionInputs(customizedQuestionOfCategoryName);

                $(divQuestionListContainer).empty();

                if (strOrObjArrQuestion.length === 0) {
                    $(btnDeleteQuestion).hide();
                    $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                    SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                }
                else {
                    $(btnDeleteQuestion).show();

                    if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                        let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);

                        if (filteredObjArrQuestion.length === 0) {
                            $(btnDeleteQuestion).hide();
                            $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                            SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                        }
                        else {
                            CreateQuestionListTable(filteredObjArrQuestion);
                            SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                        }
                        return;
                    }
                    CreateQuestionListTable(strOrObjArrQuestion);
                    SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                }
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
                alert(errorMessageOfRetry);
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
            alert(errorMessageOfAjax);
        }
    });

}
var UpdateQuestion = function (objQuestion) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=UPDATE_QUESTION",
        method: "POST",
        data: objQuestion,
        success: function (strOrObjArrQuestion) {
            if (strOrObjArrQuestion === FAILED)
                alert(errorMessageOfRetry);
            else {
                let strCurrentSetCommonQuestionOnQuestionnaireState =
                    sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
                if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) 
                    ResetQuestionInputs(commonQuestionOfCategoryName);
                else 
                    ResetQuestionInputs(customizedQuestionOfCategoryName);

                $(btnAddQuestion).removeAttr("href");
                $(btnDeleteQuestion).show();
                $(divQuestionListContainer).empty();

                if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestion.length === 0) {
                        $(btnDeleteQuestion).hide();
                        $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    else {
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    return;
                }
                CreateQuestionListTable(strOrObjArrQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
                alert(errorMessageOfRetry);
            else {
                ResetQuestionInputs(commonQuestionOfCategoryName);
                SetElementCurrentStateSession(
                    currentSetCommonQuestionOnQuestionnaireState,
                    setState
                );

                $(btnDeleteQuestion).show();
                $(divQuestionListContainer).empty();

                if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestion =
                        strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestion.length === 0) {
                        $(btnDeleteQuestion).hide();
                        $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    else {
                        CreateQuestionListTable(filteredObjArrQuestion);
                        SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    }
                    return;
                }
                CreateQuestionListTable(strOrObjArrQuestionOfCommonQuestion);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var DeleteSetQuestionListOfCommonQuestionOnQuestionnaire = function () {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=DELETE_SET_QUESTIONLIST_OF_COMMONQUESTION_ON_QUESTIONNAIRE",
        method: "POST",
        success: function (strOrObjArrQuestion) {
            ResetQuestionInputs(customizedQuestionOfCategoryName);
            $(btnDeleteQuestion).hide();
            $(divQuestionListContainer).empty();

            if (strOrObjArrQuestion === FAILED) {
                alert(errorMessageOfRetry);
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
            else if (strOrObjArrQuestion === NULL) {
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
            else if (strOrObjArrQuestion.some(item => item.IsDeleted)) {
                let filteredObjArrQuestion = strOrObjArrQuestion.filter(item2 => !item2.IsDeleted);

                if (filteredObjArrQuestion.length === 0) {
                    $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                    SetContainerSession(divQuestionListContainer, currentQuestionListTable);
                    return;
                }
                alert(errorMessageOfRetry);
                $(divQuestionListContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(divQuestionListContainer, currentQuestionListTable);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}

var CreateUserListTable = function (objArrUserModel, intTotalRows, intPagerIndex) {
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
                        ${intTotalRows - ((intPagerIndex - 1) * PAGESIZE) - i}
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
            $(divUserListContainer).show();
            $(divUserListContainer).empty();
            $(divUserListPagerContainer).empty();
            $(divUserListPagerContainer).hide();

            if (strOrObjArrUserModel === FAILED) {
                alert(errorMessageOfRetry);
                $(divUserListContainer).append(emptyMessageOfUserListOrStatistics);
                SetContainerSession(divUserListContainer, currentUserList);
                SetContainerShowStateSession(currentUserListShowState, showState);
                SetContainerShowStateSession(currentUserAnswerShowState, hideState);
            }
            else if (strOrObjArrUserModel === NULL) {
                $(divUserListContainer).append(emptyMessageOfUserListOrStatistics);
                SetContainerSession(divUserListContainer, currentUserList);
                SetContainerShowStateSession(currentUserListShowState, showState);
                SetContainerShowStateSession(currentUserAnswerShowState, hideState);
            }
            else {
                let [objArrUserModel, totalRows, currentPagerIndex] = strOrObjArrUserModel;

                CreateUserListTable(objArrUserModel, totalRows, currentPagerIndex);
                SetContainerSession(divUserListContainer, currentUserList);
                SetContainerShowStateSession(currentUserListShowState, showState);
                SetContainerShowStateSession(currentUserAnswerShowState, hideState);

                $(divUserListPagerContainer).show();
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
            alert(errorMessageOfAjax);
        }
    });
}
var UpdateUserList = function (objQuestionnaireIDAndIndex) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=UPDATE_USERLIST",
        method: "POST",
        data: objQuestionnaireIDAndIndex,
        success: function (strOrObjArrUserModel) {
            $(divUserListContainer).show();
            $(divUserListContainer).empty();
            $(divUserListPagerContainer).empty();
            $(divUserListPagerContainer).hide();

            if (strOrObjArrUserModel === FAILED) {
                alert(errorMessageOfRetry);
                $(divUserListContainer).append(emptyMessageOfUserListOrStatistics);
                SetContainerSession(divUserListContainer, currentUserList);
                SetContainerShowStateSession(currentUserListShowState, showState);
                SetContainerShowStateSession(currentUserAnswerShowState, hideState);
            }
            else {
                let [objArrUserModel, totalRows, currentPagerIndex] = strOrObjArrUserModel;

                CreateUserListTable(objArrUserModel, totalRows, currentPagerIndex);
                SetContainerSession(divUserListContainer, currentUserList);
                SetContainerShowStateSession(currentUserListShowState, showState);
                SetContainerShowStateSession(currentUserAnswerShowState, hideState);

                $(divUserListPagerContainer).show();
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
            alert(errorMessageOfAjax);
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
            <div id="divUserAnswerInnerContainer" class="row gy-3">
                <div class="col-md-10">
                    <div class="row gy-3">
                        <div class="col-md-6">
                            <div class="row">
                                <label for="txtUserName" class="col-sm-2 col-form-label">姓名</label>
                                <div class="col-sm-10">
                                    <input id="txtUserName" class="form-control" value="${userName}" disabled />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row">
                                <label for="txtUserPhone" class="col-sm-2 col-form-label">手機</label>
                                <div class="col-sm-10">
                                    <input id="txtUserPhone" class="form-control" value="${phone}" disabled />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row">
                                <label for="txtUserEmail" class="col-sm-2 col-form-label">Email</label>
                                <div class="col-sm-10">
                                    <input id="txtUserEmail" class="form-control" value="${email}" disabled />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row">
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
            </div>
        `
    );
}
var CreateUserAnswerDetail = function (objArrQuestionModel, objArrUserAnswerModel) {
    $("#divUserAnswerInnerContainer").append(
        `
            <div class="col-md-10">
                <div id="divUserAnswerDetailInnerContainer"
                     class="row gy-3"
                >
                </div>
            </div>
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
        
        $("#divUserAnswerDetailInnerContainer").append(
            `
                <div id="${questionID}" class="col-12">
                    <div class="d-flex flex-column">
                        <h3>
                            ${i}. ${questionName} ${questionRequired ? "(必填)" : ""}
                        </h3>
                    </div>
                </div>
            `
        );

        for (let j = 0; j < arrQuestionAnswer.length; j++) {
            let anothorJ = j;
            let jPlus1 = anothorJ + 1;

            if (questionTyping === "單選方塊") {
                $(`#divUserAnswerDetailInnerContainer #${questionID} div.d-flex.flex-column`).append(
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
                $(`#divUserAnswerDetailInnerContainer #${questionID} div.d-flex.flex-column`).append(
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

                $(`#divUserAnswerDetailInnerContainer #${questionID} div.d-flex.flex-column`).append(
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

        i++;
    }

    $("#divUserAnswerInnerContainer").append(
        `
            <div class="col-10">
                <div class="d-flex align-items-center justify-content-end">
                    <button id="btnBackToUserList" class="btn btn-success">返回</button>
                </div>
            </div>
        `
    );
}
var GetUserAnswer = function (objQuestionnaireAndUserID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_USERANSWER",
        method: "POST",
        data: objQuestionnaireAndUserID,
        success: function (strOrObjArrUserAnswerDetail) {
            $(btnExportAndDownloadDataToCSVContainer).hide();
            $(divUserListContainer).empty();
            $(divUserListContainer).hide();
            $(divUserListPagerContainer).empty();
            $(divUserListPagerContainer).hide();

            if (strOrObjArrUserAnswerDetail === FAILED)
                alert(errorMessageOfRetry);
            else {
                let [objUserModel, objArrQuestionModel, objArrUserAnswerModel] =
                    strOrObjArrUserAnswerDetail;

                $(divUserAnswerContainer).empty();
                CreateUserDetail(objUserModel);
                CreateUserAnswerDetail(objArrQuestionModel, objArrUserAnswerModel);
                SetContainerSession(divUserAnswerContainer, currentUserAnswer);
                SetContainerShowStateSession(currentUserAnswerShowState, showState);
                SetContainerShowStateSession(currentUserListShowState, hideState);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}

var CreateStatistics = function (objArrQuestionModel, objArrUserAnswerModel) {
    $(divStatisticsContainer).append(
        `
            <div class="row gy-3">
                <div class="col-md-10">
                    <div id="divStatisticsInnerContainer" class="row gy-3">
                    </div>
                </div>
            </div>
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

        $("#divStatisticsInnerContainer").append(
            `
                <div id="${questionID}" class="col-12">
                    <div class="d-flex flex-column">
                        <h3>
                            ${i}. ${questionName} ${questionRequired ? "(必填)" : ""}
                        </h3>
                    </div>
                </div>
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
                $(`#divStatisticsInnerContainer #${questionID} div.d-flex.flex-column h3`).after(
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
                $(`#divStatisticsInnerContainer #${questionID} div.d-flex.flex-column h3`).after(
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
                $(`#divStatisticsInnerContainer #${questionID} div.d-flex.flex-column h3`).after(
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

        i++;
    }
}
var GetStatistics = function (strQuestionnaireID) {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireDetailDataHandler.ashx?Action=GET_STATISTICS",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrStatistics) {
            if (strOrObjArrStatistics === FAILED)
                alert(errorMessageOfRetry);
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
            alert(errorMessageOfAjax);
        }
    });
}