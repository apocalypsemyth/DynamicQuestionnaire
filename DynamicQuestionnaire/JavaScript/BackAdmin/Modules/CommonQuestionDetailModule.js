var GetCommonQuestionInputs = function () {
    let strCommonQuestionName = $(txtCommonQuestionName).val();

    let objCommonQuestion = {
        "commonQuestionName": strCommonQuestionName,
    };

    return objCommonQuestion;
}
var GetQuestionOfCommonQuestionInputs = function () {
    let strQuestionNameOfCommonQuestion = $(txtQuestionNameOfCommonQuestion).val();
    let strQuestionAnswerOfCommonQuestion = $(txtQuestionAnswerOfCommonQuestion).val();
    let strCategoryName = $(selectCategoryList).val();
    let strTypingName = $(selectTypingList).val();
    let boolQuestionRequiredOfCommonQuestion =
        $(ckbQuestionRequiredOfCommonQuestion).is(":checked");

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
        let questionAnswer = objQuestionOfCommonQuestion.questionAnswer;
        let hasSemicolon = questionAnswer.indexOf(";") !== -1;

        if (!hasSemicolon)
            arrErrorMsg.push(`請以 ";" 分隔多個答案。`);
        else {
            let strArrChecking =
                hasSemicolon
                    ? questionAnswer.trim().split(";")
                    : questionAnswer;

            if (Array.isArray(strArrChecking)) {
                let hasWhiteSpace = strArrChecking.some(item => /\s/.test(item));
                let hasStartOrEndWhiteSpace = strArrChecking.some(strChecking => !strChecking);

                if (hasWhiteSpace)
                    arrErrorMsg.push("請不要留空於分號之間。");
                if (hasStartOrEndWhiteSpace)
                    arrErrorMsg.push("請不要分號於開頭或結尾。");
            }
        }
    }

    if (arrErrorMsg.length > 0)
        return arrErrorMsg.join("\n");
    else
        return true;
}

var ResetCommonQuestionInputs = function () {
    $(txtCommonQuestionName).val("");
}
var GetCommonQuestion = function (strCommonQuestionID) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=GET_COMMONQUESTION",
        method: "POST",
        data: { "commonQuestionID": strCommonQuestionID },
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
var CreateCommonQuestion = function (objCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=CREATE_COMMONQUESTION",
        method: "POST",
        data: objCommonQuestion,
        success: function (strMsg) {
            if (strMsg === SUCCESSED) {
                let objQuestionOfCommonQuestion = GetQuestionOfCommonQuestionInputs();
                let isValidQuestionOfCommonQuestion =
                    CheckQuestionOfCommonQuestionInputs(objQuestionOfCommonQuestion);
                if (typeof isValidQuestionOfCommonQuestion === "string") {
                    alert(isValidQuestionOfCommonQuestion);
                    return;
                }

                CreateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var UpdateCommonQuestion = function (objCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=UPDATE_COMMONQUESTION",
        method: "POST",
        data: objCommonQuestion,
        success: function (updatedOrNotUpdatedObjCommonQuestion) {
            $(txtCommonQuestionName)
                .val(updatedOrNotUpdatedObjCommonQuestion.CommonQuestionName);
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}

var ResetQuestionOfCommonQuestionInputs = function () {
    $(selectCategoryList).val(commonQuestionOfCategoryName).change();
    $(selectTypingList).val(SINGLE_SELECT).change();
    $(txtQuestionNameOfCommonQuestion).val("");
    $(txtQuestionAnswerOfCommonQuestion).val("");
    $(ckbQuestionRequiredOfCommonQuestion).prop("checked", false);
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
var GetQuestionListOfCommonQuestion = function (strCommonQuestionID) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=GET_QUESTIONLIST_OF_COMMONQUESTION",
        method: "POST",
        data: { "commonQuestionID": strCommonQuestionID },
        beforeSend: function () {
            $(loadingProgressBarContainer).show();
            $(loadingProgressBar).animate({ width: startPercent }, 1);
        },
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            $(btnDeleteQuestionOfCommonQuestion).hide();
            $(divQuestionListOfCommonQuestionContainer).empty();

            if (strOrObjArrQuestionOfCommonQuestion === NULL + FAILED) {
                ResetCommonQuestionInputs();
                ResetQuestionOfCommonQuestionInputs();
                $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
            else if (strOrObjArrQuestionOfCommonQuestion === FAILED) {
                alert(errorMessageOfRetry);
                $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
            else if (strOrObjArrQuestionOfCommonQuestion === NULL) {
                $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
            else if (strOrObjArrQuestionOfCommonQuestion.length === 0) {
                $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
            else {
                $(btnDeleteQuestionOfCommonQuestion).show();

                if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestionOfCommonQuestion =
                        strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestionOfCommonQuestion.length === 0) {
                        $(btnDeleteQuestionOfCommonQuestion).hide();
                        $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    else {
                        CreateQuestionListOfCommonQuestionTable(filteredObjArrQuestionOfCommonQuestion);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    return;
                }
                CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
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
var CreateQuestionOfCommonQuestion = function (objQuestionOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=CREATE_QUESTION_OF_COMMONQUESTION",
        method: "POST",
        data: objQuestionOfCommonQuestion,
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            ResetQuestionOfCommonQuestionInputs();
            $(divQuestionListOfCommonQuestionContainer).empty();

            if (strOrObjArrQuestionOfCommonQuestion === NULL + FAILED) {
                alert(errorMessageOfRetry);
                $(btnDeleteQuestionOfCommonQuestion).hide();
                $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
            else {
                $(btnDeleteQuestionOfCommonQuestion).show();

                if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestionOfCommonQuestion =
                        strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestionOfCommonQuestion.length === 0) {
                        $(btnDeleteQuestionOfCommonQuestion).hide();
                        $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    else {
                        CreateQuestionListOfCommonQuestionTable(filteredObjArrQuestionOfCommonQuestion);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    return;
                }
                CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
var DeleteQuestionListOfCommonQuestion = function (strQuestionIDListOfCommonQuestion) {
    $.ajax({
        url: "/API/BackAdmin/CommonQuestionDetailDataHandler.ashx?Action=DELETE_QUESTIONLIST_OF_COMMONQUESTION",
        method: "POST",
        data: { "checkedQuestionIDListOfCommonQuestion": strQuestionIDListOfCommonQuestion },
        success: function (strOrObjArrQuestionOfCommonQuestion) {
            if (strOrObjArrQuestionOfCommonQuestion === NULL + FAILED)
                alert(errorMessageOfRetry);
            else {
                $(divQuestionListOfCommonQuestionContainer).empty();

                if (strOrObjArrQuestionOfCommonQuestion.length === 0) {
                    $(btnDeleteQuestionOfCommonQuestion).hide();
                    $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                    SetContainerSession(
                        divQuestionListOfCommonQuestionContainer,
                        currentQuestionListOfCommonQuestionTable
                    );
                }
                else {
                    $(btnDeleteQuestionOfCommonQuestion).show();

                    if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                        let filteredObjArrQuestionOfCommonQuestion =
                            strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                        if (filteredObjArrQuestionOfCommonQuestion.length === 0) {
                            $(btnDeleteQuestionOfCommonQuestion).hide();
                            $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                            SetContainerSession(
                                divQuestionListOfCommonQuestionContainer,
                                currentQuestionListOfCommonQuestionTable
                            );
                        }
                        else {
                            CreateQuestionListOfCommonQuestionTable(filteredObjArrQuestionOfCommonQuestion);
                            SetContainerSession(
                                divQuestionListOfCommonQuestionContainer,
                                currentQuestionListOfCommonQuestionTable
                            );
                        }
                        return;
                    }
                    CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                    SetContainerSession(
                        divQuestionListOfCommonQuestionContainer,
                        currentQuestionListOfCommonQuestionTable
                    );
                }
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
                alert(errorMessageOfRetry);
            else {
                $(selectCategoryList)
                    .val(strOrObjQuestionOfCommonQuestion.QuestionCategory)
                    .change();
                $(selectTypingList)
                    .val(strOrObjQuestionOfCommonQuestion.QuestionTyping)
                    .change();
                $(txtQuestionNameOfCommonQuestion)
                    .val(strOrObjQuestionOfCommonQuestion.QuestionName);
                $(txtQuestionAnswerOfCommonQuestion)
                    .val(strOrObjQuestionOfCommonQuestion.QuestionAnswer);
                $(ckbQuestionRequiredOfCommonQuestion)
                    .prop("checked", strOrObjQuestionOfCommonQuestion.QuestionRequired);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
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
                alert(errorMessageOfRetry);
            else {
                ResetQuestionOfCommonQuestionInputs();
                $(btnAddQuestionOfCommonQuestion).removeAttr("href");
                $(btnDeleteQuestionOfCommonQuestion).show();
                $(divQuestionListOfCommonQuestionContainer).empty();

                if (strOrObjArrQuestionOfCommonQuestion.some(item => item.IsDeleted)) {
                    let filteredObjArrQuestionOfCommonQuestion =
                        strOrObjArrQuestionOfCommonQuestion.filter(item2 => !item2.IsDeleted);

                    if (filteredObjArrQuestionOfCommonQuestion.length === 0) {
                        $(btnDeleteQuestionOfCommonQuestion).hide();
                        $(divQuestionListOfCommonQuestionContainer).append(emptyMessageOfQuestionList);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    else {
                        CreateQuestionListOfCommonQuestionTable(filteredObjArrQuestionOfCommonQuestion);
                        SetContainerSession(
                            divQuestionListOfCommonQuestionContainer,
                            currentQuestionListOfCommonQuestionTable
                        );
                    }
                    return;
                }
                CreateQuestionListOfCommonQuestionTable(strOrObjArrQuestionOfCommonQuestion);
                SetContainerSession(
                    divQuestionListOfCommonQuestionContainer,
                    currentQuestionListOfCommonQuestionTable
                );
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}